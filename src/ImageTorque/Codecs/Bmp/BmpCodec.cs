using System.Buffers.Binary;

namespace ImageTorque.Codecs.Bmp;

/// <summary>
/// Represents a codec for the BMP image format.
/// </summary>
public sealed class BmpCodec : ICodec
{
    /// <inheritdoc/>
    public int HeaderSize { get; } = BmpFileHeader.Size;

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new BmpDecoder();

    /// <inheritdoc/>
    public IImageEncoder Encoder { get; } = new BmpEncoder();

    /// <inheritdoc/>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header)
    {
        if (header.Length < HeaderSize)
        {
            return false;
        }

        short headerField = BinaryPrimitives.ReadInt16BigEndian(header[..2]);
        return headerField is BmpConstants.HeaderFields.Bitmap
            or BmpConstants.HeaderFields.BitmapArray
            or BmpConstants.HeaderFields.ColorIcon
            or BmpConstants.HeaderFields.ColorPointer
            or BmpConstants.HeaderFields.Icon
            or BmpConstants.HeaderFields.Pointer;
    }

    /// <inheritdoc/>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("bmp", StringComparison.InvariantCultureIgnoreCase);
}
