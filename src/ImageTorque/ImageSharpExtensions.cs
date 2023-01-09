using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque;

internal static class ImageSharpExtensions
{
    public static SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb24> ToImageSharp(this ReadOnlyPackedPixelBuffer<Rgb24> pixelBuffer)
    {
        var image = SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgb24>(pixelBuffer.Buffer,
                                                                                                        pixelBuffer.Width,
                                                                                                        pixelBuffer.Height);
        return image;
    }

    public static SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgb48> ToImageSharp(this ReadOnlyPackedPixelBuffer<Rgb48> pixelBuffer)
    {
        var image = SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Rgb48>(pixelBuffer.Buffer,
                                                                                                        pixelBuffer.Width,
                                                                                                        pixelBuffer.Height);
        return image;
    }

    public static SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.L8> ToImageSharp(this ReadOnlyPackedPixelBuffer<Mono8> pixelBuffer)
    {
        var image = SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.L8>(pixelBuffer.Buffer,
                                                                                                    pixelBuffer.Width,
                                                                                                    pixelBuffer.Height);
        return image;
    }

    public static SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.L16> ToImageSharp(this ReadOnlyPackedPixelBuffer<Mono16> pixelBuffer)
    {
        var image = SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.L16>(pixelBuffer.Buffer,
                                                                                                    pixelBuffer.Width,
                                                                                                    pixelBuffer.Height);
        return image;
    }
}
