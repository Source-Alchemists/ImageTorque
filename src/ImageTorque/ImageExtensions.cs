using ImageTorque.Buffers;
using ImageTorque.Pixels;
using ImageTorque.Processing;

namespace ImageTorque;

public static class ImageExtensions
{
    private static readonly Encoder s_encoder = new();
    private static readonly Resizer s_resizer = new();
    private static readonly GrayscaleFilter s_grayscaleFilter = new();
    private static readonly Mirror s_mirror = new();
    private static readonly Crop s_crop = new();
    private static readonly Binarizer s_binarizer = new();

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
            PixelFormat.Mono8 => image.AsPacked<L8>(),
            PixelFormat.Mono16 => image.AsPacked<L8>(),
            PixelFormat.Rgb24Packed => image.AsPacked<Rgb24>(),
            PixelFormat.Rgb48Packed => image.AsPacked<Rgb48>(),
            PixelFormat.Rgb888Planar => image.AsPacked<Rgb24>(),
            PixelFormat.Rgb161616Planar => image.AsPacked<Rgb48>(),
            PixelFormat.Mono => image.AsPacked<L8>(),
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

    /// <summary>
    /// Resizes the image.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="resizeMode">The resize mode.</param>
    /// <returns>The resized image.</returns>
    public static Image Resize(this Image image, int width, int height, ResizeMode resizeMode = ResizeMode.Bicubic)
    {
        if (width < 1)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than 0.");
        if (height < 1)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than 0.");

        IReadOnlyPixelBuffer sourceBuffer = image.GetPixelBuffer();
        IPixelBuffer resizedPixelBuffer = s_resizer.Execute(new ResizerParameters
        {
            Input = sourceBuffer,
            Width = width,
            Height = height,
            ResizeMode = resizeMode
        });

        return new Image(resizedPixelBuffer);
    }

    /// <summary>
    /// Converts the image to grayscale.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <returns>The grayscale image.</returns>
    public static Image ToGrayscale(this Image image)
    {
        if (!image.IsColor)
        {
            return new Image(image);
        }

        IReadOnlyPixelBuffer sourceBuffer = null!;
        Type targetType = null!;
        switch (image.PixelFormat)
        {
            case PixelFormat.RgbPacked:
                sourceBuffer = image.AsPacked<Rgb>();
                targetType = typeof(PixelBuffer<LS>);
                break;
            case PixelFormat.Rgb24Packed:
                sourceBuffer = image.AsPacked<Rgb24>();
                targetType = typeof(PixelBuffer<L8>);
                break;
            case PixelFormat.Rgb48Packed:
                sourceBuffer = image.AsPacked<Rgb48>();
                targetType = typeof(PixelBuffer<L16>);
                break;
            case PixelFormat.RgbPlanar:
                sourceBuffer = image.AsPlanar<LS>();
                targetType = typeof(PixelBuffer<LS>);
                break;
            case PixelFormat.Rgb888Planar:
                sourceBuffer = image.AsPlanar<L8>();
                targetType = typeof(PixelBuffer<L8>);
                break;
            case PixelFormat.Rgb161616Planar:
                sourceBuffer = image.AsPlanar<L16>();
                targetType = typeof(PixelBuffer<L16>);
                break;
        }
        IPixelBuffer grayscaleBuffer = s_grayscaleFilter.Execute(new GrayscaleFilterParameters
        {
            Input = sourceBuffer,
            OutputType = targetType
        });

        return new Image(grayscaleBuffer);
    }

    /// <summary>
    /// Mirror the image.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="mirrorMode">The mirror mode.</param>
    /// <returns>The mirrored image.</returns>
    public static Image Mirror(this Image image, MirrorMode mirrorMode)
    {
        IReadOnlyPixelBuffer sourceBuffer = image.GetPixelBuffer();
        IPixelBuffer mirroredBuffer = s_mirror.Execute(new MirrorParameters
        {
            Input = sourceBuffer,
            MirrorMode = mirrorMode
        });

        return new Image(mirroredBuffer);
    }

    /// <summary>
    /// Crop the image.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="rectangle">The rectangle/region to crop.</param>
    /// <returns>The cropped image.</returns>
    public static Image Crop(this Image image, Rectangle rectangle)
    {
        IReadOnlyPixelBuffer sourceBuffer = image.GetPixelBuffer();
        IPixelBuffer croppedBuffer = s_crop.Execute(new CropParameters
        {
            Input = sourceBuffer,
            Rectangle = rectangle
        });

        return new Image(croppedBuffer);
    }

    /// <summary>
    /// Binary threshold the image.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="threshold">The threshold.</param>
    /// <param name="mode">The threshold mode.</param>
    /// <returns>The binary thresholded image.</returns>
    public static Image Binarize(this Image image, float threshold = 0.5f, BinaryThresholdMode mode = BinaryThresholdMode.Lumincance)
    {
        IReadOnlyPixelBuffer sourceBuffer = image.PixelFormat switch
        {
            PixelFormat.RgbPlanar => image.AsPacked<Rgb>(),
            PixelFormat.Rgb888Planar => image.AsPacked<Rgb24>(),
            PixelFormat.Rgb161616Planar => image.AsPacked<Rgb48>(),
            _ => image.GetPixelBuffer(),
        };
        IPixelBuffer binarizedBuffer = s_binarizer.Execute(new BinarizerParameters
        {
            Input = sourceBuffer,
            Threshold = threshold,
            Mode = mode
        });

        return new Image(binarizedBuffer);
    }

    internal static IReadOnlyPixelBuffer GetPixelBuffer(this Image image)
    {
        return image.PixelFormat switch
        {
            PixelFormat.Mono => image.AsPacked<LS>(),
            PixelFormat.Mono8 => image.AsPacked<L8>(),
            PixelFormat.Mono16 => image.AsPacked<L16>(),
            PixelFormat.RgbPacked => image.AsPacked<Rgb>(),
            PixelFormat.Rgb24Packed => image.AsPacked<Rgb24>(),
            PixelFormat.Rgb48Packed => image.AsPacked<Rgb48>(),
            PixelFormat.RgbPlanar => image.AsPlanar<LS>(),
            PixelFormat.Rgb888Planar => image.AsPlanar<L8>(),
            PixelFormat.Rgb161616Planar => image.AsPlanar<L16>(),
            _ => throw new NotSupportedException($"The pixel format {image.PixelFormat} is not supported."),
        };
    }
}
