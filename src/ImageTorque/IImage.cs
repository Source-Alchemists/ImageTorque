using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque;

/// <summary>
/// Represents an image.
/// </summary>
public interface IImage : IDisposable
{
    /// <summary>
    /// Gets the width.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    int Size { get; }

    /// <summary>
    /// Gets a value indicating whether this is a color image.
    /// </summary>
    bool IsColor { get; }

    /// <summary>
    /// Gets the pixel format.
    /// </summary>
    PixelFormat PixelFormat { get; }

    /// <summary>
    /// Gets the image as a packed pixel buffer.
    /// </summary>
    /// <typeparam name="TPixel">The type of the pixel.</typeparam>
    /// <returns>The image as a packed pixel buffer.</returns>
    /// <remarks>The buffer is owned by the image and should not be disposed.</remarks>
    ReadOnlyPackedPixelBuffer<TPixel> AsPacked<TPixel>()
        where TPixel : unmanaged, IPixel;

    /// <summary>
    /// Gets the image as a planar pixel buffer.
    /// </summary>
    /// <typeparam name="TPixel">The type of the pixel.</typeparam>
    /// <returns>The image as a planar pixel buffer.</returns>
    /// <remarks>The buffer is owned by the image and should not be disposed.</remarks>
    ReadOnlyPlanarPixelBuffer<TPixel> AsPlanar<TPixel>()
        where TPixel : unmanaged, IPixel;
}
