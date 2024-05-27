using ImageTorque.Buffers;

namespace ImageTorque.Formats;

public interface IImageDecoder {
    IPixelBuffer Decode(Stream stream);
    IPixelBuffer Decode(Stream stream, Configuration configuration);
    ImageInfo Identify(Stream stream);
    ImageInfo Identify(Stream stream, Configuration configuration);
}
