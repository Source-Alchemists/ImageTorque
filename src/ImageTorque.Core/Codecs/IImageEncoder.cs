using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs;

/// <summary>
/// Represents an image encoder that encodes pixel data and writes the encoded data to a stream.
/// </summary>
public interface IImageEncoder
{
    /// <summary>
    /// Encodes the specified pixel buffer and writes the encoded data to the provided stream.
    /// </summary>
    /// <typeparam name="TPixel">The type of the pixel in the pixel buffer.</typeparam>
    /// <param name="stream">The stream to write the encoded data to.</param>
    /// <param name="pixelBuffer">The pixel buffer to encode.</param>
    /// <param name="encoderType">The type of the encoder to use.</param>
    /// <param name="quality">The quality of the encoded image (optional, default is 80).</param>
    /// <remarks>
    /// The pixel buffer must implement the <see cref="IPixel"/> interface.
    /// </remarks>
    public void Encode<TPixel>(Stream stream, ReadOnlyPackedPixelBuffer<TPixel> pixelBuffer, EncoderType encoderType, int quality = 80) where TPixel : unmanaged, IPixel;
}
