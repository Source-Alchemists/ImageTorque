
using System.Runtime.CompilerServices;

namespace ImageTorque.Codecs.ImageMagick;


/// <summary>
/// Represents a JPEG codec that implements the <see cref="ICodec"/> interface.
/// </summary>
public sealed class JpegCodec : ICodec
{
    /// <summary>
    /// Gets the size of the JPEG header.
    /// </summary>
    public int HeaderSize => 11;

    /// <summary>
    /// Gets the image encoder for JPEG format.
    /// </summary>
    public IImageEncoder Encoder { get; } = new Encoder();

    /// <summary>
    /// Gets the image decoder for JPEG format.
    /// </summary>
    public IImageDecoder Decoder { get; } = new Decoder();

    /// <summary>
    /// Determines whether the specified header is a supported decoder format for JPEG.
    /// </summary>
    /// <param name="header">The header bytes to check.</param>
    /// <returns><c>true</c> if the header is a supported decoder format for JPEG; otherwise, <c>false</c>.</returns>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && (IsJpeg(header) || IsJfif(header) || IsExif(header));

    /// <summary>
    /// Determines whether the specified encoder type is a supported encoder format for JPEG.
    /// </summary>
    /// <param name="encoderType">The encoder type to check.</param>
    /// <returns><c>true</c> if the encoder type is a supported encoder format for JPEG; otherwise, <c>false</c>.</returns>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("jpg", StringComparison.InvariantCultureIgnoreCase) || encoderType.Equals("jpeg", StringComparison.InvariantCultureIgnoreCase);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsJpeg(ReadOnlySpan<byte> header) =>
        header[0] == 0xFF && // 255
        header[1] == 0xD8; // 216

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsJfif(ReadOnlySpan<byte> header) =>
        header[6] == 0x4A && // J
        header[7] == 0x46 && // F
        header[8] == 0x49 && // I
        header[9] == 0x46 && // F
        header[10] == 0x00;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsExif(ReadOnlySpan<byte> header) =>
        header[6] == 0x45 && // E
        header[7] == 0x78 && // X
        header[8] == 0x69 && // I
        header[9] == 0x66 && // F
        header[10] == 0x00;
}
