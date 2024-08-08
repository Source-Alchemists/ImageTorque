using ImageTorque.Codecs.Bmp;

namespace ImageTorque.Processing.Tests;

public class BmpCodecTests
{
    [Theory]
    [InlineData("./lena8.bmp")]
    [InlineData("./lena24.bmp")]
    public void Test_Load(string path)
    {
        // Arrange
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var stream = new FileStream(path, FileMode.Open);

        // Act
        var image = Image.Load(stream, configuration);

        // Assert
        Assert.NotNull(image);
    }

    [Theory]
    [InlineData("./lena8.bmp")]
    [InlineData("./lena24.bmp")]
    public void Test_Save(string path)
    {
        // Arrange
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var stream = new FileStream(path, FileMode.Open);
        var image = Image.Load(stream, configuration);
        using var outputStream = new MemoryStream();

        // Act
        image.Save(outputStream, "bmp", configuration);

        // Assert
        Assert.True(outputStream.Length > 0);
    }
}
