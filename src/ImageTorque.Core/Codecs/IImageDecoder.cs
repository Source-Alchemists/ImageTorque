using ImageTorque.Buffers;

namespace ImageTorque.Codecs;

/// <summary>
/// Represents an image decoder that can decode image data from a stream into a pixel buffer.
/// </summary>
public interface IImageDecoder {
    /// <summary>
    /// Decodes the image data from the specified stream into a pixel buffer.
    /// </summary>
    /// <param name="stream">The stream containing the image data.</param>
    /// <returns>The decoded pixel buffer.</returns>
    IPixelBuffer Decode(Stream stream);

    /// <summary>
    /// Decodes the image data from the specified stream into a pixel buffer using the specified configuration.
    /// </summary>
    /// <param name="stream">The stream containing the image data.</param>
    /// <param name="configuration">The configuration to use for decoding.</param>
    /// <returns>The decoded pixel buffer.</returns>
    IPixelBuffer Decode(Stream stream, IConfiguration configuration);
}
