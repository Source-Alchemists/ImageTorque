using System.Runtime.CompilerServices;

using ImageMagick;

using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick;

internal sealed class Encoder : IImageEncoder
{
    public void Encode<TPixel>(Stream stream, ReadOnlyPackedPixelBuffer<TPixel> pixelBuffer, string encoderType, int quality = 80) where TPixel : unmanaged, IPixel
    {
        MagickFormat format = encoderType switch
        {
            "png" => MagickFormat.Png,
            "bmp" => MagickFormat.Bmp,
            "jpg" => MagickFormat.Jpg,
            "tiff" => MagickFormat.Tiff,
            _ => throw new NotSupportedException("Unsupported encoder type.")
        };

        if (pixelBuffer is ReadOnlyPackedPixelBuffer<L8> l8)
        {
            SaveL8(stream, l8, format);
        }
        else if (pixelBuffer is ReadOnlyPackedPixelBuffer<L16> l16)
        {
            SaveL16(stream, l16, format);
        }
        else if (pixelBuffer is ReadOnlyPackedPixelBuffer<Rgb24> rgb24)
        {
            SaveRgb24(stream, rgb24, format);
        }
        else if (pixelBuffer is ReadOnlyPackedPixelBuffer<Rgb48> rgb48)
        {
            SaveRgb48(stream, rgb48, format);
        }
        else
        {
            throw new NotSupportedException("Unsupported pixel format.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SaveL8(Stream stream, ReadOnlyPackedPixelBuffer<L8> pixelBuffer, MagickFormat magickFormat)
    {
        PixelReadSettings settings = new(pixelBuffer.Width, pixelBuffer.Height, StorageType.Char, PixelMapping.RGB)
        {
            Mapping = "R"
        };

        using var image = new MagickImage();
        image.ReadPixels(pixelBuffer!.Pixels.AsByte(), settings);
        image.Write(stream, magickFormat);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SaveL16(Stream stream, ReadOnlyPackedPixelBuffer<L16> pixelBuffer, MagickFormat magickFormat)
    {
        PixelReadSettings settings = new(pixelBuffer.Width, pixelBuffer.Height, StorageType.Quantum, PixelMapping.RGB)
        {
            Mapping = "R"
        };

        using var image = new MagickImage();
        image.ReadPixels(pixelBuffer!.Pixels.AsUInt16(), settings);
        image.Write(stream, magickFormat);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SaveRgb24(Stream stream, ReadOnlyPackedPixelBuffer<Rgb24> pixelBuffer, MagickFormat magickFormat)
    {
        PixelReadSettings settings = new(pixelBuffer.Width, pixelBuffer.Height, StorageType.Char, PixelMapping.RGB);

        using var image = new MagickImage();
        image.ReadPixels(pixelBuffer!.Pixels.AsByte(), settings);
        image.Write(stream, magickFormat);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SaveRgb48(Stream stream, ReadOnlyPackedPixelBuffer<Rgb48> pixelBuffer, MagickFormat magickFormat)
    {
        PixelReadSettings settings = new(pixelBuffer.Width, pixelBuffer.Height, StorageType.Quantum, PixelMapping.RGB);

        using var image = new MagickImage();
        image.ReadPixels(pixelBuffer!.Pixels.AsUInt16(), settings);
        image.Write(stream, magickFormat);
    }

}
