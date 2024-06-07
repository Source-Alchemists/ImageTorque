using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents the information of a PNG image.
/// </summary>
internal sealed record PngInfos
{
    /// <summary>
    /// Gets or sets the bit depth of the image.
    /// </summary>
    public byte? BitDepth { get; init; }

    /// <summary>
    /// Gets or sets the color type of the image.
    /// </summary>
    public PngColorType? ColorType { get; init; }

    /// <summary>
    /// Gets or sets the resolution units of the image.
    /// </summary>
    public PixelResolutionUnit ResolutionUnits { get; init; }

    /// <summary>
    /// Gets or sets the horizontal resolution of the image.
    /// </summary>
    public double HorizontalResolution { get; init; }

    /// <summary>
    /// Gets or sets the vertical resolution of the image.
    /// </summary>
    public double VerticalResolution { get; init; }

    /// <summary>
    /// Gets or sets the color palette of the image.
    /// </summary>
    public byte[] ColorPalette { get; set; } = [];

    /// <summary>
    /// Gets or sets the color table of the image.
    /// </summary>
    public Rgb24[] ColorTable { get; set; } = [];

    /// <summary>
    /// Gets or sets the number of bytes per pixel of the image.
    /// </summary>
    public int BytesPerPixel { get; set; }

    /// <summary>
    /// Gets or sets the number of bytes per scanline of the image.
    /// </summary>
    public int BytesPerScanline { get; set; }

    /// <summary>
    /// Gets or sets the number of bytes per sample of the image.
    /// </summary>
    public int BytesPerSample { get; set; }
}
