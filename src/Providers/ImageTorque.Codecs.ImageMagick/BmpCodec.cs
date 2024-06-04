using System.Buffers.Binary;

namespace ImageTorque.Codecs.ImageMagick;

/// <summary>
/// Represents a codec for the BMP image format.
/// </summary>
public sealed class BmpCodec : ICodec
{
    /// <summary>
    /// Gets the size of the BMP header.
    /// </summary>
    public int HeaderSize => 14;

    /// <summary>
    /// Determines if the given header is a supported decoder format for BMP images.
    /// </summary>
    /// <param name="header">The header of the image.</param>
    /// <returns><c>true</c> if the header is a supported decoder format; otherwise, <c>false</c>.</returns>
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

    /// <summary>
    /// Determines if the given encoder type is a supported encoder format for BMP images.
    /// </summary>
    /// <param name="encoderType">The encoder type.</param>
    /// <returns><c>true</c> if the encoder type is a supported encoder format; otherwise, <c>false</c>.</returns>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("bmp", StringComparison.InvariantCultureIgnoreCase);

    /// <summary>
    /// Gets the image encoder for BMP images.
    /// </summary>
    public IImageEncoder Encoder { get; } = new Encoder();

    /// <summary>
    /// Gets the image decoder for BMP images.
    /// </summary>
    public IImageDecoder Decoder { get; } = new Decoder();
}
