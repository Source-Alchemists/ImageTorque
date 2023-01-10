using ImageTorque.Buffers;
using ImageTorque.Pixels;
using ImageTorque.Processing;

namespace ImageTorque;

public static class ImageExtensions
{
    private static readonly Encoder s_encoder = new();

    /// <summary>
    /// Saves the image to the specified stream.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="encodeType">Type of the encode.</param>
    /// <param name="quality">The quality.</param>
    public static void Save(this IImage image, Stream stream, EncoderType encodeType, int quality = 80)
    {
        IReadOnlyPixelBuffer pixelBuffer = image.PixelFormat switch
        {
            PixelFormat.Mono8 => image.AsPacked<Mono8>(),
            PixelFormat.Mono16 => image.AsPacked<Mono8>(),
            PixelFormat.Rgb24Packed => image.AsPacked<Rgb24>(),
            PixelFormat.Rgb48Packed => image.AsPacked<Rgb48>(),
            PixelFormat.Rgb888Planar => image.AsPacked<Rgb24>(),
            PixelFormat.Rgb161616Planar => image.AsPacked<Rgb48>(),
            PixelFormat.Mono => image.AsPacked<Mono8>(),
            PixelFormat.RgbPacked => image.AsPacked<Rgb24>(),
            _ => throw new NotSupportedException($"The pixel format {image.PixelFormat} is not supported."),
        };
        s_encoder.Execute(new EncoderParameters
        {
            Input = pixelBuffer,
            Stream = stream,
            EncoderType = encodeType,
            Quality = quality
        });
    }

    /// <summary>
    /// Saves the image as file in the specified format.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="path">The path.</param>
    /// <param name="encodeType">The encode type.</param>
    /// <param name="quality">The quality.</param>
    public static void Save(this IImage image, string path, EncoderType encodeType, int quality = 80)
    {
        using FileStream fileStream = File.OpenWrite(path);
        Save(image, fileStream, encodeType, quality);
    }

    /// <summary>
    /// Calculates the image clamp size. Keeping the aspect ratio.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="maxSize">The max size.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public static void CalculateClampSize(this IImage image, int maxSize, out int width, out int height)
    {
        if (image.Width > image.Height)
        {
            double per = (double)maxSize / image.Width;
            width = (int)(image.Width * per);
            height = (int)(image.Height * per);
        }
        else if (image.Height > image.Width)
        {
            double per = (double)maxSize / image.Height;
            width = (int)(image.Width * per);
            height = (int)(image.Height * per);
        }
        else
        {
            width = maxSize;
            height = maxSize;
        }
    }
}
