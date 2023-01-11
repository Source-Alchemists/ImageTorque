namespace ImageTorque.Pixels;

public interface IPackedPixel<T> : IPixel<T> where T : unmanaged, IPixel<T>
{
}