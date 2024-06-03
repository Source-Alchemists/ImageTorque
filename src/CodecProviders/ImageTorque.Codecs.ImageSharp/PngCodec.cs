
using System.Buffers.Binary;

namespace ImageTorque.Codecs.ImageSharp;

/// <summary>
/// Represents a PNG codec for decoding PNG image files.
/// </summary>
public sealed class PngCodec : ICodec
{
    private const ulong HeaderValue = 0x89504E470D0A1A0AUL;

    /// <inheritdoc/>
    public int HeaderSize => 8;

    /// <inheritdoc/>
    public bool IsSupportedFileFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && BinaryPrimitives.ReadUInt64BigEndian(header) == HeaderValue;

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new Decoder();
}
