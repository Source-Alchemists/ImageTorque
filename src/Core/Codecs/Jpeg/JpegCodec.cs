
namespace ImageTorque.Codecs.Jpeg;

public sealed class JpegCodec : ICodec
{
    public int HeaderSize => 11;

    public IImageDecoder Decoder => throw new NotImplementedException();

    public bool IsSupportedFileFormat(ReadOnlySpan<byte> header) => throw new NotImplementedException();
}
