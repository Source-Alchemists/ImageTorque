using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs;

public interface IImageEncoder
{
    public void Encode<TPixel>(IPixelBuffer<TPixel> pixelBuffer, Stream stream) where TPixel : unmanaged, IPixel;
}
