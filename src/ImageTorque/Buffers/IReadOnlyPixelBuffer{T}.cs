namespace ImageTorque.Buffers;

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
