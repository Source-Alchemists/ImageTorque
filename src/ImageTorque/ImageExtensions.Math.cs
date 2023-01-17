using ImageTorque.Buffers;
using ImageTorque.Processing;

namespace ImageTorque;

/// <summary>
/// Image extensions.
/// </summary>
public static partial class ImageExtensions
{
    private static readonly ImageMath s_imageMath = new();

    /// <summary>
    /// Add two images together.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="image2">The image to add.</param>
    /// <returns>The added image.</returns>
    public static Image Add(this Image image, Image image2) => MathOperation(image, image2, ImageMathMode.Add);

    /// <summary>
    /// Subtract two images.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="image2">The image to subtract.</param>
    /// <returns>The subtracted image.</returns>
    public static Image Subtract(this Image image, Image image2) => MathOperation(image, image2, ImageMathMode.Subtract);

    /// <summary>
    /// Multiply two images.
    /// </summary>
    /// <param name="image">The image.</param>
    /// <param name="image2">The image to multiply.</param>
    /// <returns>The multiplied image.</returns>
    public static Image Multiply(this Image image, Image image2) => MathOperation(image, image2, ImageMathMode.Multiply);

    private static Image MathOperation(Image image1, Image image2, ImageMathMode mode)
    {
        if(image1.IsColor != image2.IsColor)
        {
            throw new InvalidOperationException("Cannot perform math operations on images with different color types.");
        }
        IReadOnlyPixelBuffer b1 = image1.GetPixelBuffer();
        Image tmpImage = image2;
        bool resized = false;
        if (image2.Width != image1.Width || image2.Height != image1.Height)
        {
            tmpImage = tmpImage.Resize(image1.Width, image2.Height, ResizeMode.Bicubic);
            resized = true;
        }

        IReadOnlyPixelBuffer b2 = tmpImage.GetPixelBuffer(image1.PixelFormat);
        var result = new Image(s_imageMath.Execute(new ImageMathParameters { InputA = b1, InputB = b2, ImageMathMode = mode }));
        if (resized)
        {
            tmpImage.Dispose();
        }

        return result;
    }
}
