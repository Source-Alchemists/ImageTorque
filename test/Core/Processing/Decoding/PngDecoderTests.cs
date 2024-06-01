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
        var decoder = new PngDecoder();
        using var stream = new FileStream("./lena24.png", FileMode.Open);

        // Act
        ImageInfo imageInfo = decoder.Identify(stream);

        // Assert
        Assert.Equal(512, imageInfo.Width);
        Assert.Equal(512, imageInfo.Height);
        Assert.Equal(24, imageInfo.BitsPerPixel);
    }

    [Fact]
    public void Test_Decode_Rgb24()
    {
        // Arrange
        var decoder = new PngDecoder();
        using var stream = new FileStream("./lena24.png", FileMode.Open);

        // Act
        var pixelBuffer = decoder.Decode(stream) as PixelBuffer<Rgb24>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(pixelBuffer!.Pixels));
        Assert.Equal("01f8ff0e23a809255ae52a9857a7aeb0f89d92610205fef29c36c3d359ac3339", hash);
    }
}
