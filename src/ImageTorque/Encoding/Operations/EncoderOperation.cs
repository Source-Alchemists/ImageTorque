using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Encoding;

public class EncoderOperation : Encoder
{
    public EncoderOperation()
    {
        AddOperation<ReadOnlyPackedPixelBuffer<Mono8>, object>(EncodeMono8);
        AddOperation<ReadOnlyPackedPixelBuffer<Mono16>, object>(EncodeMono16);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb24>, object>(EncodeRgb24);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb48>, object>(EncodeRgb48);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static object Save(SixLabors.ImageSharp.Image image, Stream stream, EncoderType encoderType, int quality)
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

        return null!;
    }

    private static object EncodeMono8(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<Mono8>;

        using var image = pixelBuffer!.ToImageSharp();
        var stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return null!;
    }

    private static object EncodeMono16(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<Mono16>;

        using var image = pixelBuffer!.ToImageSharp();
        var stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return null!;
    }

    private static object EncodeRgb24(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<Rgb24>;

        using var image = pixelBuffer!.ToImageSharp();
        var stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return null!;
    }

    private static object EncodeRgb48(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<Rgb48>;

        using var image = pixelBuffer!.ToImageSharp();
        var stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return null!;
    }
}
