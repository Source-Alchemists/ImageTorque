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
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<L8>(), "bmp");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("c63f5825a40943cc7538624f95952aee93f9b7b59930c89d521e0d178b8d10ef", outHash);
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
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<Rgb24>(), "bmp");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("015cd941382818563ab5fc7cc8dc58fc3394ee7f31bd6e0130f0b7e17aa6d2f9", outHash);
    }
}
