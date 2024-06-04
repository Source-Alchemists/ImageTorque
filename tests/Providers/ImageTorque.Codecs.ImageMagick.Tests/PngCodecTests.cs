using System.Runtime.InteropServices;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick.Tests;

public class PngCodecTests
{
    [Fact]
    public void Test_Decode_Png_L8()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;

        using FileStream stream = File.OpenRead("./lena8.png");

        // Act
        using Buffers.IPixelBuffer<L8>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<L8>;

        var image = new Image(pixelBuffer!);
        using FileStream outStream = File.OpenWrite("./test8.png");
        var configuration = ConfigurationFactory.Build([new ImageSharp.PngCodec()]);
        image.Save(outStream, EncoderType.Png, configuration);

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(pixelBuffer!.Pixels));
        Assert.Equal("4ba5d347f3d6b97e75aa1aa543042552f5ac8489468c2a6b046f03d5e648eaab", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);
    }

    [Fact]
    public void Test_Decode_Png_L16()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;

        using FileStream stream = File.OpenRead("./lena16.png");

        // Act
        using Buffers.IPixelBuffer<L16>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<L16>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L16, byte>(pixelBuffer!.Pixels));
        Assert.Equal("5c9327dc3f2e32139f4b5e21ac35f892f0d216d48946ef7e825c3ea99289772b", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);
    }

    [Fact]
    public void Test_Decode_Png_Rgb24()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;

        using FileStream stream = File.OpenRead("./lena24.png");

        // Act
        using Buffers.IPixelBuffer<Rgb24>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<Rgb24>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(pixelBuffer!.Pixels));
        Assert.Equal("01f8ff0e23a809255ae52a9857a7aeb0f89d92610205fef29c36c3d359ac3339", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);
    }

    [Fact]
    public void Test_Decode_Png_Rgb48()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;

        using FileStream stream = File.OpenRead("./lena48.png");

        // Act
        using Buffers.IPixelBuffer<Rgb48>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<Rgb48>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb48, byte>(pixelBuffer!.Pixels));
        Assert.Equal("e849e7b08f171e5fb47f748f9bea2e1985555d7d05c8a720fb26e515b71e91d9", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);
    }
}
