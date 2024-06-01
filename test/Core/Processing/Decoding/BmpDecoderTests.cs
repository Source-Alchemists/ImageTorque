using System.Runtime.InteropServices;

using ImageTorque.Buffers;
using ImageTorque.Codecs.Bmp;
using ImageTorque.Pixels;

namespace ImageTorque.Tests;

public class BmpDecoderTests
{
    [Fact]
    public void Test_Decode_L8()
    {
        // Arrange
        var decoder = new BmpDecoder();
        using var stream = new FileStream("./lena8.bmp", FileMode.Open);

        // Act
        var pixelBuffer = decoder.Decode(stream) as PixelBuffer<L8>;

        // Assert
        Assert.NotNull(pixelBuffer);
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(pixelBuffer!.Pixels));
        Assert.Equal("eb6cd30ede15f638d0fccba36977d7c6a6ae3f31b8bd8f78dffdfcb47437acc7", hash);
    }

    [Fact]
    public void Test_Decode_Rgb24()
    {
        // Arrange
        var decoder = new BmpDecoder();
        using var stream = new FileStream("./lena24.bmp", FileMode.Open);

        // Act
        var pixelBuffer = decoder.Decode(stream) as PixelBuffer<Rgb24>;

        // Assert
        Assert.NotNull(pixelBuffer);
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(pixelBuffer!.Pixels));
        Assert.Equal("01f8ff0e23a809255ae52a9857a7aeb0f89d92610205fef29c36c3d359ac3339", hash);
    }
}
