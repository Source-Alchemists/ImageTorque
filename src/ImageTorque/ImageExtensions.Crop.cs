using ImageTorque.Buffers;
using ImageTorque.Processing;

namespace ImageTorque;

/// <summary>
/// Image extensions.
/// </summary>
public static partial class ImageExtensions
{
    private static readonly Crop s_crop = new();

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
}
