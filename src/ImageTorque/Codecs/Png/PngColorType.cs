namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents the color type of a PNG image.
/// </summary>
internal enum PngColorType : byte
{
    /// <summary>
    /// Grayscale color type.
    /// </summary>
    Grayscale = 0,

    /// <summary>
    /// RGB color type.
    /// </summary>
    Rgb = 2,

    /// <summary>
    /// Palette color type.
    /// </summary>
    Palette = 3,

    /// <summary>
    /// Grayscale with alpha color type.
    /// </summary>
    GrayscaleWithAlpha = 4,

    /// <summary>
    /// RGB with alpha color type.
    /// </summary>
    RgbWithAlpha = 6
}
