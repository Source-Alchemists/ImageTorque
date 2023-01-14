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
    /// Gets a value indicating whether this instance is color.
    /// </summary>
    bool IsColor { get; }

    /// <summary>
    /// Gets the pixel buffer type.
    /// </summary>
    PixelBufferType PixelBufferType { get; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    PixelType PixelType { get; }

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
