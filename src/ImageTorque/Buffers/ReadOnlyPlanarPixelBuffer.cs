using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public sealed record ReadOnlyPlanarPixelBuffer<TPixel> : ReadOnlyPixelBuffer<TPixel>
    where TPixel : unmanaged, IPlanarPixel<TPixel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyPlanarPixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="pixelBuffer">The pixel buffer.</param>
    public ReadOnlyPlanarPixelBuffer(IPixelBuffer<TPixel> pixelBuffer)
        : base(pixelBuffer)
    {
    }

    /// <inheritdoc/>
    public override ReadOnlySpan<TPixel> GetChannel(int channelIndex) => PixelBuffer.GetChannel(channelIndex);

    /// <summary>
    /// Gets the row of pixels at the specified <paramref name="channelIndex"/> and <paramref name="rowIndex"/>.
    /// </summary>
    /// <param name="channelIndex">The channel index.</param>
    /// <param name="rowIndex">The row index.</param>
    /// <returns>The row of pixels.</returns>
    public ReadOnlySpan<TPixel> GetRow(int channelIndex, int rowIndex)
    {
        var frame = channelIndex * Width * Height;
        return Pixels.Slice(frame + rowIndex * Width, Width);
    }
}