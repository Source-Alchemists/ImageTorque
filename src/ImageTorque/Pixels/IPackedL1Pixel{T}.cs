using System.Numerics;

namespace ImageTorque.Pixels;

public interface IPackedL1Pixel<T> : IPackedPixel<T>
    where T : unmanaged, INumber<T>
{
    T Value { get; set; }
}
