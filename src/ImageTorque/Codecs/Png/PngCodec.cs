using System.Buffers.Binary;

namespace ImageTorque.Codecs.Png;

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
    public bool IsSupportedEncoderFormat(EncoderType encoderType) => false;
}
