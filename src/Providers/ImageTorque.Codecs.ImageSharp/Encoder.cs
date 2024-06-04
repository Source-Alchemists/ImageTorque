using System.Runtime.CompilerServices;

using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageSharp;

internal sealed class Encoder : IImageEncoder
{
    public void Encode<TPixel>(Stream stream, ReadOnlyPackedPixelBuffer<TPixel> pixelBuffer, EncoderType encoderType, int quality = 80) where TPixel : unmanaged, IPixel
    {
        using SixLabors.ImageSharp.Image image = pixelBuffer switch
        {
            ReadOnlyPackedPixelBuffer<L8> l8 => l8.ToImageSharp(),
            ReadOnlyPackedPixelBuffer<L16> l16 => l16.ToImageSharp(),
            ReadOnlyPackedPixelBuffer<Rgb24> rgb24 => rgb24.ToImageSharp(),
            ReadOnlyPackedPixelBuffer<Rgb48> rgb48 => rgb48.ToImageSharp(),
            _ => throw new NotSupportedException("Unsupported pixel format.")
        };

        Save(image, stream, encoderType, quality);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Save(SixLabors.ImageSharp.Image image, Stream stream, EncoderType encoderType, int quality)
    {
        switch (encoderType)
        {
            case EncoderType.Png:
                SixLabors.ImageSharp.ImageExtensions.SaveAsPng(image, stream,
                                                    new SixLabors.ImageSharp.Formats.Png.PngEncoder()
                                                    {
                                                        CompressionLevel = SixLabors.ImageSharp.Formats.Png.PngCompressionLevel.BestSpeed
                                                    });
                break;
            case EncoderType.Bmp:
                SixLabors.ImageSharp.ImageExtensions.SaveAsBmp(image, stream);
                break;
            case EncoderType.Jpeg:
                SixLabors.ImageSharp.ImageExtensions.SaveAsJpeg(image, stream,
                                                    new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder()
                                                    {
                                                        Quality = quality
                                                    });
                break;
        }

        return true;
    }
}
