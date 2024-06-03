using ImageTorque;
using ImageTorque.Buffers;
using ImageTorque.Pixels;
using ImageTorque.Processing;

namespace AyBorg.SDK.ImageProcessing.Tests;

public class ImageTests
{
    private readonly PixelBuffer<Rgb> _packedPixelBufferRgb;
    private readonly PixelBuffer<Rgb24> _packedPixelBufferRgb24;
    private readonly PlanarPixelBuffer<L8> _planarPixelBufferRgb888;
    private readonly PixelBuffer<LS> _packedPixelBufferMono;
    private readonly PixelBuffer<L8> _packedPixelBufferMono8;

    public ImageTests()
    {
        _packedPixelBufferRgb = new PixelBuffer<Rgb>(2, 2, new[] {
                                                                new Rgb(0f, 0f, 0f),
                                                                new Rgb(0.0039215687f, 0.0078431373f, 0.0117647059f),
                                                                new Rgb(0.015686275f, 0.0196078432f, 0.0235294118f),
                                                                new Rgb(1f, 1f, 1f) });
        _packedPixelBufferRgb24 = new PixelBuffer<Rgb24>(2, 2, new[] {
                                                                new Rgb24(0x00, 0x00, 0x00),
                                                                new Rgb24(0x01, 0x02, 0x03),
                                                                new Rgb24(0x04, 0x05, 0x06),
                                                                new Rgb24(0xFF, 0xFF, 0xFF) });
        _planarPixelBufferRgb888 = new PlanarPixelBuffer<L8>(2, 2, new[] {
                                                                new L8(0x00), new L8(0x01), new L8(0x04), new L8(0xFF),
                                                                new L8(0x00), new L8(0x02), new L8(0x05), new L8(0xFF),
                                                                new L8(0x00), new L8(0x03), new L8(0x06), new L8(0xFF) });
        _packedPixelBufferMono = new PixelBuffer<LS>(2, 2, new LS[] { 0f, 0.003921569f, 0.5019608f, 1f });
        _packedPixelBufferMono8 = new PixelBuffer<L8>(2, 2, new L8[] { 0x00, 0x01, 0x80, 0xFF });
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
        ReadOnlyPlanarPixelBuffer<L8> resultBuffer = image.AsPlanar<L8>();

        // Assert
        Assert.IsType<ReadOnlyPlanarPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888.AsReadOnly(), resultBuffer);
    }

    [Fact]
    public void TestMonoToMono8()
    {
        // Arrange
        using var image = new Image(_packedPixelBufferMono);

        // Act
        ReadOnlyPackedPixelBuffer<L8> resultBuffer = image.AsPacked<L8>();

        // Assert
        Assert.IsType<ReadOnlyPackedPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono8.AsReadOnly(), resultBuffer);
    }

    [Fact]
    public void TestLoadAndSaveMono8()
    {
        // Arrange
        string targetName = $"{nameof(ImageTests)}_{nameof(TestLoadAndSaveMono8)}.png";
        using IImage loadedImage = Image.Load("./lena8.bmp", ConfigurationFactory.Build([
                new ImageTorque.Codecs.ImageSharp.PngCodec(),
                new ImageTorque.Codecs.ImageSharp.BmpCodec()
            ]));

        // Act
        loadedImage.Save(targetName, EncoderType.Png);

        // Assert
        Assert.True(File.Exists(targetName));
        File.Delete(targetName);
    }

    [Fact]
    public void TestLoadAndSaveRgb24()
    {
        string targetName = $"{nameof(ImageTests)}_{nameof(TestLoadAndSaveRgb24)}.png";
        using IImage loadedImage = Image.Load("./lena24.png", ConfigurationFactory.Build([new ImageTorque.Codecs.ImageSharp.PngCodec()]));
        loadedImage.Save(targetName, EncoderType.Png);

        Assert.True(File.Exists(targetName));
        File.Delete(targetName);
    }
}
