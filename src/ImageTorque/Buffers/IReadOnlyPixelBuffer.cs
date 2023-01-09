using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public interface IReadOnlyPixelBuffer
{
    /// <summary>
    /// Gets the buffer.
    /// </summary>
    ReadOnlySpan<byte> Buffer { get; }

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
}

public interface IReadOnlyPixelBuffer<TPixel> : IReadOnlyPixelBuffer where TPixel : unmanaged
{
    /// <summary>
    /// Gets the pixels.
    /// </summary>
    ReadOnlySpan<TPixel> Pixels { get; }

    /// <summary>
    /// Gets the pixel at the specified index.
    /// </summary>
    TPixel this[int x, int y] { get; }

    /// <summary>
    /// Gets the pixel at the specified index.
    /// </summary>
    TPixel this[int index] { get; }

    /// <summary>
    /// Gets the channel.
    /// </summary>
    ReadOnlySpan<TPixel> GetChannel(int channelIndex);
}