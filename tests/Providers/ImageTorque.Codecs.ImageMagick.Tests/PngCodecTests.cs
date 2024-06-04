using System.Runtime.InteropServices;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick.Tests;

public class PngCodecTests
{
    [Fact]
    public void Test_DecodeAndEncode_Png_L8()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena8.png");

        // Act
        using Buffers.IPixelBuffer<L8>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<L8>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(pixelBuffer!.Pixels));
        Assert.Equal("4ba5d347f3d6b97e75aa1aa543042552f5ac8489468c2a6b046f03d5e648eaab", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using FileStream outStream = File.Create("./test8.png");
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<L8>(), EncoderType.Png);

        // Assert
        Assert.True(File.Exists("./test8.png"));
        File.Delete("./test8.png");
    }

    [Fact]
    public void Test_DecodeAndEncode_Png_L16()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena16.png");

        // Act
        using Buffers.IPixelBuffer<L16>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<L16>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L16, byte>(pixelBuffer!.Pixels));
        Assert.Equal("5c9327dc3f2e32139f4b5e21ac35f892f0d216d48946ef7e825c3ea99289772b", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using FileStream outStream = File.Create("./test16.png");
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<L16>(), EncoderType.Png);

        // Assert
        Assert.True(File.Exists("./test16.png"));
        File.Delete("./test16.png");
    }

    [Fact]
    public void Test_DecodeAndEncode_Png_Rgb24()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena24.png");

        // Act
        using Buffers.IPixelBuffer<Rgb24>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<Rgb24>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(pixelBuffer!.Pixels));
        Assert.Equal("01f8ff0e23a809255ae52a9857a7aeb0f89d92610205fef29c36c3d359ac3339", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using FileStream outStream = File.Create("./test24.png");
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<Rgb24>(), EncoderType.Png);

        // Assert
        Assert.True(File.Exists("./test24.png"));
        File.Delete("./test24.png");
    }

    [Fact]
    public void Test_Decode_Png_Rgb48()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena48.png");

        // Act
        using Buffers.IPixelBuffer<Rgb48>? pixelBuffer = decoder.Decode(stream) as Buffers.IPixelBuffer<Rgb48>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb48, byte>(pixelBuffer!.Pixels));
        Assert.Equal("e849e7b08f171e5fb47f748f9bea2e1985555d7d05c8a720fb26e515b71e91d9", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using FileStream outStream = File.Create("./test48.png");
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<Rgb48>(), EncoderType.Png);

        // Assert
        Assert.True(File.Exists("./test48.png"));
        File.Delete("./test48.png");
    }
}
