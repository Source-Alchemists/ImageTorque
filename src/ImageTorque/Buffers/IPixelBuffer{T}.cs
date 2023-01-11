namespace ImageTorque.Buffers;

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
