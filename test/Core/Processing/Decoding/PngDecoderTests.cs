using System.Runtime.InteropServices;
using ImageTorque.Buffers;
using ImageTorque.Codecs.Png;
using ImageTorque.Pixels;

namespace ImageTorque.Tests;

public class PngDecoderTests
{
    [Fact]
    public void Test_Identify()
    {
        // Arrange
        var format = new PngFormat();
        using var stream = new FileStream("./lena24.png", FileMode.Open);

        // Act
        ImageInfo imageInfo = format.Decoder.Identify(stream);

        // Assert
        Assert.Equal(512, imageInfo.Width);
        Assert.Equal(512, imageInfo.Height);
        Assert.Equal(24, imageInfo.BitsPerPixel);
    }

    [Fact]
    public void Test_Decode()
    {
        // Arrange
        var format = new PngFormat();
        using var stream = new FileStream("./lena24.png", FileMode.Open);
        using var sixImage = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgb24>(stream);
        stream.Position = 0;
        sixImage.DangerousTryGetSinglePixelMemory(out Memory<SixLabors.ImageSharp.PixelFormats.Rgb24> sixPixels);
        Span<Rgb24> sixBuffer = MemoryMarshal.Cast<SixLabors.ImageSharp.PixelFormats.Rgb24, Rgb24>(sixPixels.Span);

        // Act
        var pixelBuffer = format.Decoder.Decode(stream) as PixelBuffer<Rgb24>;
        Span<Rgb24> pixels = pixelBuffer!.Pixels;

        // Assert
        Assert.Equal(sixBuffer.ToArray(), pixels.ToArray());
    }
}
