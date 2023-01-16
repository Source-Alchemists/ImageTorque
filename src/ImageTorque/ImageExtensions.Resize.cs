using ImageTorque.Buffers;
using ImageTorque.Processing;

namespace ImageTorque;

/// <summary>
/// Image extensions.
/// </summary>
public static partial class ImageExtensions
{
    private static readonly Resizer s_resizer = new();

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
}
