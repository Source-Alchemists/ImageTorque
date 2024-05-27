namespace ImageTorque.Formats;

public interface IImageFormat {
    int HeaderSize { get; }
    //IImageEncoder Encode { get; }
    IImageDecoder Decoder { get; }

    bool IsSupportedFileFormat(ReadOnlySpan<byte> header);
}
