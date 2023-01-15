using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

/// <summary>
/// Marshal operations for pixel buffers.
/// </summary>
public static class PixelBufferMarshal
{
    /// <summary>
    /// Creates a copy of the pixel buffer.
    /// </summary>
    /// <param name="pixelBuffer">The pixel buffer.</param>
    /// <returns>The copy of the pixel buffer.</returns>
    public static IPixelBuffer Copy(IPixelBuffer pixelBuffer)
    {
        PixelFormat pixelFormat = pixelBuffer.PixelFormat;
        int width = pixelBuffer.Width;
        int height = pixelBuffer.Height;

        IPixelBuffer newPixelBuffer = CreatePixelBuffer(pixelFormat, width, height);
        pixelBuffer.Buffer.CopyTo(newPixelBuffer.Buffer);

        return newPixelBuffer;
    }

    /// <summary>
    /// Gets the pixel format.
    /// </summary>
    /// <param name="pixelBufferType">The pixel buffer type.</param>
    /// <param name="pixelType">The pixel type.</param>
    /// <returns>The pixel format.</returns>
    public static PixelFormat GetPixelFormat(PixelBufferType pixelBufferType, PixelType pixelType)
    {
        return pixelBufferType switch
        {
            PixelBufferType.Packed => pixelType switch
            {
                PixelType.LF => PixelFormat.Mono,
                PixelType.L8 => PixelFormat.Mono8,
                PixelType.L16 => PixelFormat.Mono16,
                PixelType.Rgb => PixelFormat.RgbPacked,
                PixelType.Rgb24 => PixelFormat.Rgb24Packed,
                PixelType.Rgb48 => PixelFormat.Rgb48Packed,
                _ => PixelFormat.Unknown
            },
            PixelBufferType.Planar => pixelType switch
            {
                PixelType.Rgb => PixelFormat.RgbPlanar,
                PixelType.Rgb24 => PixelFormat.Rgb888Planar,
                PixelType.Rgb48 => PixelFormat.Rgb161616Planar,
                _ => PixelFormat.Unknown
            },
            _ => PixelFormat.Unknown
        };
    }

    /// <summary>
    /// Creates the pixel buffer.
    /// </summary>
    /// <param name="pixelFormat">The pixel format.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <returns>The pixel buffer.</returns>
    public static IPixelBuffer CreatePixelBuffer(PixelFormat pixelFormat, int width, int height)
    {
        return pixelFormat switch
        {
            PixelFormat.Mono => new PixelBuffer<LS>(width, height),
            PixelFormat.Mono8 => new PixelBuffer<L8>(width, height),
            PixelFormat.Mono16 => new PixelBuffer<L16>(width, height),
            PixelFormat.RgbPacked => new PixelBuffer<Rgb>(width, height),
            PixelFormat.Rgb24Packed => new PixelBuffer<Rgb24>(width, height),
            PixelFormat.Rgb48Packed => new PixelBuffer<Rgb48>(width, height),
            PixelFormat.RgbPlanar => new PlanarPixelBuffer<LS>(width, height),
            PixelFormat.Rgb888Planar => new PlanarPixelBuffer<L8>(width, height),
            PixelFormat.Rgb161616Planar => new PlanarPixelBuffer<L16>(width, height),
            _ => throw new NotImplementedException($"Pixel format {pixelFormat} is not supported.")
        };
    }
}
