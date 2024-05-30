namespace ImageTorque.Codecs;

public interface ICodec {
    int HeaderSize { get; }
    //IImageEncoder Encode { get; }
    IImageDecoder Decoder { get; }

    bool IsSupportedFileFormat(ReadOnlySpan<byte> header);
}
