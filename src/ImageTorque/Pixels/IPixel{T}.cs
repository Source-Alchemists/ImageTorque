using System.Numerics;

namespace ImageTorque.Pixels;

/// <summary>
/// Represents a pixel.
/// </summary>
public interface IPixel<T> : IPixel where T : unmanaged, INumber<T>
{
    /// <summary>
    /// Get the minimum value.
    /// </summary>
    T Min { get; }
    /// <summary>
    /// Get the maximum value.
    /// </summary>
    T Max { get; }
}
