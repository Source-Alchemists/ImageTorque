using System.Security.Cryptography;
using System.Text;
using ImageTorque.Pixels;

namespace ImageTorque.Tests;

public class ImageExtensionsResizeTests
{
    [Theory]
    [InlineData("5634f0b4e4289c58a816acf4459bd651751f07cc8224edc8add31e5b631965f0", ResizeMode.NearestNeighbor)]
    [InlineData("59c262391f989a561b20c5c552f9e84ae6a102309d172fd9f6fbdce201aedda9", ResizeMode.Bicubic)]
    [InlineData("8c5233adb268c2530e73820ac31936b369c5cf4132d92ef0f058e6d4603061b4", ResizeMode.Bilinear)]
    public void Test_Resize_RealRgbImage(string expectedResult, ResizeMode mode)
    {
        // Arrange
        using var image = Image.Load("./resources/lena24.png");

        // Act
        using Image resizedImage = image.Resize(20, 20, mode);

        // Assert
        string hash = CreateHash(resizedImage.AsPacked<Rgb24>().Pixels[..400].AsByte());
        Assert.Equal(expectedResult, hash);
    }

    [Theory]
    [InlineData("d99ee2fde97ee3a30e335ce333d471136b5fae11d46cefe925a85f2038c7ece2", ResizeMode.NearestNeighbor)]
    [InlineData("4c6c323307de9fc37bddcfd3cd55a560e75dacab66bc012f1c775f6ff2b1ba31", ResizeMode.Bicubic)]
    [InlineData("1f06dc1ff42111021b77c7c9bb4af264ba0efec80cff4bf9e45043a0c1739325", ResizeMode.Bilinear)]
    public void Test_Resize_RealGrayscaleImage(string expectedResult, ResizeMode mode)
    {
        // Arrange
        using var image = Image.Load("./resources/lena8.bmp");

        // Act
        using Image resizedImage = image.Resize(20, 20, mode);

        // Assert
        string hash = CreateHash(resizedImage.AsPacked<L8>().Pixels[..400].AsByte());
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
