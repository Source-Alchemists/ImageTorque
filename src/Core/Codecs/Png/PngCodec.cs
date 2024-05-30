using System.Buffers.Binary;

namespace ImageTorque.Codecs.Png;

public sealed class PngCodec : ICodec
{
    public int HeaderSize { get; } = PngConstants.HeaderSize;
    public static PngCodec Instance { get; } = new PngCodec();
    public IImageDecoder Decoder { get; } = new PngDecoder();

    public bool IsSupportedFileFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && BinaryPrimitives.ReadUInt64BigEndian(header) == PngConstants.HeaderValue;
}
