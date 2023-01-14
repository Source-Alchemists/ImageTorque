using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public sealed record PackedPixelBuffer<TPixel> : PixelBuffer<TPixel>
    where TPixel : unmanaged, IPixel
{
    /// <inheritdoc/>
    public override PixelBufferType PixelBufferType => PixelBufferType.Packed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PackedPixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public PackedPixelBuffer(int width, int height) : base(width, height)
    {
        PixelFormat = PixelBufferMarshal.GetPixelFormat(PixelBufferType, PixelInfo.PixelType);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PackedPixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="pixels">The pixels.</param>
    public PackedPixelBuffer(int width, int height, ReadOnlySpan<TPixel> pixels)
        : this(width, height)
    {
        pixels.CopyTo(Pixels);
    }

    /// <inheritdoc/>
    public override Span<TPixel> GetChannel(int channelIndex)
    {
        if (channelIndex != 0)
        {
            throw new ArgumentOutOfRangeException(nameof(channelIndex));
        }

        return Pixels;
    }

    /// <summary>
    /// Gets the pixel buffer as read only.
    /// </summary>
    /// <returns>The pixel buffer as read only.</returns>
    public override IReadOnlyPixelBuffer<TPixel> AsReadOnly() => new ReadOnlyPackedPixelBuffer<TPixel>(this);
}
