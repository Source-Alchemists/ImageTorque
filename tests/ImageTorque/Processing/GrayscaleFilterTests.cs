using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class GrayscaleFilterTests
{
    [Fact]
    public void Test_RgbToMono()
    {
        // Arrange
        var grayscaleFilter = new GrayscaleFilter();
        PixelBuffer<Rgb> pixelBuffer = new(2, 2, [new Rgb(0.3f, 0.2f, 0.8f), new Rgb(0.4f, 0.8f, 0.9f), new Rgb(0.1f, 0f, 1f), new Rgb(0.5f, 0.5f, 0.5f)]);

        // Act
        IPixelBuffer result = grayscaleFilter.Execute(new GrayscaleFilterParameters
        {
            Input = pixelBuffer.AsReadOnly()
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal([0.2983f, 0.6918f, 0.1439f, 0.5f], ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Rgb24ToMono8()
    {
        // Arrange
        var grayscaleFilter = new GrayscaleFilter();
        PixelBuffer<Rgb24> pixelBuffer = new(2, 2, [new Rgb24(76, 51, 204), new Rgb24(102, 204, 229), new Rgb24(25, 0, 255), new Rgb24(128, 128, 128)]);

        // Act
        IPixelBuffer result = grayscaleFilter.Execute(new GrayscaleFilterParameters
        {
            Input = pixelBuffer.AsReadOnly()
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        Assert.InRange(((PixelBuffer<L8>)result).Pixels[0].Value, 0, 255);
        Assert.Equal([75, 176, 36, 128], ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Rgb48ToMono16()
    {
        // Arrange
        var grayscaleFilter = new GrayscaleFilter();
        PixelBuffer<Rgb48> pixelBuffer = new(2, 2, [new Rgb48(19595, 13107, 52428), new Rgb48(26214, 52428, 58981), new Rgb48(6553, 0, 65535), new Rgb48(32768, 32768, 32768)]);

        // Act
        IPixelBuffer result = grayscaleFilter.Execute(new GrayscaleFilterParameters
        {
            Input = pixelBuffer.AsReadOnly()
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        Assert.InRange(((PixelBuffer<L16>)result).Pixels[0].Value, 0, 65535);
        Assert.Equal([19529, 45337, 9430, 32768], ((PixelBuffer<L16>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_RgbFFFToMono()
    {
        // Arrange
        var grayscaleFilter = new GrayscaleFilter();
        PlanarPixelBuffer<LS> pixelBuffer = new(2, 2, [
                                                        new LS(0.3f), new LS(0.4f), new LS(0.1f), new LS(0.5f),
                                                        new LS(0.2f), new LS(0.8f), new LS(0f), new LS(0.5f),
                                                        new LS(0.8f), new LS(0.9f), new LS(1f), new LS(0.5f)
                                                        ]);

        // Act
        IPixelBuffer result = grayscaleFilter.Execute(new GrayscaleFilterParameters
        {
            Input = pixelBuffer.AsReadOnly()
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal([0.2983f, 0.6918f, 0.1439f, 0.5f], ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Rgb888ToMono8()
    {
        // Arrange
        var grayscaleFilter = new GrayscaleFilter();
        PlanarPixelBuffer<L8> pixelBuffer = new(2, 2, [
                                                        new L8(76), new L8(102), new L8(25), new L8(128),
                                                        new L8(51), new L8(204), new L8(0), new L8(128),
                                                        new L8(204), new L8(229), new L8(255), new L8(128)
                                                        ]);

        // Act
        IPixelBuffer result = grayscaleFilter.Execute(new GrayscaleFilterParameters
        {
            Input = pixelBuffer.AsReadOnly()
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        Assert.InRange(((PixelBuffer<L8>)result).Pixels[0].Value, 0, 255);
        Assert.Equal([75, 176, 36, 128], ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Rgb161616ToMono16()
    {
        // Arrange
        var grayscaleFilter = new GrayscaleFilter();
        PlanarPixelBuffer<L16> pixelBuffer = new(2, 2, [
                                                        new L16(19595), new L16(26214), new L16(6553), new L16(32768),
                                                        new L16(13107), new L16(52428), new L16(0), new L16(32768),
                                                        new L16(52428), new L16(58981), new L16(65535), new L16(32768)
                                                        ]);

        // Act
        IPixelBuffer result = grayscaleFilter.Execute(new GrayscaleFilterParameters
        {
            Input = pixelBuffer.AsReadOnly()
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        Assert.InRange(((PixelBuffer<L16>)result).Pixels[0].Value, 0, 65535);
        Assert.Equal([19529, 45337, 9430, 32768], ((PixelBuffer<L16>)result).Pixels.ToArray());
    }
}
