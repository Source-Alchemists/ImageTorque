using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

using ImageTorque.Buffers;
using ImageTorque.Memory;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Bmp;

public sealed unsafe class BmpDecoder : IImageDecoder
{
    public IPixelBuffer Decode(Stream stream) => Decode(stream, Configuration.Default);

    public IPixelBuffer Decode(Stream stream, Configuration configuration)
    {
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
                return DecodeRgb(stream, infoHeader, inverted, colorPalette, bytesPerColorMapEntry);
            case BmpInfoHeader.CompressionType.RLE8:
            case BmpInfoHeader.CompressionType.RLE4:
            case BmpInfoHeader.CompressionType.BitFields:
            case BmpInfoHeader.CompressionType.AlphaBitFields:
            default:
                throw new NotSupportedException($"Bitmap format '{infoHeader.Compression}' not supported!");
        }
    }

    private static IPixelBuffer DecodeRgb(Stream stream, in BmpInfoHeader infoHeader, bool inverted, in byte[] colorPalette, int bytesPerColorMapEntry)
    {
        if (infoHeader.BitsPerPixel <= 8)
        {
            return DecodeFromRgbPalette(stream, infoHeader, inverted, colorPalette, bytesPerColorMapEntry);
        }
        return null!;
    }

    private static PixelBuffer<L8> DecodeFromRgbPalette(Stream stream, in BmpInfoHeader infoHeader, bool inverted, in byte[] colorPalette, int bytesPerColorMapEntry)
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
            int newY = Invert(y, infoHeader.Height, inverted);
            if (stream.Read(rowSpan) == 0)
            {
                throw new InvalidDataException("Bitmap row size invalid!");
            }

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
}
