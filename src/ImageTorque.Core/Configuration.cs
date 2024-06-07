using ImageTorque.Codecs;

namespace ImageTorque;

/// <summary>
/// Represents the configuration settings for the ImageTorque library.
/// </summary>
public sealed record Configuration
{
    /// <summary>
    /// Gets or sets the default configuration instance.
    /// </summary>
    public static Configuration Default { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether to prefer contiguous image buffers.
    /// </summary>
    public bool PreferContiguousImageBuffers { get; init; } = true;

    /// <summary>
    /// Gets or sets the maximum size of the image header.
    /// </summary>
    public int MaxHeaderSize { get; init; }

    /// <summary>
    /// Gets or sets the collection of codecs used for image encoding and decoding.
    /// </summary>
    public IReadOnlyCollection<ICodec> Codecs { get; init; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether to use CRC validation for image data.
    /// </summary>
    public bool UseCrcValidation { get; init; } = false; // We don't use CRC validation by default, because we're running in a machine vision context
}
