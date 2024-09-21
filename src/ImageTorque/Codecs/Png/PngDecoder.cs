/*
 * Copyright 2024 Source Alchemists
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ImageTorque.Buffers;
using ImageTorque.Compression.Zlib;
using ImageTorque.Memory;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents a PNG image decoder that implements the <see cref="IImageDecoder"/> interface.
/// </summary>
internal class PngDecoder : IImageDecoder
{
    /// <inheritdoc/>
    public IPixelBuffer Decode(Stream stream) => Decode(stream, IConfiguration.Default);

    /// <inheritdoc/>
    public IPixelBuffer Decode(Stream stream, IConfiguration configuration)
    {
        _ = Identify(stream, configuration, out PngInfos pngMeta);
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

    /// <summary>
    /// Represents information about an image.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing the image data.</param>
    /// <returns>The <see cref="ImageInfo"/> containing the image information.</returns>
    public static ImageInfo Identify(Stream stream) => Identify(stream, IConfiguration.Default);

    /// <summary>
    /// Represents information about an image.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing the image data.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> to use for decoding.</param>
    /// <returns>The <see cref="ImageInfo"/> containing the image information.</returns>
    public static ImageInfo Identify(Stream stream, IConfiguration configuration) => Identify(stream, configuration, out _);

    private static IPixelBuffer<TPixel> Decode<TPixel>(Stream stream, PngInfos pngIno, IConfiguration configuration) where TPixel : unmanaged, IPixel
    {
        stream.Skip(PngConstants.HeaderSize);
        PngHeader header = new();
        PixelBuffer<TPixel>? pixelBuffer = null;
        Span<byte> buffer = stackalloc byte[20];
        IMemoryOwner<byte> scanline = null!;
        IMemoryOwner<byte> lastScanline = null!;
        PngChunk? lastChunk = null!;

        try
        {
            while (TryReadChunk(stream, buffer, configuration, out PngChunk chunk))
            {
                try
                {
                    switch (chunk.Type)
                    {
                        case PngChunkType.ImageHeader:
                            if (!Equals(header, default(PngHeader)))
                            {
                                throw new InvalidDataException("Invalid header!");
                            }

                            (header, pngIno) = PngChunkUtils.ReadHeaderChunk(pngIno, chunk.Data.Memory.Span);
                            break;
                        case PngChunkType.Palette:
                            pngIno.ColorPalette = chunk.Data.Memory.Span.ToArray();
                            break;
                        case PngChunkType.ImageData:
                            if (pixelBuffer is null)
                            {
                                (scanline, lastScanline) = InitializePixelBuffer(pngIno, header, out pixelBuffer);
                            }

                            ReadScanlines(stream, pixelBuffer, scanline, lastScanline,
                                            pngIno, header,
                                            chunk,
                                            () =>
                                            {
                                                if (lastChunk != null)
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
                                                    if (chunk.Type is PngChunkType.ImageData)
                                                    {
                                                        chunk.Data?.Dispose();
                                                        return chunk.Length;
                                                    }

                                                    lastChunk = chunk;
                                                }

                                                return chunk.Length;
                                            });
                            break;
                        case PngChunkType.ImageTrailer:
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
            lastChunk?.Data?.Dispose();
        }
    }

    private static ImageInfo Identify(Stream stream, IConfiguration configuration, out PngInfos pngMeta)
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
                    case PngChunkType.ImageHeader:
                        (header, pngMeta) = PngChunkUtils.ReadHeaderChunk(pngMeta, chunk.Data.Memory.Span);
                        break;
                    case PngChunkType.Palette:
                        pngMeta.ColorPalette = chunk.Data.Memory.Span.ToArray();
                        break;
                    case PngChunkType.ImageData:
                    case PngChunkType.ImageTrailer:
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

        ColorPaletteToColorTable(pngMeta);
        return new ImageInfo(header.Width, header.Height, CalculateBitsPerPixel(pngMeta, header));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool TryReadChunk(in Stream stream, Span<byte> buffer, IConfiguration configuration, out PngChunk chunk)
    {
        if (stream.Position >= stream.Length - 1)
        {
            chunk = default;
            return false;
        }

        if (!PngChunkUtils.TryReadChunkLength(stream, buffer, out int length))
        {
            chunk = default;
            return false;
        }

        while (length < 0)
        {
            if (!PngChunkUtils.TryReadChunkLength(stream, buffer, out length))
            {
                chunk = default;
                return false;
            }
        }

        PngChunkType type = PngChunkUtils.ReadChunkType(stream, buffer);

        long position = stream.Position;
        chunk = new PngChunk(
            length: (int)Math.Min(length, stream.Length - position),
            type: type,
            data: PngChunkUtils.ReadChunkData(stream, length));

        if (configuration.UseCrcValidation)
        {
            PngChunkUtils.ValidateChunk(stream, chunk, buffer);
        }
        else
        {
            stream.Skip(4);
        }

        if (type is PngChunkType.ImageData)
        {
            stream.Position = position;
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculateBitsPerPixel(PngInfos pngMeta, PngHeader header)
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
    private static int CalculateBytesPerPixel(PngInfos pngMeta, PngHeader header)
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
    private static int CalculateScanlineLength(PngInfos pngMeta, PngHeader header)
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
    private static (IMemoryOwner<byte>, IMemoryOwner<byte>) InitializePixelBuffer<TPixel>(PngInfos pngMeta, in PngHeader header, out PixelBuffer<TPixel> pixelBuffer) where TPixel : unmanaged, IPixel
    {
        pixelBuffer = new PixelBuffer<TPixel>(header.Width, header.Height);

        pngMeta.BytesPerPixel = CalculateBytesPerPixel(pngMeta, header);
        pngMeta.BytesPerScanline = CalculateScanlineLength(pngMeta, header) + 1;
        pngMeta.BytesPerSample = 1;
        if (header.BitDepth >= 8)
        {
            pngMeta.BytesPerSample = header.BitDepth / 8;
        }

        IMemoryOwner<byte> scanline = OptimizedMemoryPool<byte>.Shared.Rent(pngMeta.BytesPerScanline);
        IMemoryOwner<byte> lastScanline = OptimizedMemoryPool<byte>.Shared.Rent(pngMeta.BytesPerScanline);
        ColorPaletteToColorTable(pngMeta);
        return (scanline, lastScanline);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (IMemoryOwner<byte>, IMemoryOwner<byte>) ReadScanlines<TPixel>(in Stream stream, PixelBuffer<TPixel> pixelBuffer, in IMemoryOwner<byte> scanline, in IMemoryOwner<byte> lastScanline, PngInfos info, in PngHeader header, PngChunk chunk, Func<int> getData)
        where TPixel : unmanaged, IPixel
    {
        using ZlibInflateStream inflateStream = new(stream, getData);
        inflateStream.AllocateNewBytes(chunk.Length, true);
        DeflateStream dataStream = inflateStream.DeflateStream!;
        return DecodePixelData(dataStream, pixelBuffer, scanline, lastScanline, info, header);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static (IMemoryOwner<byte>, IMemoryOwner<byte>) DecodePixelData<TPixel>(in DeflateStream compressedStream, in PixelBuffer<TPixel> pixelBuffer, IMemoryOwner<byte> scanline, IMemoryOwner<byte> lastScanline, in PngInfos info, in PngHeader header)
        where TPixel : unmanaged, IPixel
    {
        int currentRow = 0;
        int currentRowBytesRead = 0;
        int height = header.Height;

        while (currentRow < height)
        {
            int bytesPerFrameScanline = CalculateScanlineLength(info, header) + 1;
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
                    FilterSub.Decode(scanSpan, info.BytesPerPixel);
                    break;
                case FilterType.Up:
                    FilterUp.Decode(scanSpan, prevSpan);
                    break;
                case FilterType.Average:
                    FilterAverage.Decode(scanSpan, prevSpan, info.BytesPerPixel);
                    break;
                case FilterType.Paeth:
                    FilterPaeth.Decode(scanSpan, prevSpan, info.BytesPerPixel);
                    break;
                default:
                    throw new NotSupportedException($"Unknown filter '{(FilterType)scanSpan[0]}'!");
            }

            ProcessDefilteredScanline(currentRow, scanSpan, pixelBuffer, info, header);
            (scanline, lastScanline) = (lastScanline, scanline);
            currentRow++;
        }

        return (scanline, lastScanline);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ProcessDefilteredScanline<TPixel>(int currentRow, ReadOnlySpan<byte> scanline, in PixelBuffer<TPixel> pixelBuffer, in PngInfos info, in PngHeader header)
        where TPixel : unmanaged, IPixel
    {
        Span<TPixel> destination = pixelBuffer.GetRow(currentRow);
        Span<TPixel> rowSpan = destination;
        ReadOnlySpan<byte> trimmed = scanline[1..];
        IMemoryOwner<byte>? buffer = null;
        try
        {
            ReadOnlySpan<byte> scanlineSpan = TryScaleTo8Bit(trimmed, info.BytesPerScanline - 1, header.BitDepth, out buffer) ? buffer.Memory.Span : trimmed;

            switch (info.ColorType)
            {
                case PngColorType.Grayscale:
                case PngColorType.GrayscaleWithAlpha:
                    ScanlineProcessor.ProcessGrayscale(header, scanlineSpan, rowSpan);

                    break;
                case PngColorType.Palette:
                    ScanlineProcessor.ProcessPalette(header, scanlineSpan, rowSpan, info.ColorTable);

                    break;
                case PngColorType.Rgb:
                case PngColorType.RgbWithAlpha:
                    ScanlineProcessor.ProcessRgb(header, scanlineSpan, rowSpan, info.BytesPerPixel, info.BytesPerSample);

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

        buffer = OptimizedMemoryPool<byte>.Shared.Rent(bytesPerScanline * 8 / bits);
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

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void ColorPaletteToColorTable(PngInfos info)
    {
        Span<byte> colorPalette = info.ColorPalette.AsSpan();
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

        info.ColorTable = colorTable;
    }
}
