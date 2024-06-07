namespace ImageTorque;

/// <summary>
/// Represents information about an image.
/// </summary>
public sealed record ImageInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageInfo"/> class.
    /// </summary>
    public ImageInfo() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageInfo"/> class with the specified width, height, and bits per pixel.
    /// </summary>
    /// <param name="width">The width of the image.</param>
    /// <param name="height">The height of the image.</param>
    /// <param name="bitsPerPixel">The number of bits per pixel in the image.</param>
    public ImageInfo(int width, int height, int bitsPerPixel)
    {
        Width = width;
        Height = height;
        BitsPerPixel = bitsPerPixel;
    }

    /// <summary>
    /// Gets the width of the image.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the image.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the number of bits per pixel in the image.
    /// </summary>
    public int BitsPerPixel { get; }
}
