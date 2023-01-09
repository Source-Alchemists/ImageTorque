using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public sealed record ReadOnlyPackedPixelBuffer<TPixel> : ReadOnlyPixelBuffer<TPixel>
    where TPixel : unmanaged, IPackedPixel<TPixel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyPackedPixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="pixelBuffer">The pixel buffer.</param>
    public ReadOnlyPackedPixelBuffer(IPixelBuffer<TPixel> pixelBuffer)
        : base(pixelBuffer)
    {
    }

    /// <inheritdoc/>
    public override ReadOnlySpan<TPixel> GetChannel(int channelIndex) => PixelBuffer.GetChannel(channelIndex);

    /// <summary>
    /// Gets the row of pixels at the specified <paramref name="rowIndex"/>.
    /// </summary>
    /// <param name="rowIndex">The row index.</param>
    /// <returns>The row of pixels.</returns>
    public ReadOnlySpan<TPixel> GetRow(int rowIndex) => PixelBuffer.GetRow(rowIndex);
}