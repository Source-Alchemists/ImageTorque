using System.Buffers;
using System.Runtime.InteropServices;
using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public abstract record PixelBuffer<TPixel> : IPixelBuffer<TPixel>
    where TPixel : unmanaged, IPixel
{
    private readonly Memory<TPixel> _memory;
    private readonly IMemoryOwner<TPixel> _memoryOwner;
    private bool _isDisposed = false;

    /// <summary>
    /// Gets the pixel at the specified index.
    /// </summary>
    public TPixel this[int index] { get => Pixels[index]; set => Pixels[index] = value; }

    /// <summary>
    /// Gets the pixel at the specified coordinates.
    /// </summary>
    public TPixel this[int x, int y]
    {
        get
        {
            int index = y * Width + x;
            return _memory.Span[index];
        }
        set
        {
            int index = y * Width + x;
            _memory.Span[index] = value;
        }
    }

    /// <summary>
    /// Gets the width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the number of channels.
    /// </summary>
    public int NumberOfChannels { get; }

    /// <summary>
    /// Gets the size.
    /// </summary>
    public int Size { get;}

    /// <summary>
    /// Gets the pixels.
    /// </summary>
    public Span<TPixel> Pixels => _memory.Span;

    /// <summary>
    /// Gets the buffer.
    /// </summary>
    public Span<byte> Buffer => MemoryMarshal.AsBytes(_memory.Span);

    /// <summary>
    /// Gets the pixel info.
    /// </summary>
    public PixelInfo PixelInfo { get; }

    /// <summary>
    /// Gets the pixel format.
    /// </summary>
    public PixelFormat PixelFormat { get; protected set; } = PixelFormat.Unknown;

    /// <summary>
    /// Gets the pixel buffer type.
    /// </summary>
    public abstract PixelBufferType PixelBufferType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    protected PixelBuffer(int width, int height)
    {
        Width = width;
        Height = height;
        TPixel pixel = Activator.CreateInstance<TPixel>();
        PixelInfo = pixel.PixelInfo;
        NumberOfChannels = pixel.PixelInfo.ChannelsPerImage;
        Size = width * height * NumberOfChannels;

        _memoryOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height * NumberOfChannels);
        _memory = _memoryOwner.Memory;
    }

    protected PixelBuffer(PixelBuffer<TPixel> other)
    {
        Width = other.Width;
        Height = other.Height;
        NumberOfChannels = other.NumberOfChannels;
        Size = other.Size;
        PixelInfo = other.PixelInfo;
        PixelFormat = other.PixelFormat;

        _memoryOwner = MemoryPool<TPixel>.Shared.Rent(Width * Height * NumberOfChannels);
        _memory = _memoryOwner.Memory;
        other.Pixels.CopyTo(_memory.Span);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the channel.
    /// </summary>
    /// <param name="channelIndex">Index of the channel.</param>
    /// <returns>Span of the channel.</returns>
    public abstract Span<TPixel> GetChannel(int channelIndex);

    /// <summary>
    /// Gets the row.
    /// </summary>
    /// <param name="rowIndex">Index of the row.</param>
    /// <returns>Span of the row.</returns>
    public Span<TPixel> GetRow(int rowIndex) => _memory.Span.Slice(rowIndex * Width, Width);

    /// <summary>
    /// Gets the pixel buffer as read only.
    /// </summary>
    /// <returns>The pixel buffer as read only.</returns>
    public abstract IReadOnlyPixelBuffer<TPixel> AsReadOnly();

    IReadOnlyPixelBuffer IPixelBuffer.AsReadOnly() => AsReadOnly();

    /// <summary>
    /// Indicates whether the specified <see cref="PixelBuffer<TPixel>"/>, is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="PixelBuffer<TPixel>"/> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="PixelBuffer<TPixel>"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(PixelBuffer<TPixel>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (Width != other.Width || Height != other.Height)
        {
            return false;
        }

        return Pixels.SequenceEqual(other.Pixels);
    }

    /// <summary>
    /// Gets a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Width, Height, PixelInfo);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _memoryOwner.Dispose();
            }

            _isDisposed = true;
        }
    }
}
