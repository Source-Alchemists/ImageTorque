using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class ImageMathTests
{
    private readonly ImageMath _imageMath = new();

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_LF(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<LF> pixelBufferA = new(2, 2, new LF[] { 0f, 0.4f, 0.5f, 0.6f });
        PixelBuffer<LF> pixelBufferB = new(2, 2, new LF[] { 0.5f, 0.3f, 0.6f, 0f });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters{
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<LF>>(result);
        switch(mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new LF[] { 0.5f, 0.70000005f, 1f, 0.6f }, ((PixelBuffer<LF>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new LF[] { 0f, 0.099999994f, 0f, 0.6f }, ((PixelBuffer<LF>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new LF[] { 0f, 0.120000005f, 0.3f, 0f }, ((PixelBuffer<LF>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_L8(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<L8> pixelBufferA = new(2, 2, new L8[] { 0, 102, 128, 153 });
        PixelBuffer<L8> pixelBufferB = new(2, 2, new L8[] { 128, 77, 153, 0 });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters{
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        switch(mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new L8[] { 128, 179, byte.MaxValue, 153 }, ((PixelBuffer<L8>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new L8[] { 0, 25, 0, 153 }, ((PixelBuffer<L8>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new L8[] { 0, byte.MaxValue, byte.MaxValue, 0 }, ((PixelBuffer<L8>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_L16(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<L16> pixelBufferA = new(2, 2, new L16[] { 0, 10200, 12800, 15300 });
        PixelBuffer<L16> pixelBufferB = new(2, 2, new L16[] { 12800, 7700, 15300, 0 });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters{
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        switch(mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new L16[] { 12800, 17900, 28100, 15300 }, ((PixelBuffer<L16>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new L16[] { 0, 2500, 0, 15300 }, ((PixelBuffer<L16>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new L16[] { 0, ushort.MaxValue, ushort.MaxValue, 0 }, ((PixelBuffer<L16>)result).Pixels[..4].ToArray());
                break;
        }
    }
}
