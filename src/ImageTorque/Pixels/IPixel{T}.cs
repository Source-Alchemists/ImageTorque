using System.Numerics;

namespace ImageTorque.Pixels;

public interface IPixel<T> : IPixel where T : unmanaged, INumber<T>
{

}
