using System.Numerics;

namespace ImageTorque.Pixels;

public interface IPackedL3Pixel<T> : IPackedPixel<T>
    where T : unmanaged, INumber<T>
{
    T R { get; set; }
    T G { get; set; }
    T B { get; set; }
}
