namespace ImageTorque.Pixels;

public interface IPixel<T> where T : unmanaged, IPixel<T>
{
    /// <summary>
    /// Gets the pixel info.
    /// </summary>
    PixelInfo PixelInfo { get; }
}