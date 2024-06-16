using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImageTorque.Buffers;
using ImageTorque.Memory;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Bmp;

/// <summary>
/// Represents a BMP image encoder.
/// </summary>
/// <remarks>
/// The BMP encoder supports the following pixel types:
/// <list type="bullet">
/// <item><description><see cref="PixelType.L8"/></description></item>
/// <item><description><see cref="PixelType.Rgb24"/></description></item>
/// </list>
/// </remarks>
public sealed class BmpEncoder : IImageEncoder
{
    /// <inheritdoc/>
    public void Encode<TPixel>(Stream stream, ReadOnlyPackedPixelBuffer<TPixel> pixelBuffer, string encoderType, int quality = 80) where TPixel : unmanaged, IPixel
    {
        ushort bpp = 0;
        int colorPaletteSize = 1024;
        bpp = pixelBuffer.PixelType switch
        {
            PixelType.L8 => 8,
            PixelType.Rgb24 => 24,
            _ => throw new NotSupportedException($"The pixel type {pixelBuffer.PixelType} is not supported."),
        };

        if(bpp != 8)
        {
            colorPaletteSize = 0;
        }

        int bytesPerLine = (int)(4 * ((((uint)pixelBuffer.Width * bpp) + 31) / 32));
        int padding = bytesPerLine - (int)(pixelBuffer.Width * (bpp / 8F));

        var infoHeader = new BmpInfoHeader(
            version: BmpInfoHeader.HeaderVersionType.BITMAPINFOHEADER,
            size: 40,
            width: pixelBuffer.Width,
            height: pixelBuffer.Height,
            planes: 1,
            bitsPerPixel: bpp)
        {
            ImageSize = bytesPerLine * pixelBuffer.Height,
        };
        Span<byte> headerBuffer = stackalloc byte[40];
        var fileHeader = new BmpFileHeader(
            type:0x4D42,
            fileSize: 14 + 40 + colorPaletteSize + infoHeader.ImageSize,
            reserved: 0,
            offset: 14 + 40 + colorPaletteSize);
        WriteFileHeader(stream, headerBuffer, fileHeader);
        WriteInfoHeader(stream, headerBuffer, infoHeader);
        WriteImageData(stream, pixelBuffer, padding);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteFileHeader(Stream stream, Span<byte> buffer, BmpFileHeader fileHeader)
    {
        ref BmpFileHeader dest = ref Unsafe.As<byte, BmpFileHeader>(ref MemoryMarshal.GetReference(buffer));
        dest = fileHeader;
        stream.Write(buffer.Slice(0, BmpFileHeader.Size));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteInfoHeader(Stream stream, Span<byte> buffer, BmpInfoHeader infoHeader)
    {
        buffer.Clear();
        BinaryPrimitives.WriteInt32LittleEndian(buffer[..4], 40);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4, 4), infoHeader.Width);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(8, 4), infoHeader.Height);
        BinaryPrimitives.WriteInt16LittleEndian(buffer.Slice(12, 2), infoHeader.Planes);
        BinaryPrimitives.WriteUInt16LittleEndian(buffer.Slice(14, 2), infoHeader.BitsPerPixel);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(16, 4), (int)infoHeader.Compression);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(20, 4), infoHeader.ImageSize);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(24, 4), 0);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(28, 4), 0);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(32, 4), infoHeader.ClrUsed);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(36, 4), infoHeader.ClrImportant);
        stream.Write(buffer.Slice(0, 40));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteImageData<TPixel>(Stream stream, ReadOnlyPackedPixelBuffer<TPixel> pixelBuffer, int padding) where TPixel : unmanaged, IPixel
    {
        switch (pixelBuffer.PixelType)
        {
            case PixelType.L8:
                WriteL8Image(stream, pixelBuffer, padding);
                break;
            case PixelType.Rgb24:
                WriteRgb24Image(stream, pixelBuffer);
                break;
            default:
                throw new NotSupportedException($"The pixel type {pixelBuffer.PixelType} is not supported.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteL8Image<TPixel>(Stream stream, ReadOnlyPackedPixelBuffer<TPixel> pixelBuffer, int padding) where TPixel : unmanaged, IPixel
    {
        IMemoryOwner<byte> colorPaletteBuffer = OptimizedMemoryPool<byte>.Shared.Rent(1024);
        Span<byte> colorPalette = colorPaletteBuffer.Memory.Span;
        for (int i = 0; i <= 255; i++)
        {
            int index = i * 4;
            byte grayValue = (byte)i;
            colorPalette[index] = grayValue;
            colorPalette[index + 1] = grayValue;
            colorPalette[index + 2] = grayValue;
            colorPalette[index + 3] = 0;
        }
        stream.Write(colorPalette);
        for (int y = pixelBuffer.Height - 1; y >= 0; y--)
        {
            ReadOnlySpan<TPixel> row = pixelBuffer.GetRow(y);
            ReadOnlySpan<byte> rowBytes = MemoryMarshal.AsBytes(row);
            stream.Write(rowBytes);

            for (int i = 0; i < padding; i++)
            {
                stream.WriteByte(0);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void WriteRgb24Image<TPixel>(Stream stream, ReadOnlyPackedPixelBuffer<TPixel> pixelBuffer) where TPixel : unmanaged, IPixel
    {
        int rowBytesWithoutPadding = pixelBuffer.Width * 3;
        for (int y = pixelBuffer.Height - 1; y >= 0; y--)
        {
            ReadOnlySpan<TPixel> row = pixelBuffer.GetRow(y);
            ReadOnlySpan<byte> rowBytes = MemoryMarshal.AsBytes(row);
            for (int x = 0; x < rowBytesWithoutPadding; x += 3)
            {
                byte r = rowBytes[x];
                byte g = rowBytes[x + 1];
                byte b = rowBytes[x + 2];
                stream.Write([b, g, r]);
            }
        }
    }
}
