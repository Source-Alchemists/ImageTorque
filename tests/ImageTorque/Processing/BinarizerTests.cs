using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class BinarizerTests
{
    [Theory]
    [InlineData(BinaryThresholdMode.Lumincance, 0f, 0f, 1f, 1f)]
    [InlineData(BinaryThresholdMode.MaxChroma, 0f, 0f, 1f, 1f)]
    [InlineData(BinaryThresholdMode.Saturation, 0f, 0f, 1f, 1f)]
    public void Test_Binarize_Mono(BinaryThresholdMode binaryThresholdMode, float ep1, float ep2, float ep3, float ep4)
    {
        // Arrange
        var expectedPixels = new LS[] { ep1, ep2, ep3, ep4 };
        var binarizer = new Binarizer();
        PixelBuffer<LS> pixelBuffer = new(2, 2, [0f, 0.4f, 0.5f, 0.6f]);

        // Act
        IPixelBuffer result = binarizer.Execute(new BinarizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Threshold = 0.5f,
            Mode = binaryThresholdMode
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal(expectedPixels, ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(BinaryThresholdMode.Lumincance, 0, 0, 255, 255)]
    [InlineData(BinaryThresholdMode.MaxChroma, 0, 0, 255, 255)]
    [InlineData(BinaryThresholdMode.Saturation, 0, 0, 255, 255)]
    public void Test_Binarize_Mono8(BinaryThresholdMode binaryThresholdMode, byte ep1, byte ep2, byte ep3, byte ep4)
    {
        // Arrange
        var expectedPixels = new L8[] { ep1, ep2, ep3, ep4 };
        var binarizer = new Binarizer();
        PixelBuffer<L8> pixelBuffer = new(2, 2, [0, 100, 129, 150]);

        // Act
        IPixelBuffer result = binarizer.Execute(new BinarizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Threshold = 0.5f,
            Mode = binaryThresholdMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        Assert.InRange(((PixelBuffer<L8>)result).Pixels[0].Value, 0, 255);
        Assert.Equal(expectedPixels, ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(BinaryThresholdMode.Lumincance, 0, 0, 65535, 65535)]
    [InlineData(BinaryThresholdMode.MaxChroma, 0, 0, 65535, 65535)]
    [InlineData(BinaryThresholdMode.Saturation, 0, 0, 65535, 65535)]
    public void Test_Binarize_Mono16(BinaryThresholdMode binaryThresholdMode, ushort ep1, ushort ep2, ushort ep3, ushort ep4)
    {
        // Arrange
        var expectedPixels = new L16[] { ep1, ep2, ep3, ep4 };
        var binarizer = new Binarizer();
        PixelBuffer<L16> pixelBuffer = new(2, 2, [0, 10000, 32768, 40000]);

        // Act
        IPixelBuffer result = binarizer.Execute(new BinarizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Threshold = 0.5f,
            Mode = binaryThresholdMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        Assert.InRange(((PixelBuffer<L16>)result).Pixels[0].Value, 0, 65535);
        Assert.Equal(expectedPixels, ((PixelBuffer<L16>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(BinaryThresholdMode.Lumincance, 0f, 0f, 1f, 1f)]
    [InlineData(BinaryThresholdMode.MaxChroma, 0f, 0f, 0f, 0f)]
    [InlineData(BinaryThresholdMode.Saturation, 0f, 0f, 0f, 1f)]
    public void Test_Binarize_Rgb(BinaryThresholdMode binaryThresholdMode, float ep1, float ep2, float ep3, float ep4)
    {
        // Arrange
        var expectedPixels = new LS[] { ep1, ep2, ep3, ep4 };
        var binarizer = new Binarizer();
        PixelBuffer<Rgb> pixelBuffer = new(2, 2, [new Rgb(0f, 0f, 0f), new Rgb(0.4f, 0.5f, 0.3f), new Rgb(0.7f, 0.7f, 0.8f), new Rgb(1f, 0.9f, 0.7f)]);

        // Act
        IPixelBuffer result = binarizer.Execute(new BinarizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Threshold = 0.5f,
            Mode = binaryThresholdMode
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal(expectedPixels, ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(BinaryThresholdMode.Lumincance, 0, 0, 255, 255)]
    [InlineData(BinaryThresholdMode.MaxChroma, 0, 0, 0, 0)]
    [InlineData(BinaryThresholdMode.Saturation, 0, 0, 255, 255)]
    public void Test_Binarize_Rgb24(BinaryThresholdMode binaryThresholdMode, byte ep1, byte ep2, byte ep3, byte ep4)
    {
        // Arrange
        var expectedPixels = new L8[] { ep1, ep2, ep3, ep4 };
        var binarizer = new Binarizer();
        PixelBuffer<Rgb24> pixelBuffer = new(2, 2, [new Rgb24(0, 0, 0), new Rgb24(100, 129, 100), new Rgb24(200, 200, 255), new Rgb24(255, 200, 100)]);

        // Act
        IPixelBuffer result = binarizer.Execute(new BinarizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Threshold = 0.5f,
            Mode = binaryThresholdMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        Assert.InRange(((PixelBuffer<L8>)result).Pixels[0].Value, 0, 255);
        Assert.Equal(expectedPixels, ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(BinaryThresholdMode.Lumincance, 0, 0, 65535, 65535)]
    [InlineData(BinaryThresholdMode.MaxChroma, 0, 0, 65535, 65535)]
    [InlineData(BinaryThresholdMode.Saturation, 0, 65535, 65535, 65535)]
    public void Test_Binarize_Rgb48(BinaryThresholdMode binaryThresholdMode, ushort ep1, ushort ep2, ushort ep3, ushort ep4)
    {
        // Arrange
        var expectedPixels = new L16[] { ep1, ep2, ep3, ep4 };
        var binarizer = new Binarizer();
        PixelBuffer<Rgb48> pixelBuffer = new(2, 2, [new Rgb48(0, 0, 0), new Rgb48(10000, 32768, 10000), new Rgb48(65535, 20000, 65535), new Rgb48(65535, 20000, 65535)]);

        // Act
        IPixelBuffer result = binarizer.Execute(new BinarizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Threshold = 0.5f,
            Mode = binaryThresholdMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        Assert.InRange(((PixelBuffer<L16>)result).Pixels[0].Value, 0, 65535);
        Assert.Equal(expectedPixels, ((PixelBuffer<L16>)result).Pixels.ToArray());
    }
}
