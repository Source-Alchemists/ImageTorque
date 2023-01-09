using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public sealed record PlanarPixelBuffer<TPixel> : PixelBuffer<TPixel>
    where TPixel : unmanaged, IPlanarPixel<TPixel>
{
    /// <inheritdoc/>
    public override PixelBufferType PixelBufferType => PixelBufferType.Planar;


    /// <summary>
    /// Initializes a new instance of the <see cref="PlanarPixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public PlanarPixelBuffer(int width, int height) : base(width, height)
    {
        PixelFormat = PixelBufferMarshal.GetPixelFormat(PixelBufferType, PixelInfo.PixelType);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlanarPixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="pixels">The pixels.</param>
    public PlanarPixelBuffer(int width, int height, ReadOnlySpan<TPixel> pixels)
        : this(width, height)
    {
        pixels.CopyTo(Pixels);
    }

    /// <inheritdoc/>
    public override Span<TPixel> GetChannel(int channelIndex)
    {
        if (channelIndex < 0 || channelIndex >= NumberOfChannels)
        {
            throw new ArgumentOutOfRangeException(nameof(channelIndex));
        }

        var channelLength = Width * Height;
        return Pixels.Slice(channelIndex * channelLength, channelLength);
    }

    /// <summary>
    /// Gets the row of pixels at the specified <paramref name="rowIndex"/> and <paramref name="channelIndex"/>.
    /// </summary>
    /// <param name="channelIndex">The channel index.</param>
    /// <param name="rowIndex">The row index.</param>
    /// <returns>The row of pixels.</returns>
    public Span<TPixel> GetRow(int channelIndex, int rowIndex)
    {
        if (channelIndex < 0 || channelIndex >= NumberOfChannels)
        {
            throw new ArgumentOutOfRangeException(nameof(channelIndex));
        }

        if (rowIndex < 0 || rowIndex >= Height)
        {
            throw new ArgumentOutOfRangeException(nameof(rowIndex));
        }

        var frame = channelIndex * Width * Height;
        return Pixels.Slice(frame + rowIndex * Width, Width);
    }

    /// <summary>
    /// Gets the pixel buffer as read only.
    /// </summary>
    /// <returns>The pixel buffer as read only.</returns>
    public override IReadOnlyPixelBuffer<TPixel> AsReadOnly() => new ReadOnlyPlanarPixelBuffer<TPixel>(this);
}