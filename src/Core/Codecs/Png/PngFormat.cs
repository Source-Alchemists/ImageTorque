using System.Buffers.Binary;

namespace ImageTorque.Codecs.Png;

public sealed class PngFormat : IImageFormat
{
    public int HeaderSize { get; } = PngConstants.HeaderSize;
    public static PngFormat Instance { get; } = new PngFormat();
    public IImageDecoder Decoder { get; } = new PngDecoder();

    public bool IsSupportedFileFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && BinaryPrimitives.ReadUInt64BigEndian(header) == PngConstants.HeaderValue;
}
