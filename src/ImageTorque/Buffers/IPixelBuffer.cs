using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public interface IPixelBuffer : IDisposable
{
    /// <summary>
    /// Gets the buffer.
    /// </summary>
    Span<byte> Buffer { get; }

    /// <summary>
    /// Gets the width.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Gets the number of channels.
    /// </summary>
    int NumberOfChannels { get; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    int Size { get; }

    /// <summary>
    /// Gets the pixel buffer type.
    /// </summary>
    PixelBufferType PixelBufferType { get; }

    /// <summary>
    /// Gets the pixel info.
    /// </summary>
    PixelInfo PixelInfo { get; }

    /// <summary>
    /// Gets the pixel format.
    /// </summary>
    PixelFormat PixelFormat { get; }

    /// <summary>
    /// Gets the pixel buffer as read only.
    /// </summary>
    /// <returns>The pixel buffer as read only.</returns>
    IReadOnlyPixelBuffer AsReadOnly();
}

public interface IPixelBuffer<TPixel> : IPixelBuffer where TPixel : unmanaged
{
    /// <summary>
    /// Gets the pixels.
    /// </summary>
    Span<TPixel> Pixels { get; }

    /// <summary>
    /// Gets the pixel at the specified coordinates.
    /// </summary>
    TPixel this[int x, int y] { get; set; }

    /// <summary>
    /// Gets the pixel at the specified index.
    /// </summary>
    TPixel this[int index] { get; set; }

    /// <summary>
    /// Gets the channel.
    /// </summary>
    /// <param name="channelIndex">Index of the channel.</param>
    /// <returns>The channel.</returns>
    Span<TPixel> GetChannel(int channelIndex);

    /// <summary>
    /// Gets the row.
    /// </summary>
    /// <param name="rowIndex">Index of the row.</param>
    /// <returns>The row.</returns>
    Span<TPixel> GetRow(int rowIndex);

    /// <summary>
    /// Gets the pixel buffer as read only.
    /// </summary>
    /// <returns>The pixel buffer as read only.</returns>
    new IReadOnlyPixelBuffer<TPixel> AsReadOnly();
}