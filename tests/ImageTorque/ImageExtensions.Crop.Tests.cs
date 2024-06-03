using System.Security.Cryptography;
using System.Text;
using ImageTorque.Pixels;

namespace ImageTorque.Tests;

public class ImageExtensionsCropTests
{
    [Theory]
    [InlineData("bd4b1ec085b1c4103ea7830e2f120ae0b9a1749ab91638963d5c506f3c7c6d50", 0, 0, 200, 200)]
    [InlineData("33e2a078fd5f79107ab888f346f4d444eabec3f9eabaeea509116895c9caf222", 100, 100, 200, 200)]
    public void Test_Crop_RealRgbImage(string expectedResult, int x, int y, int width, int height)
    {
        // Arrange
        using var image = Image.Load("./lena24.png");

        // Act
        using Image result = image.Crop(new Rectangle { X = x, Y = y, Width = width, Height = height });

        // Assert
        string hash = TestHelper.CreateHash(result.AsPacked<Rgb24>().Pixels[..40000].AsByte());
        Assert.Equal(expectedResult, hash);
    }

    [Theory]
    [InlineData("e13bb06bac3bac4d86c04bc5f721a52047f11d4c1f5e98c1f3b3caeef19bea4b", 0, 0, 200, 200)]
    [InlineData("269e413ac761ea3082f92faa108c06100ecaa2100a1f3def391964d6f5286238", 100, 100, 200, 200)]
    public void Test_Crop_RealGrayscaleImage(string expectedResult, int x, int y, int width, int height)
    {
        // Arrange
        using var image = Image.Load("./lena8.bmp");

        // Act
        using Image result = image.Crop(new Rectangle { X = x, Y = y, Width = width, Height = height });

        // Assert
        string hash = TestHelper.CreateHash(result.AsPacked<L8>().Pixels[..40000].AsByte());
        Assert.Equal(expectedResult, hash);
    }
}
