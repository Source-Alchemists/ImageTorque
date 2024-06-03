using System.Numerics;

namespace ImageTorque.Pixels;

/// <summary>
/// Represents a pixel with three color channels.
/// </summary>
public interface IL3Pixel<T> : IPixel<T>
    where T : unmanaged, INumber<T>
{
    /// <summary>
    /// Gets or sets the red value.
    /// </summary>
    T R { get; set; }
    /// <summary>
    /// Gets or sets the green value.
    /// </summary>
    T G { get; set; }
    /// <summary>
    /// Gets or sets the blue value.
    /// </summary>
    T B { get; set; }
}
