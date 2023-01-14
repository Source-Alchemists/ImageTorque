using System.Numerics;

namespace ImageTorque.Pixels;

public interface IL1Pixel<T> : IPixel<T> where T : unmanaged, INumber<T>
{
    T Value { get; set; }
}
