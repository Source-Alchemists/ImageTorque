using ImageTorque.Buffers;
using ImageTorque.Codecs.Bmp;
using ImageTorque.Pixels;
using ImageTorque.Tests;

namespace ImageTorque.Processing.Tests;

public class BmpEncoderTests
{
    [Fact]
    public void Test_Encode_L8()
    {
        // Arrange
        var encoder = new BmpEncoder();
        var decoder = new BmpDecoder();
        using var inputStream = new FileStream("./lena8.bmp", FileMode.Open);
        var pixelBuffer = decoder.Decode(inputStream) as PixelBuffer<L8>;
        using var outputStream = new FileStream("./lena8_encoded.bmp", FileMode.Create);

        // Act
        encoder.Encode(outputStream, (ReadOnlyPackedPixelBuffer<L8>)pixelBuffer!.AsReadOnly(), "bmp");

        // Assert
        Assert.True(inputStream.Length > 0);
        outputStream.Position = 0;
        using var memStrem = new MemoryStream();
        outputStream.CopyTo(memStrem);
        string hash = TestHelper.CreateHash(memStrem.ToArray());
        Assert.Equal("51878087d22511c68c24449e2ef68286a685df6d5c734762bdbd8e052a1235cf", hash);
        if(File.Exists("./lena8_encoded.bmp"))
        {
            File.Delete("./lena8_encoded.bmp");
        }
    }

    [Fact]
    public void Test_Encode_Rgb24()
    {
        // Arrange
        var encoder = new BmpEncoder();
        var decoder = new BmpDecoder();
        using var inputStream = new FileStream("./lena24.bmp", FileMode.Open);
        var pixelBuffer = decoder.Decode(inputStream) as PixelBuffer<Rgb24>;
        using var outputStream = new FileStream("./lena24_encoded.bmp", FileMode.Create);

        // Act
        encoder.Encode(outputStream, (ReadOnlyPackedPixelBuffer<Rgb24>)pixelBuffer!.AsReadOnly(), "bmp");

        // Assert
        Assert.True(inputStream.Length > 0);
        outputStream.Position = 0;
        using var memStrem = new MemoryStream();
        outputStream.CopyTo(memStrem);
        string hash = TestHelper.CreateHash(memStrem.ToArray());
        Assert.Equal("d414a5c847b02454113912841b27f9eb469d5234c3fc6e6d0ddc66eeef2eb56f", hash);
        if(File.Exists("./lena24_encoded.bmp"))
        {
            File.Delete("./lena24_encoded.bmp");
        }
    }
}
