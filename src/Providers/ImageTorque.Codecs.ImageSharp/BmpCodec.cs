using System.Buffers.Binary;

namespace ImageTorque.Codecs.ImageSharp;

/// <summary>
/// Represents a codec for the BMP file format.
/// </summary>
public sealed class BmpCodec : ICodec
{
    /// <inheritdoc/>
    public int HeaderSize => 14;

    /// <inheritdoc/>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header)
    {
        if (header.Length < HeaderSize)
        {
            return false;
        }

        short headerField = BinaryPrimitives.ReadInt16BigEndian(header[..2]);
        return headerField is 0x424D
            or 0x4241
            or 0x4349
            or 0x4350
            or 0x4943
            or 0x5054;
    }

    /// <inheritdoc/>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("bmp", StringComparison.InvariantCultureIgnoreCase) || encoderType.Equals("dib", StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc/>
    public IImageEncoder Encoder { get; } = new Encoder();

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new Decoder();
}
