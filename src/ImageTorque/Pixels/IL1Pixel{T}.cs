using System.Numerics;

namespace ImageTorque.Pixels;

/// <summary>
/// Represents a pixel with a single lumincance value.
/// </summary>
public interface IL1Pixel<T> : IPixel<T> where T : unmanaged, INumber<T>
{
    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    T Value { get; set; }
}
