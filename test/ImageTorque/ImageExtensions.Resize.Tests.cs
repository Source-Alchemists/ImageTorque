using System.Security.Cryptography;
using System.Text;
using ImageTorque.Pixels;

namespace ImageTorque.Tests;

public class ImageExtensionsResizeTests
{
    [Theory]
    [InlineData("30fe0cabf4f47230077118859f51d04a6d1ff5c3f9fa76ff339acac07a26447c", ResizeMode.NearestNeighbor)]
    [InlineData("2a50812f7c572a02c6cd2efa8c33c9e10344636ff359fe6e775f676e5ca3d0c1", ResizeMode.Bicubic)]
    [InlineData("5e3106ce7925862a5d66e5e1fb33dff72e0b8e896f8c479b9b4c69bb932bf888", ResizeMode.Bilinear)]
    public void Test_RealRgbImage(string expectedResult, ResizeMode mode)
    {
        // Arrange
        var image = Image.Load("./resources/lena24.png");

        // Act
        Image resizedImage = image.Resize(128, 156, mode);
        resizedImage.Save("test.png", Processing.EncoderType.Png);

        // Assert
        string hash = CreateHash(resizedImage.AsPacked<Rgb24>().Pixels.AsByte());
        Assert.Equal(expectedResult, hash);
    }

    private static string CreateHash(ReadOnlySpan<byte> imageData)
    {
        byte[] hashBytes = SHA256.HashData(imageData);
        var sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("x2"));
        }
        return sb.ToString();
    }
}
