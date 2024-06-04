using System.Buffers.Binary;

namespace ImageTorque.Codecs.ImageMagick;

public sealed class PngCodec : ICodec
{
    private const ulong HeaderValue = 0x89504E470D0A1A0AUL;

    /// <inheritdoc/>
    public int HeaderSize => 8;

    /// <inheritdoc/>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && BinaryPrimitives.ReadUInt64BigEndian(header) == HeaderValue;

    /// <inheritdoc/>
    public bool IsSupportedEncoderFormat(EncoderType encoderType) => encoderType == EncoderType.Png;

    /// <inheritdoc/>
    public IImageEncoder Encoder => throw new NotSupportedException();

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new Decoder();
}
