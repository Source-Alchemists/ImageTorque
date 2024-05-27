using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Formats;

public interface IImageEncoder
{
    public bool SkipMetadata { get; init; }

    public void Encode<TPixel>(PixelBuffer<TPixel> pixelBuffer, Stream stream) where TPixel : unmanaged, IPixel;
}
