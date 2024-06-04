
using System.Runtime.CompilerServices;

namespace ImageTorque.Codecs.ImageMagick;


/// <summary>
/// Represents a codec for TIFF (Tagged Image File Format) images.
/// </summary>
public sealed class TiffCodec : ICodec
{
    /// <summary>
    /// Gets the size of the TIFF header.
    /// </summary>
    public int HeaderSize => 8;

    /// <summary>
    /// Gets the image encoder for TIFF images.
    /// </summary>
    public IImageEncoder Encoder { get; } = new Encoder();

    /// <summary>
    /// Gets the image decoder for TIFF images.
    /// </summary>
    public IImageDecoder Decoder { get; } = new Decoder();

    /// <summary>
    /// Determines whether the specified header is a supported format for decoding TIFF images.
    /// </summary>
    /// <param name="header">The header of the image file.</param>
    /// <returns><c>true</c> if the header is a supported format for decoding TIFF images; otherwise, <c>false</c>.</returns>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header)
    {
        if (header.Length >= HeaderSize)
        {
            if (header[0] == 0x49 && header[1] == 0x49)
            {
                return LittleEndianTiff(header);
            }
            else if (header[0] == 0x4D && header[1] == 0x4D)
            {
                return BigEndianTiff(header);
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified encoder type is a supported format for encoding TIFF images.
    /// </summary>
    /// <param name="encoderType">The type of the encoder.</param>
    /// <returns><c>true</c> if the encoder type is a supported format for encoding TIFF images; otherwise, <c>false</c>.</returns>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("tif", StringComparison.InvariantCultureIgnoreCase) || encoderType.Equals("tiff", StringComparison.InvariantCultureIgnoreCase);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool BigEndianTiff(ReadOnlySpan<byte> header)
    {
        if (header[2] == 0 && header[3] == 0x2A)
        {
            return true;
        }
        else if (header[2] == 0 && header[3] == 0x2B && header[4] == 0 && header[5] == 8 && header[6] == 0 && header[7] == 0)
        {
            return true;
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool LittleEndianTiff(ReadOnlySpan<byte> header)
    {
        if (header[2] == 0x2A && header[3] == 0x00)
        {
            return true;
        }
        else if (header[2] == 0x2B && header[3] == 0x00 && header[4] == 8 && header[5] == 0 && header[6] == 0 && header[7] == 0)
        {
            return true;
        }

        return false;
    }
}
