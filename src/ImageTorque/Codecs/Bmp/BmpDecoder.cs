using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ImageTorque.Buffers;
using ImageTorque.Memory;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Bmp;

/// <summary>
/// Represents a decoder for BMP (Bitmap) images.
/// </summary>
public sealed class BmpDecoder : IImageDecoder
{
    /// <inheritdoc/>
    public IPixelBuffer Decode(Stream stream) => Decode(stream, Configuration.Default);

    /// <inheritdoc/>
    public IPixelBuffer Decode(Stream stream, Configuration configuration)
    {
        if(stream.CanSeek)
        {
            stream.Position = 0;
        }
        BmpFileHeader fileHeader = ReadFileHeader(stream);
        BmpInfoHeader infoHeader = ReadInfoHeader(stream);
        byte[] colorPalette = [];
        int colorMapSizeBytes = -1;
        int bytesPerColorMapEntry = 4;
        bool inverted = false;

        if (infoHeader.Height < 0)
        {
            inverted = true;
            infoHeader = infoHeader with { Height = -infoHeader.Height };
        }

        if (infoHeader.ClrUsed == 0)
        {
            if (infoHeader.BitsPerPixel is 1 or 2 or 4 or 8)
            {
                switch (fileHeader.Type)
                {
                    case BmpConstants.HeaderFields.Bitmap:
                        colorMapSizeBytes = fileHeader.Offset - BmpFileHeader.Size - infoHeader.Size;
                        int colorCountForBitDepth = 1 << infoHeader.BitsPerPixel;
                        bytesPerColorMapEntry = colorMapSizeBytes / colorCountForBitDepth;
                        bytesPerColorMapEntry = Math.Max(bytesPerColorMapEntry, 3);

                        break;
                    case BmpConstants.HeaderFields.BitmapArray:
                    case BmpConstants.HeaderFields.ColorIcon:
                    case BmpConstants.HeaderFields.ColorPointer:
                    case BmpConstants.HeaderFields.Icon:
                    case BmpConstants.HeaderFields.Pointer:
                        colorMapSizeBytes = 1 << infoHeader.BitsPerPixel * 3;
                        break;
                }
            }
        }
        else
        {
            colorMapSizeBytes = infoHeader.ClrUsed * bytesPerColorMapEntry;
        }

        if (colorMapSizeBytes > 0)
        {
            if (stream.Position > fileHeader.Offset - colorMapSizeBytes)
            {
                throw new InvalidDataException("Color map outside of bitmap offset!");
            }

            colorPalette = new byte[colorMapSizeBytes];

            if (stream.Read(colorPalette, 0, colorMapSizeBytes) == 0)
            {
                throw new InvalidDataException("Invalid color palette informations!");
            }
        }

        int skipAmount = fileHeader.Offset - (int)stream.Position;
        if ((skipAmount + (int)stream.Position) > stream.Length)
        {
            throw new InvalidDataException("Invalid file header offset!");
        }

        if (skipAmount > 0)
        {
            stream.Skip(skipAmount);
        }

        switch (infoHeader.Compression)
        {
            case BmpInfoHeader.CompressionType.RGB:
                return DecodeUncompressed(stream, infoHeader, inverted, colorPalette, bytesPerColorMapEntry);
            case BmpInfoHeader.CompressionType.RLE8:
            case BmpInfoHeader.CompressionType.RLE4:
            case BmpInfoHeader.CompressionType.BitFields:
            case BmpInfoHeader.CompressionType.AlphaBitFields:
            default:
                throw new NotSupportedException($"Bitmap format '{infoHeader.Compression}' not supported!");
        }
    }

    private static IPixelBuffer DecodeUncompressed(Stream stream, in BmpInfoHeader infoHeader, bool inverted, in byte[] colorPalette, int bytesPerColorMapEntry)
    {
        if (infoHeader.BitsPerPixel <= 8)
        {
            return DecodeL8(stream, infoHeader, inverted, colorPalette, bytesPerColorMapEntry);
        }
        else if (infoHeader.BitsPerPixel == 16)
        {
            return DecodeL16(stream, infoHeader, inverted, BmpConstants.Rgb16RMask, BmpConstants.Rgb16GMask, BmpConstants.Rgb16BMask);
        }
        else if (infoHeader.BitsPerPixel == 24)
        {
            return DecodeRgb24(stream, infoHeader, inverted);
        }

        throw new NotSupportedException($"Bitmap with bits per pixel '{infoHeader.BitsPerPixel}' not supprted!");
    }

    private static PixelBuffer<L8> DecodeL8(Stream stream, in BmpInfoHeader infoHeader, bool inverted, in byte[] colorPalette, int bytesPerColorMapEntry)
    {
        var pixelBuffer = new PixelBuffer<L8>(infoHeader.Width, infoHeader.Height);
        int pixelPerByte = 8 / infoHeader.BitsPerPixel;
        int arrayWidth = (infoHeader.Width + pixelPerByte - 1) / pixelPerByte;
        int mask = 0xFF >> (8 - infoHeader.BitsPerPixel);
        int padding = arrayWidth % 4;

        if (padding != 0)
        {
            padding = 4 - padding;
        }

        using IMemoryOwner<byte> row = OptimizedMemoryPool<byte>.Shared.Rent(arrayWidth + padding);
        Span<byte> rowSpan = row.Memory.Span;

        for (int y = 0; y < infoHeader.Height; y++)
        {
            if (stream.Read(rowSpan) == 0)
            {
                throw new InvalidDataException("Bitmap row size invalid!");
            }

            int newY = Invert(y, infoHeader.Height, inverted);
            int offset = 0;
            Span<L8> pixelRow = pixelBuffer.GetRow(newY);

            for (int x = 0; x < arrayWidth; x++)
            {
                int colOffset = x * pixelPerByte;
                for (int shift = 0, newX = colOffset; shift < pixelPerByte && newX < infoHeader.Width; shift++, newX++)
                {
                    int colorIndex = ((rowSpan[offset] >> (8 - infoHeader.BitsPerPixel - (shift * infoHeader.BitsPerPixel))) & mask) * bytesPerColorMapEntry;
                    Rgb24 source = Unsafe.As<byte, Rgb24>(ref colorPalette[colorIndex]);
                    pixelRow[newX] = new L8(ColorNumerics.Get8BitBT709Luminance(source.B, source.G, source.R)); // bitmap is stored as BGR so we have to switch the colors
                }

                offset++;
            }
        }

        return pixelBuffer;
    }

    private static PixelBuffer<L16> DecodeL16(Stream stream, in BmpInfoHeader infoHeader, bool inverted, int redMask, int greenMask, int blueMask)
    {
        var pixelBuffer = new PixelBuffer<L16>(infoHeader.Width, infoHeader.Height);
        int padding = CalculatePadding(infoHeader.Width, 2);
        int stride = (infoHeader.Width * 2) + padding;
        int rightShiftRedMask = CalculateRightShift((uint)redMask);
        int rightShiftGreenMask = CalculateRightShift((uint)greenMask);
        int rightShiftBlueMask = CalculateRightShift((uint)blueMask);
        int redMaskBits = CountBits((uint)redMask);
        int greenMaskBits = CountBits((uint)greenMask);
        int blueMaskBits = CountBits((uint)blueMask);

        using IMemoryOwner<byte> row = OptimizedMemoryPool<byte>.Shared.Rent(stride);
        Span<byte> rowSpan = row.Memory.Span;

        for (int y = 0; y < infoHeader.Height; y++)
        {
            if (stream.Read(rowSpan) == 0)
            {
                throw new InvalidDataException("Bitmap row size invalid!");
            }

            int newY = Invert(y, infoHeader.Height, inverted);
            int offset = 0;
            Span<L16> pixelRow = pixelBuffer.GetRow(newY);

            for (int x = 0; x < infoHeader.Width; x++)
            {
                short temp = BinaryPrimitives.ReadInt16LittleEndian(rowSpan[offset..]);
                int r = (redMaskBits == 5) ? GetByteFrom5BitValue((temp & redMask) >> rightShiftRedMask) : GetByteFrom6BitValue((temp & redMask) >> rightShiftRedMask);
                int g = (greenMaskBits == 5) ? GetByteFrom5BitValue((temp & greenMask) >> rightShiftGreenMask) : GetByteFrom6BitValue((temp & greenMask) >> rightShiftGreenMask);
                int b = (blueMaskBits == 5) ? GetByteFrom5BitValue((temp & blueMask) >> rightShiftBlueMask) : GetByteFrom6BitValue((temp & blueMask) >> rightShiftBlueMask);
                pixelRow[x] = new L16(ColorNumerics.Get8BitBT709Luminance((byte)r, (byte)g, (byte)b));
                offset += 2;
            }
        }

        return pixelBuffer;
    }

    private static PixelBuffer<Rgb24> DecodeRgb24(Stream stream, in BmpInfoHeader infoHeader, bool inverted)
    {
        var pixelBuffer = new PixelBuffer<Rgb24>(infoHeader.Width, infoHeader.Height);
        int bytesPerPixel = infoHeader.BitsPerPixel / 8;
        int padding = CalculatePadding(infoHeader.Width, bytesPerPixel);
        int rowLength = (infoHeader.Width * bytesPerPixel) + padding;

        using IMemoryOwner<byte> row = OptimizedMemoryPool<byte>.Shared.Rent(rowLength);
        Span<byte> rowSpan = row.Memory.Span;

        for (int y = 0; y < infoHeader.Height; y++)
        {
            if (stream.Read(rowSpan) == 0)
            {
                throw new InvalidDataException("Bitmap row size invalid!");
            }

            int newY = Invert(y, infoHeader.Height, inverted);
            Span<Rgb24> pixelRow = pixelBuffer.GetRow(newY);
            Span<Rgb24> sourceRow = MemoryMarshal.Cast<byte, Rgb24>(rowSpan);

            for (int x = 0; x < infoHeader.Width; x++)
            {
                Rgb24 source = sourceRow[x];
                pixelRow[x] = new Rgb24(source.B, source.G, source.R); // bitmap is stored as BGR so we have to switch the colors
            }
        }

        return pixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static BmpFileHeader ReadFileHeader(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[BmpFileHeader.Size];
        stream.Read(buffer.Slice(0, BmpFileHeader.Size));
        return BmpFileHeader.Parse(buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static BmpInfoHeader ReadInfoHeader(Stream stream)
    {
        Span<byte> buffer = stackalloc byte[(int)BmpInfoHeader.SupportedHeaderVersion];
        stream.Read(buffer.Slice(0, BmpInfoHeader.HeaderSize));
        BmpInfoHeader.HeaderVersionType headerVersion = (BmpInfoHeader.HeaderVersionType)BinaryPrimitives.ReadInt32LittleEndian(buffer);
        stream.Read(buffer.Slice(BmpInfoHeader.HeaderSize, (int)headerVersion - BmpInfoHeader.HeaderSize));
        return headerVersion switch
        {
            BmpInfoHeader.HeaderVersionType.BITMAPCOREHEADER => BmpInfoHeader.ParseAsBITMAPCOREHEADER(buffer),
            BmpInfoHeader.HeaderVersionType.OS22XBITMAPHEADER => BmpInfoHeader.ParseAsOS22XBITMAPHEADER(buffer),
            BmpInfoHeader.HeaderVersionType.OS22XBITMAPHEADER_ALT => BmpInfoHeader.ParseAsOS22XBITMAPHEADER_ALT(buffer),
            BmpInfoHeader.HeaderVersionType.BITMAPINFOHEADER => BmpInfoHeader.ParseAsBITMAPINFOHEADER(stream, buffer),
            BmpInfoHeader.HeaderVersionType.BITMAPV2INFOHEADER => BmpInfoHeader.ParseAsBITMAPV2INFOHEADER(buffer),
            BmpInfoHeader.HeaderVersionType.BITMAPV3INFOHEADER => BmpInfoHeader.ParseAsBITMAPV3INFOHEADER(buffer),
            BmpInfoHeader.HeaderVersionType.BITMAPV4HEADER => BmpInfoHeader.ParseAsBITMAPV4HEADER(buffer),
            BmpInfoHeader.HeaderVersionType.BITMAPV5HEADER => BmpInfoHeader.ParseAsBITMAPV5HEADER(buffer),
            _ => throw new NotSupportedException($"Header version '{headerVersion}' is not supported!"),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Invert(int y, int height, bool inverted) => (!inverted) ? height - y - 1 : y;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculatePadding(int width, int components)
    {
        int padding = width * components % 4;
        if (padding != 0)
        {
            padding = 4 - padding;
        }

        return padding;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte GetByteFrom5BitValue(int value) => (byte)((value << 3) | (value >> 2));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte GetByteFrom6BitValue(int value) => (byte)((value << 2) | (value >> 4));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CalculateRightShift(uint value)
    {
        int count = 0;
        while (value > 0)
        {
            if ((1 & value) == 0)
            {
                count++;
            }
            else
            {
                break;
            }

            value >>= 1;
        }

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CountBits(uint value)
    {
        int count = 0;
        while (value != 0)
        {
            count++;
            value &= value - 1;
        }

        return count;
    }
}
