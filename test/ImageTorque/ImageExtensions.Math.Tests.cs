using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Tests;

public class ImageExtensionsMathTests
{
    [Fact]
    public void Test_AddImages()
    {
        // Arrange
        var buffer1 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(0) });
        var buffer2 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(255) });
        var image1 = new Image(buffer1);
        var image2 = new Image(buffer2);

        // Act
        Image resultImage = image1.Add(image2);

        // Assert
        Assert.NotNull(resultImage);
        Assert.Equal(new L8[]{new L8(255), new L8(255), new L8(255), new L8(255)}, resultImage.AsPacked<L8>().Pixels[..4].ToArray());
    }

    [Fact]
    public void Test_SubtractImages()
    {
        // Arrange
        var buffer1 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(0) });
        var buffer2 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(255) });
        var image1 = new Image(buffer1);
        var image2 = new Image(buffer2);

        // Act
        Image resultImage = image1.Subtract(image2);

        // Assert
        Assert.NotNull(resultImage);
        Assert.Equal(new L8[]{new L8(0), new L8(255), new L8(0), new L8(0)}, resultImage.AsPacked<L8>().Pixels[..4].ToArray());
    }

    [Fact]
    public void Test_MultiplyImages()
    {
        // Arrange
        var buffer1 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(0) });
        var buffer2 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(255) });
        var image1 = new Image(buffer1);
        var image2 = new Image(buffer2);

        // Act
        Image resultImage = image1.Multiply(image2);

        // Assert
        Assert.NotNull(resultImage);
        Assert.Equal(new L8[]{new L8(0), new L8(0), new L8(0), new L8(0)}, resultImage.AsPacked<L8>().Pixels[..4].ToArray());
    }

    [Fact]
    public void Test_AddImagesWithDifferentPixelBuffers()
    {
        // Arrange
        var buffer1 = new PixelBuffer<Rgb24>(2, 2, new Rgb24[] {
                                                                new Rgb24(0, 255, 0),
                                                                new Rgb24(255, 0, 0),
                                                                new Rgb24(0, 0, 255),
                                                                new Rgb24(0, 0, 0) });
        var buffer2 = new PixelBuffer<Rgb>(2, 2, new Rgb[] {
                                                                new Rgb(1f, 1f, 1f),
                                                                new Rgb(0f, 0f, 0f),
                                                                new Rgb(1f, 1f, 1f),
                                                                new Rgb(0f, 0f, 0f) });
        var image1 = new Image(buffer1);
        var image2 = new Image(buffer2);

        // Act
        Image resultImage = image1.Add(image2);

        // Assert
        Assert.NotNull(resultImage);
        Assert.Equal(new Rgb24[]{new Rgb24(255, 255, 255), new Rgb24(255, 0, 0), new Rgb24(255, 255, 255), new Rgb24(0, 0, 0)}, resultImage.AsPacked<Rgb24>().Pixels[..4].ToArray());
    }

    [Fact]
    public void Test_AddImageWithDifferentSize()
    {
        // Arrange
        var buffer1 = new PixelBuffer<L8>(2, 4, new L8[] {
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(255) });
        var buffer2 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(255) });
        var image1 = new Image(buffer1);
        var image2 = new Image(buffer2);

        // Act
        Image resultImage = image1.Add(image2);

        // Assert
        Assert.NotNull(resultImage);
        Assert.Equal(new L8[]{new L8(255), new L8(255), new L8(255), new L8(255), new L8(255), new L8(255), new L8(0), new L8(255)}, resultImage.AsPacked<L8>().Pixels[..8].ToArray());
    }

    [Fact]
    public void Test_AddImagesWithDifferentColorTypes()
    {
        // Arrange
        var buffer1 = new PixelBuffer<Rgb24>(2, 2, new Rgb24[] {
                                                                new Rgb24(0, 255, 0),
                                                                new Rgb24(255, 0, 0),
                                                                new Rgb24(0, 0, 255),
                                                                new Rgb24(0, 0, 0) });
        var buffer2 = new PixelBuffer<L8>(2, 2, new L8[] {
                                                                new L8(255),
                                                                new L8(0),
                                                                new L8(255),
                                                                new L8(255) });
        var image1 = new Image(buffer1);
        var image2 = new Image(buffer2);

        // Act
        Assert.Throws<InvalidOperationException>(() => image1.Add(image2));
    }
}
