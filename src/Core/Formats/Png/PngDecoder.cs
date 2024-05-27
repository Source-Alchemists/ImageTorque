using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImageTorque.Buffers;
using ImageTorque.Compression.Zlib;
using ImageTorque.Pixels;

namespace ImageTorque.Formats.Png;

public class PngDecoder : IImageDecoder
{
    public IPixelBuffer Decode(Stream stream) => Decode(stream, Configuration.Default);

    public IPixelBuffer Decode(Stream stream, Configuration configuration)
    {
        _ = Identify(stream, configuration, out PngMeta pngMeta);
        stream.Position = 0;

        PngColorType color = pngMeta.ColorType.GetValueOrDefault();
        byte bits = pngMeta.BitDepth.GetValueOrDefault();

        switch (color)
        {
            case PngColorType.Grayscale:
                if (bits == 16)
                {
                    return Decode<L16>(stream, pngMeta, configuration);
                }

                return Decode<L8>(stream, pngMeta, configuration);

            case PngColorType.Palette:
            case PngColorType.Rgb:
                if (bits == 16)
                {
                    return Decode<Rgb48>(stream, pngMeta, configuration);
                }

                return Decode<Rgb24>(stream, pngMeta, configuration);

            default:
                throw new NotSupportedException($"Pixel type {color} is not supported!");
        }
    }

    private IPixelBuffer<TPixel> Decode<TPixel>(Stream stream, PngMeta pngMeta, Configuration configuration) where TPixel : unmanaged, IPixel
    {
        Stream processStream = stream;
        processStream.Skip(PngConstants.HeaderSize);
        PngHeader header = new();
        PixelBuffer<TPixel>? pixelBuffer = null;
        Span<byte> buffer = stackalloc byte[20];
        IMemoryOwner<byte> scanline = null!;
        IMemoryOwner<byte> lastScanline = null!;
        PngChunk? nextChunk = null!;

        try
        {
            while (TryReadChunk(processStream, buffer, configuration, out PngChunk chunk))
            {
                try
                {
                    switch (chunk.Type)
                    {
                        case PngChunkType.Header:
                            if (!Equals(header, default(PngHeader)))
                            {
                                throw new InvalidDataException("Invalid header!");
                            }

                            (header, pngMeta) = ReadHeaderChunk(pngMeta, chunk.Data.Memory.Span);
                            break;
                        case PngChunkType.Palette:
                            pngMeta.ColorPalette = chunk.Data.Memory.Span.ToArray();
                            break;
                        case PngChunkType.Data:
                            if (pixelBuffer is null)
                            {
                                (scanline, lastScanline) = InitializeImage(pngMeta, header, out pixelBuffer);
                                AssignColorPalette(pngMeta);
                            }

                            ReadScanlines(stream, pixelBuffer, scanline, lastScanline,
                                            pngMeta, header,
                                            chunk,
                                            () =>
                                            {
                                                if (nextChunk != null)
                                                {
                                                    return 0;
                                                }

                                                Span<byte> buffer = stackalloc byte[20];

                                                int length = stream.Read(buffer.Slice(0, 4));
                                                if (length == 0)
                                                {
                                                    return 0;
                                                }

                                                if (TryReadChunk(stream, buffer, configuration, out PngChunk chunk))
                                                {
                                                    if (chunk.Type is PngChunkType.Data)
                                                    {
                                                        chunk.Data?.Dispose();
                                                        return chunk.Length;
                                                    }

                                                    nextChunk = chunk;
                                                }

                                                return chunk.Length;
                                            });
                            break;
                        case PngChunkType.End:
                            break;
                        default:
                            break;
                    }
                }
                finally
                {
                    chunk.Data?.Dispose();
                }
            }

            if (pixelBuffer is null)
            {
                throw new InvalidDataException("No data!");
            }

            return pixelBuffer;
        }
        catch
        {
            pixelBuffer?.Dispose();
            throw;
        }
        finally
        {
            scanline?.Dispose();
            lastScanline?.Dispose();
            nextChunk?.Data?.Dispose();
        }
    }

    public ImageInfo Identify(Stream stream) => Identify(stream, Configuration.Default);

    public ImageInfo Identify(Stream stream, Configuration configuration) => Identify(stream, configuration, out _);

    private ImageInfo Identify(Stream stream, Configuration configuration, out PngMeta pngMeta)
    {
        pngMeta = new();
        PngHeader header = new();
        Stream processStream = stream;
        Span<byte> buffer = stackalloc byte[20];

        processStream.Skip(PngConstants.HeaderSize);

        while (TryReadChunk(processStream, buffer, configuration, out PngChunk chunk))
        {
            try
            {
                switch (chunk.Type)
                {
                    case PngChunkType.Header:
                        (header, pngMeta) = ReadHeaderChunk(pngMeta, chunk.Data.Memory.Span);
                        break;
                    case PngChunkType.Palette:
                        pngMeta.ColorPalette = chunk.Data.Memory.Span.ToArray();
                        break;
                    case PngChunkType.Data:
                    case PngChunkType.End:
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                chunk.Data?.Dispose();
            }
        }

        if (header.Width == 0 && header.Height == 0)
        {
            throw new InvalidDataException("Invalid header detected!");
        }

        AssignColorPalette(pngMeta);
        return new ImageInfo(header.Width, header.Height, CalculateBitsPerPixel(pngMeta, header));
    }

    private bool TryReadChunk(Stream stream, Span<byte> buffer, Configuration configuration, out PngChunk chunk)
    {
        if (stream.Position >= stream.Length - 1)
        {
            chunk = default;
            return false;
        }

        if (!TryReadChunkLength(stream, buffer, out int length))
        {
            chunk = default;
            return false;
        }

        while (length < 0)
        {
            if (!TryReadChunkLength(stream, buffer, out length))
            {
                chunk = default;
                return false;
            }
        }

        PngChunkType type = ReadChunkType(stream, buffer);

        long position = stream.Position;
        chunk = new PngChunk(
            length: (int)Math.Min(length, stream.Length - position),
            type: type,
            data: ReadChunkData(stream, length));

        if (configuration.UseCrcValidation)
        {
            ValidateChunk(stream, chunk, buffer);
        }
        else
        {
            stream.Skip(4);
        }

        if (type is PngChunkType.Data)
        {
            stream.Position = position;
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculateBitsPerPixel(PngMeta pngMeta, PngHeader header)
    {
        return pngMeta.ColorType switch
        {
            PngColorType.Grayscale or PngColorType.Palette => header.BitDepth,
            PngColorType.GrayscaleWithAlpha => header.BitDepth * 2,
            PngColorType.Rgb => header.BitDepth * 3,
            PngColorType.RgbWithAlpha => header.BitDepth * 4,
            _ => throw new NotSupportedException($"Color type {pngMeta.ColorType} not supproted!"),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculateBytesPerPixel(PngMeta pngMeta, PngHeader header)
        => pngMeta.ColorType
        switch
        {
            PngColorType.Grayscale => header.BitDepth == 16 ? 2 : 1,
            PngColorType.GrayscaleWithAlpha => header.BitDepth == 16 ? 4 : 2,
            PngColorType.Palette => 1,
            PngColorType.Rgb => header.BitDepth == 16 ? 6 : 3,
            _ => header.BitDepth == 16 ? 8 : 4,
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculateScanlineLength(PngMeta pngMeta, PngHeader header)
    {
        int mod = header.BitDepth == 16 ? 16 : 8;
        int scanlineLength = header.Width * header.BitDepth * pngMeta.BytesPerPixel;

        int amount = scanlineLength % mod;
        if (amount != 0)
        {
            scanlineLength += mod - amount;
        }

        return scanlineLength / mod;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (IMemoryOwner<byte>, IMemoryOwner<byte>) InitializeImage<TPixel>(PngMeta pngMeta, PngHeader header, out PixelBuffer<TPixel> pixelBuffer) where TPixel : unmanaged, IPixel
    {
        pixelBuffer = new PixelBuffer<TPixel>(header.Width, header.Height);

        pngMeta.BytesPerPixel = CalculateBytesPerPixel(pngMeta, header);
        pngMeta.BytesPerScanline = CalculateScanlineLength(pngMeta, header) + 1;
        pngMeta.BytesPerSample = 1;
        if (header.BitDepth >= 8)
        {
            pngMeta.BytesPerSample = header.BitDepth / 8;
        }

        IMemoryOwner<byte> scanline = MemoryPool<byte>.Shared.Rent(pngMeta.BytesPerScanline);
        scanline.Memory.Span.Clear();
        IMemoryOwner<byte> lastScanline = MemoryPool<byte>.Shared.Rent(pngMeta.BytesPerScanline);
        lastScanline.Memory.Span.Clear();
        return (scanline, lastScanline);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (IMemoryOwner<byte>, IMemoryOwner<byte>) ReadScanlines<TPixel>(Stream stream, PixelBuffer<TPixel> pixelBuffer, IMemoryOwner<byte> scanline, IMemoryOwner<byte> lastScanline, PngMeta pngMeta, PngHeader header, PngChunk chunk, Func<int> getData)
        where TPixel : unmanaged, IPixel
    {
        using ZlibInflateStream inflateStream = new(stream, getData);
        inflateStream.AllocateNewBytes(chunk.Length, true);
        DeflateStream dataStream = inflateStream.DeflateStream!;

        if (header.InterlaceMethod is PngInterlaceMode.Adam7)
        {
            return DecodeInterlacedPixelData(dataStream, pixelBuffer, scanline, lastScanline, pngMeta, header);
        }

        return DecodePixelData(dataStream, pixelBuffer, scanline, lastScanline, pngMeta, header);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (IMemoryOwner<byte>, IMemoryOwner<byte>) DecodeInterlacedPixelData<TPixel>(DeflateStream compressedStream, PixelBuffer<TPixel> pixelBuffer, IMemoryOwner<byte> scanline, IMemoryOwner<byte> lastScanline, PngMeta pngMeta, PngHeader header) where TPixel : unmanaged, IPixel
    {
        int currentRow = Adam7.FirstRow[0];
        int currentRowBytesRead = 0;
        int pass = 0;
        int endRow = header.Height;
        int width = header.Width;

        while (true)
        {
            int numColumns = Adam7.ComputeColumns(width, pass);

            if (numColumns == 0)
            {
                pass++;
                continue;
            }

            int bytesPerInterlaceScanline = CalculateScanlineLength(pngMeta, header) + 1;

            while (currentRow < endRow)
            {
                while (currentRowBytesRead < bytesPerInterlaceScanline)
                {
                    int bytesRead = compressedStream.Read(scanline.Memory.Span.Slice(currentRowBytesRead, bytesPerInterlaceScanline - currentRowBytesRead));
                    if (bytesRead <= 0)
                    {
                        return (scanline, lastScanline);
                    }

                    currentRowBytesRead += bytesRead;
                }

                currentRowBytesRead = 0;

                Span<byte> scanSpan = scanline.Memory.Span.Slice(0, bytesPerInterlaceScanline);
                Span<byte> prevSpan = lastScanline.Memory.Span.Slice(0, bytesPerInterlaceScanline);

                switch ((FilterType)scanSpan[0])
                {
                    case FilterType.None:
                        break;
                    case FilterType.Sub:
                        FilterSub.Decode(scanSpan, pngMeta.BytesPerPixel);
                        break;
                    case FilterType.Up:
                        FilterUp.Decode(scanSpan, prevSpan);
                        break;
                    case FilterType.Average:
                        FilterAverage.Decode(scanSpan, prevSpan, pngMeta.BytesPerPixel);
                        break;
                    case FilterType.Paeth:
                        FilterPaeth.Decode(scanSpan, prevSpan, pngMeta.BytesPerPixel);
                        break;
                    default:
                        throw new NotSupportedException($"Filter '{(FilterType)scanSpan[0]} is not supported!");
                }

                Span<TPixel> rowSpan = pixelBuffer.GetRow(currentRow);
                ProcessInterlacedDefilteredScanline(scanline.Memory.Span, rowSpan, pngMeta, header, pixelOffset: Adam7.FirstColumn[pass], increment: Adam7.ColumnIncrement[pass]);

                (scanline, lastScanline) = (lastScanline, scanline);

                currentRow += Adam7.RowIncrement[pass];
            }

            pass++;

            if (pass < 7)
            {
                currentRow = Adam7.FirstRow[pass];
            }
            else
            {
                break;
            }
        }

        return (scanline, lastScanline);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (IMemoryOwner<byte>, IMemoryOwner<byte>) DecodePixelData<TPixel>(
        DeflateStream compressedStream,
        PixelBuffer<TPixel> pixelBuffer,
        IMemoryOwner<byte> scanline,
        IMemoryOwner<byte> lastScanline,
        PngMeta pngMeta,
        PngHeader header)
        where TPixel : unmanaged, IPixel
    {
        int currentRow = 0;
        int currentRowBytesRead = 0;
        int height = header.Height;

        while (currentRow < height)
        {
            int bytesPerFrameScanline = CalculateScanlineLength(pngMeta, header) + 1;
            Span<byte> scanSpan = scanline.Memory.Span[..bytesPerFrameScanline];
            Span<byte> prevSpan = lastScanline.Memory.Span[..bytesPerFrameScanline];

            while (currentRowBytesRead < bytesPerFrameScanline)
            {
                int bytesRead = compressedStream.Read(scanSpan.Slice(currentRowBytesRead, bytesPerFrameScanline - currentRowBytesRead));
                if (bytesRead <= 0)
                {
                    return (scanline, lastScanline);
                }

                currentRowBytesRead += bytesRead;
            }

            currentRowBytesRead = 0;

            switch ((FilterType)scanSpan[0])
            {
                case FilterType.None:
                    break;
                case FilterType.Sub:
                    FilterSub.Decode(scanSpan, pngMeta.BytesPerPixel);
                    break;
                case FilterType.Up:
                    FilterUp.Decode(scanSpan, prevSpan);
                    break;
                case FilterType.Average:
                    FilterAverage.Decode(scanSpan, prevSpan, pngMeta.BytesPerPixel);
                    break;
                case FilterType.Paeth:
                    FilterPaeth.Decode(scanSpan, prevSpan, pngMeta.BytesPerPixel);
                    break;
                default:
                    throw new NotSupportedException($"Unknown filter '{(FilterType)scanSpan[0]}'!");
            }

            ProcessDefilteredScanline(currentRow, scanSpan, pixelBuffer, pngMeta, header);
            (scanline, lastScanline) = (lastScanline, scanline);
            currentRow++;
        }

        return (scanline, lastScanline);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessInterlacedDefilteredScanline<TPixel>(ReadOnlySpan<byte> scanline, Span<TPixel> destination,
        PngMeta pngMeta, PngHeader header, int pixelOffset = 0, int increment = 1) where TPixel : unmanaged, IPixel
    {
        Span<TPixel> rowSpan = destination;
        ReadOnlySpan<byte> trimmed = scanline[1..];
        IMemoryOwner<byte>? buffer = null;
        try
        {
            ReadOnlySpan<byte> scanlineSpan = TryScaleTo8Bit(trimmed, pngMeta.BytesPerScanline, header.BitDepth, out buffer) ? buffer!.Memory.Span : trimmed;

            switch (pngMeta.ColorType)
            {
                case PngColorType.Grayscale:
                case PngColorType.GrayscaleWithAlpha:
                    ScanlineProcessor.ProcessInterlacedGrayscaleScanline(header, scanlineSpan, rowSpan, (uint)pixelOffset, (uint)increment);
                    break;

                case PngColorType.Palette:
                    ScanlineProcessor.ProcessInterlacedPaletteScanline(header, scanlineSpan, rowSpan, (uint)pixelOffset, (uint)increment, pngMeta.ColorTable);
                    break;

                case PngColorType.Rgb:
                case PngColorType.RgbWithAlpha:
                    ScanlineProcessor.ProcessInterlacedRgbScanline(header, scanlineSpan, rowSpan, (uint)pixelOffset, (uint)increment, pngMeta.BytesPerPixel, pngMeta.BytesPerSample);
                    break;
            }
        }
        finally
        {
            buffer?.Dispose();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessDefilteredScanline<TPixel>(
        int currentRow,
        ReadOnlySpan<byte> scanline,
        PixelBuffer<TPixel> pixelBuffer,
        PngMeta pngMeta,
        PngHeader header)
        where TPixel : unmanaged, IPixel
    {
        Span<TPixel> destination = pixelBuffer.GetRow(currentRow);
        Span<TPixel> rowSpan = destination;
        ReadOnlySpan<byte> trimmed = scanline[1..];
        IMemoryOwner<byte>? buffer = null;
        try
        {
            ReadOnlySpan<byte> scanlineSpan = TryScaleTo8Bit(
                trimmed,
                pngMeta.BytesPerScanline - 1,
                header.BitDepth,
                out buffer)
            ? buffer.Memory.Span
            : trimmed;

            switch (pngMeta.ColorType)
            {
                case PngColorType.Grayscale:
                case PngColorType.GrayscaleWithAlpha:
                    ScanlineProcessor.ProcessGrayscaleScanline(header, scanlineSpan, rowSpan);

                    break;
                case PngColorType.Palette:
                    ScanlineProcessor.ProcessPaletteScanline(header, scanlineSpan, rowSpan, pngMeta.ColorTable);

                    break;
                case PngColorType.Rgb:
                case PngColorType.RgbWithAlpha:
                    ScanlineProcessor.ProcessRgbScanline(header, scanlineSpan, rowSpan, pngMeta.BytesPerPixel, pngMeta.BytesPerSample);

                    break;
            }
        }
        finally
        {
            buffer?.Dispose();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryScaleTo8Bit(ReadOnlySpan<byte> source, int bytesPerScanline, int bits, [NotNullWhen(true)] out IMemoryOwner<byte>? buffer)
    {
        if (bits >= 8)
        {
            buffer = null;
            return false;
        }

        buffer = MemoryPool<byte>.Shared.Rent(bytesPerScanline * 8 / bits);
        buffer.Memory.Span.Clear();
        ref byte sourceRef = ref MemoryMarshal.GetReference(source);
        ref byte resultRef = ref MemoryMarshal.GetReference(buffer.Memory.Span);
        int mask = 0xFF >> (8 - bits);
        int resultOffset = 0;

        for (int i = 0; i < bytesPerScanline; i++)
        {
            byte b = Unsafe.Add(ref sourceRef, (uint)i);
            for (int shift = 0; shift < 8; shift += bits)
            {
                int colorIndex = (b >> (8 - bits - shift)) & mask;
                Unsafe.Add(ref resultRef, (uint)resultOffset) = (byte)colorIndex;
                resultOffset++;
            }
        }

        return true;
    }

    private void ValidateChunk(Stream stream, in PngChunk chunk, Span<byte> buffer)
    {
        uint inputCrc = ReadChunkCrc(stream, buffer);
        if (chunk.IsCritical(PngCrcChunkHandling.IgnoreNonCritical))
        {
            Span<byte> chunkType = stackalloc byte[4];
            BinaryPrimitives.WriteUInt32BigEndian(chunkType, (uint)chunk.Type);

            Crc32 crc32 = new();
            crc32.Append(chunkType);
            crc32.Append(chunk.Data.Memory.Span.Slice(0, chunk.Length));

            if (crc32.GetCurrentHashAsUInt32() != inputCrc)
            {
                chunk.Data?.Dispose();
                throw new InvalidDataException("Invalid chunk crc!");
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IMemoryOwner<byte> ReadChunkData(Stream stream, int length)
    {
        if (length == 0)
        {
            return MemoryPool<byte>.Shared.Rent(0);
        }

        length = (int)Math.Min(length, stream.Length - stream.Position);
        IMemoryOwner<byte> buffer = MemoryPool<byte>.Shared.Rent(length);
        buffer.Memory.Span.Clear();

        stream.Read(buffer.Memory.Span.Slice(0, length));

        return buffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PngChunkType ReadChunkType(Stream stream, Span<byte> buffer)
    {
        if (stream.Read(buffer.Slice(0, 4)) == 4)
        {
            return (PngChunkType)BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }

        throw new InvalidDataException("Invalid chunk type!");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (PngHeader, PngMeta) ReadHeaderChunk(PngMeta pngMeta, ReadOnlySpan<byte> data)
    {
        var header = PngHeader.Parse(data);
        header.Validate();

        PngMeta meta = pngMeta with
        {
            BitDepth = header.BitDepth,
            ColorType = header.ColorType,
            InterlaceMethod = header.InterlaceMethod
        };

        return (header, meta);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryReadChunkLength(Stream stream, Span<byte> buffer, out int result)
    {
        if (stream.Read(buffer.Slice(0, 4)) == 4)
        {
            result = BinaryPrimitives.ReadInt32BigEndian(buffer);
            return true;
        }

        result = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SkipChunkDataAndCrc(Stream stream, in PngChunk chunk) => stream.Skip(chunk.Length + 4);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ReadChunkCrc(Stream stream, Span<byte> buffer)
    {
        uint crc = 0;
        if (stream.Read(buffer.Slice(0, 4)) == 4)
        {
            crc = BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }

        return crc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AssignColorPalette(PngMeta pngMeta)
    {
        Span<byte> colorPalette = pngMeta.ColorPalette.AsSpan();
        if (colorPalette.Length == 0)
        {
            return;
        }

        Rgb24[] colorTable = new Rgb24[colorPalette.Length / Unsafe.SizeOf<Rgb24>()];
        ReadOnlySpan<Rgb24> rgbTable = MemoryMarshal.Cast<byte, Rgb24>(colorPalette);

        for (int i = 0; i < colorTable.Length; i++)
        {
            Rgb24 sourcePixel = rgbTable[i];
            colorTable[i] = new(sourcePixel.R, sourcePixel.G, sourcePixel.B);
        }

        pngMeta.ColorTable = colorTable;
    }
}
