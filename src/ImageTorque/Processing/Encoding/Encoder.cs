using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal class Encoder : IProcessor<EncoderParameters, bool>
{
    public bool Execute(EncoderParameters parameters)
    {

        Type inputType = parameters.Input!.GetType();
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L8>))
        {
            return EncodeMono8(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L16>))
        {
            return EncodeMono16(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return EncodeRgb24(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return EncodeRgb48(parameters);
        }

        return false;
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

    private static bool EncodeMono8(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<L8>;

        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.L8> image = pixelBuffer!.ToImageSharp();
        Stream? stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return true;
    }

    private static bool EncodeMono16(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<L16>;

        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.L16> image = pixelBuffer!.ToImageSharp();
        Stream? stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return true;
    }

    private static bool EncodeRgb24(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<Rgb24>;

        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24> image = pixelBuffer!.ToImageSharp();
        Stream? stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return true;
    }

    private static bool EncodeRgb48(EncoderParameters parameters)
    {
        var pixelBuffer = parameters.Input as ReadOnlyPackedPixelBuffer<Rgb48>;

        using SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb48> image = pixelBuffer!.ToImageSharp();
        Stream? stream = parameters.Stream;
        Save(image, stream!, parameters.EncoderType, parameters.Quality);
        return true;
    }
}
