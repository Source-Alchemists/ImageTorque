using System.Buffers.Binary;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents a PNG codec that implements the <see cref="ICodec"/> interface.
/// </summary>
public sealed class PngCodec : ICodec
{
    /// <inheritdoc/>
    public int HeaderSize { get; } = PngConstants.HeaderSize;

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new PngDecoder();

    /// <inheritdoc/>
    public IImageEncoder Encoder => throw new NotSupportedException();

    /// <inheritdoc/>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && BinaryPrimitives.ReadUInt64BigEndian(header) == PngConstants.HeaderValue;

    /// <inheritdoc/>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("png", StringComparison.InvariantCultureIgnoreCase);
}
