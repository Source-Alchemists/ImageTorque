using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs;

public abstract class ImageEncoder : IImageEncoder
{
    public bool SkipMetadata { get; init; }

    public void Encode<TPixel>(IPixelBuffer<TPixel> pixelBuffer, Stream stream) where TPixel : unmanaged, IPixel
         => EncodeWithSeekableStream(pixelBuffer, stream, default);

    protected abstract void Encode<TPixel>(IPixelBuffer<TPixel> pixelBuffer, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel;

    private void EncodeWithSeekableStream<TPixel>(IPixelBuffer<TPixel> pixelBuffer, Stream stream, CancellationToken cancellationToken) where TPixel : unmanaged, IPixel
    {
        Configuration configuration = Configuration.Default;
        if (stream.CanSeek)
        {
            Encode(pixelBuffer, stream, cancellationToken);
        }
        else
        {
            using Microsoft.IO.RecyclableMemoryStream ms = configuration.StreamManager.GetStream();
            Encode(pixelBuffer, stream, cancellationToken);
            ms.Position = 0;
            ms.CopyTo(stream, configuration.StreamProcessingBufferSize);
        }
    }
}
