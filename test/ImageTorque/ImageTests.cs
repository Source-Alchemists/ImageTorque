
using ImageTorque;
using ImageTorque.Buffers;
using ImageTorque.Pixels;
using ImageTorque.Processing;

namespace AyBorg.SDK.ImageProcessing.Tests;

public class ImageTests
{
    private readonly PackedPixelBuffer<Rgb> _packedPixelBufferRgb;
    private readonly PackedPixelBuffer<Rgb24> _packedPixelBufferRgb24;
    private readonly PlanarPixelBuffer<Rgb888> _planarPixelBufferRgb888;
    private readonly PackedPixelBuffer<Mono> _packedPixelBufferMono;
    private readonly PackedPixelBuffer<Mono8> _packedPixelBufferMono8;

    public ImageTests()
    {
        _packedPixelBufferRgb = new PackedPixelBuffer<Rgb>(2, 2, new[] {
                                                                new Rgb(0f, 0f, 0f),
                                                                new Rgb(0.0039215687f, 0.0078431373f, 0.0117647059f),
                                                                new Rgb(0.015686275f, 0.0196078432f, 0.0235294118f),
                                                                new Rgb(1f, 1f, 1f) });
        _packedPixelBufferRgb24 = new PackedPixelBuffer<Rgb24>(2, 2, new[] {
                                                                new Rgb24(0x00, 0x00, 0x00),
                                                                new Rgb24(0x01, 0x02, 0x03),
                                                                new Rgb24(0x04, 0x05, 0x06),
                                                                new Rgb24(0xFF, 0xFF, 0xFF) });
        _planarPixelBufferRgb888 = new PlanarPixelBuffer<Rgb888>(2, 2, new[] {
                                                                new Rgb888(0x00), new Rgb888(0x01), new Rgb888(0x04), new Rgb888(0xFF),
                                                                new Rgb888(0x00), new Rgb888(0x02), new Rgb888(0x05), new Rgb888(0xFF),
                                                                new Rgb888(0x00), new Rgb888(0x03), new Rgb888(0x06), new Rgb888(0xFF) });
        _packedPixelBufferMono = new PackedPixelBuffer<Mono>(2, 2, new Mono[] { 0f, 0.003921569f, 0.5019608f, 1f });
        _packedPixelBufferMono8 = new PackedPixelBuffer<Mono8>(2, 2, new Mono8[] { 0x00, 0x01, 0x80, 0xFF });
    }

    [Fact]
    public void TestCopy()
    {
        // Arrange
        using var image = new Image(_packedPixelBufferRgb);

        // Act
        var imageCopy = new Image(image);

        // Assert
        Assert.Equal(image, imageCopy);
    }

    [Fact]
    public void TestRgbToRgb24()
    {
        // Arrange
        using var image = new Image(_packedPixelBufferRgb);

        // Act
        ReadOnlyPackedPixelBuffer<Rgb24> resultBuffer = image.AsPacked<Rgb24>();

        // Assert
        Assert.IsType<ReadOnlyPackedPixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24.AsReadOnly(), resultBuffer);
    }

    [Fact]
    public void TestRgbToRgb888()
    {
        // Arrange
        using var image = new Image(_packedPixelBufferRgb);

        // Act
        ReadOnlyPlanarPixelBuffer<Rgb888> resultBuffer = image.AsPlanar<Rgb888>();

        // Assert
        Assert.IsType<ReadOnlyPlanarPixelBuffer<Rgb888>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888.AsReadOnly(), resultBuffer);
    }

    [Fact]
    public void TestMonoToMono8()
    {
        // Arrange
        using var image = new Image(_packedPixelBufferMono);

        // Act
        ReadOnlyPackedPixelBuffer<Mono8> resultBuffer = image.AsPacked<Mono8>();

        // Assert
        Assert.IsType<ReadOnlyPackedPixelBuffer<Mono8>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono8.AsReadOnly(), resultBuffer);
    }

    [Fact]
    public void TestLoadAndSaveMono8()
    {
        string targetName = $"{nameof(ImageTests)}_{nameof(TestLoadAndSaveMono8)}.png";
        using IImage loadedImage = Image.Load("./resources/lena8.bmp");
        loadedImage.Save(targetName, EncoderType.Png);

        Assert.True(File.Exists(targetName));
        File.Delete(targetName);
    }

    [Fact]
    public void TestLoadAndSaveRgb24()
    {
        string targetName = $"{nameof(ImageTests)}_{nameof(TestLoadAndSaveRgb24)}.png";
        using IImage loadedImage = Image.Load("./resources/lena24.png");
        loadedImage.Save(targetName, EncoderType.Png);

        Assert.True(File.Exists(targetName));
        File.Delete(targetName);
    }
}
