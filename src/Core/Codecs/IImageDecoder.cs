using ImageTorque.Buffers;

namespace ImageTorque.Codecs;

public interface IImageDecoder {
    IPixelBuffer Decode(Stream stream);
    IPixelBuffer Decode(Stream stream, Configuration configuration);
}
