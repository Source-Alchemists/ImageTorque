using System.Numerics;

namespace ImageTorque.Pixels;

public interface IL3Pixel<T> : IPixel<T>
    where T : unmanaged, INumber<T>
{
    T R { get; set; }
    T G { get; set; }
    T B { get; set; }
}
