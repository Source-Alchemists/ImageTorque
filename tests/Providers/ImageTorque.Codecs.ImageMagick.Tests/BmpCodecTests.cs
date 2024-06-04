using System.Runtime.InteropServices;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick.Tests;

public class BmpCodecTests
{
    [Fact]
    public void Test_DecodeAndEncode_L8()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena8.bmp");

        // Act
        using Buffers.IPixelBuffer<L8>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<L8>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(pixelBuffer!.Pixels));
        Assert.Equal("eb6cd30ede15f638d0fccba36977d7c6a6ae3f31b8bd8f78dffdfcb47437acc7", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using FileStream outStream = File.Create("./test8.bmp");
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<L8>(), "bmp");

        // Assert
        Assert.True(File.Exists("./test8.bmp"));
        File.Delete("./test8.bmp");
    }

    [Fact]
    public void Test_DecodeAndEncode_Rgb24()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena24.bmp");

        // Act
        using Buffers.IPixelBuffer<Rgb24>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<Rgb24>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(pixelBuffer!.Pixels));
        Assert.Equal("01f8ff0e23a809255ae52a9857a7aeb0f89d92610205fef29c36c3d359ac3339", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using FileStream outStream = File.Create("./test24.bmp");
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<Rgb24>(), "bmp");

        // Assert
        Assert.True(File.Exists("./test24.bmp"));
        File.Delete("./test24.bmp");
    }
}
