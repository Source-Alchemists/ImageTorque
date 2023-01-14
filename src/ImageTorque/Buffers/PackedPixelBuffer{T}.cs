using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public sealed record PackedPixelBuffer<T> : PixelBuffer<T>
    where T : unmanaged, IPixel
{
    /// <inheritdoc/>
    public override PixelBufferType PixelBufferType => PixelBufferType.Packed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PackedPixelBuffer{T}"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public PackedPixelBuffer(int width, int height) : base(width, height)
    {
        PixelFormat = PixelBufferMarshal.GetPixelFormat(PixelBufferType, PixelInfo.PixelType);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PackedPixelBuffer{T}"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="pixels">The pixels.</param>
    public PackedPixelBuffer(int width, int height, ReadOnlySpan<T> pixels)
        : this(width, height)
    {
        pixels.CopyTo(Pixels);
    }

    /// <inheritdoc/>
    public override Span<T> GetChannel(int channelIndex)
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
    public override IReadOnlyPixelBuffer<T> AsReadOnly() => new ReadOnlyPackedPixelBuffer<T>(this);
}
