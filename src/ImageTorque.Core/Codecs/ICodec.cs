namespace ImageTorque.Codecs;

/// <summary>
/// Represents a codec for encoding and decoding images.
/// </summary>
public interface ICodec {

    /// <summary>
    /// Gets the size of the header for the codec.
    /// </summary>
    int HeaderSize { get; }

    //IImageEncoder Encode { get; }

    /// <summary>
    /// Gets the image decoder used by the codec.
    /// </summary>
    IImageDecoder Decoder { get; }

    /// <summary>
    /// Determines whether the specified file format is supported by the codec.
    /// </summary>
    /// <param name="header">The header of the file to check.</param>
    /// <returns><c>true</c> if the file format is supported; otherwise, <c>false</c>.</returns>
    bool IsSupportedFileFormat(ReadOnlySpan<byte> header);
}
