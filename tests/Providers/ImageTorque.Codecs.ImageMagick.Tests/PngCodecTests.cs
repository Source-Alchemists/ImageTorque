using System.Runtime.InteropServices;

using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick.Tests;

public class PngCodecTests
{
    [Fact]
    public void Test_DecodeAndEncode_L8()
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
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<L8>(), "png");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("34cd28ec33c5de078c2c0a58d0085744fa55dcb9e8d88414be4fa1b9386855d8", outHash);
    }

    [Fact]
    public void Test_DecodeAndEncode_L16()
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
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<L16>(), "png");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("f29a6d3f21b066f10dbda62a5da59694a05911192b66ce8171213063025bef1c", outHash);
    }

    [Fact]
    public void Test_DecodeAndEncode_Rgb24()
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
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<Rgb24>(), "png");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("e1638703cb9199724d34194abc17cd46087093f610baa8e228ddb300227083f3", outHash);
    }

    [Fact]
    public void Test_Decode_Rgb48()
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
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<Rgb48>(), "png");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("afdac99bb9bf203ec4cccdeaa58c019186eadfe45fdbe0b1e29e97d098b984aa", outHash);
    }
}
