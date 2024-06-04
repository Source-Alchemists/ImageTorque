using System.Runtime.CompilerServices;

namespace ImageTorque.Codecs.ImageSharp;

/// <summary>
/// Represents a JPEG codec for decoding JPEG images.
/// </summary>
public sealed class JpegCodec : ICodec
{
    /// <inheritdoc/>
    public int HeaderSize { get; } = 11;

    /// <inheritdoc/>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && (IsJpeg(header) || IsJfif(header) || IsExif(header));

    /// <inheritdoc/>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("jpg", StringComparison.InvariantCultureIgnoreCase) || encoderType.Equals("jpeg", StringComparison.InvariantCultureIgnoreCase);

    /// <inheritdoc/>
    public IImageEncoder Encoder { get; } = new Encoder();

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new Decoder();

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
