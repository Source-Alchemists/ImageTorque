using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class ResizerTests
{
    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 0.11f, 0.15f, 0.51f, 0.55f)]
    [InlineData(ResizeMode.Bilinear, 0.11f, 0.14500001f, 0.45999998f, 0.495f)]
    [InlineData(ResizeMode.Bicubic, 0.158125f, 0.224375f, 0.820625f, 0.886875f)]
    public void Test_Resize_Mono(ResizeMode resizeMode, float ep1, float ep2, float ep3, float ep4)
    {
        // Arrange
        var expectedPixels = new LS[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PixelBuffer<LS> pixelBuffer = new(8, 8, [
                                                    0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f,
                                                    0.21f, 0.22f, 0.23f, 0.24f, 0.25f, 0.26f, 0.27f, 0.28f,
                                                    0.31f, 0.32f, 0.33f, 0.34f, 0.35f, 0.36f, 0.37f, 0.38f,
                                                    0.41f, 0.42f, 0.43f, 0.44f, 0.45f, 0.46f, 0.47f, 0.48f,
                                                    0.51f, 0.52f, 0.53f, 0.54f, 0.55f, 0.56f, 0.57f, 0.58f,
                                                    0.61f, 0.62f, 0.63f, 0.64f, 0.65f, 0.66f, 0.67f, 0.68f,
                                                    0.71f, 0.72f, 0.73f, 0.74f, 0.75f, 0.76f, 0.77f, 0.78f,
                                                    0.81f, 0.82f, 0.83f, 0.84f, 0.85f, 0.86f, 0.87f, 0.88f
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal(expectedPixels, ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 11, 15, 51, 55)]
    [InlineData(ResizeMode.Bilinear, 11, 14, 46, 49)]
    [InlineData(ResizeMode.Bicubic, 15, 22, 82, 88)]
    public void Test_Resize_Mono8(ResizeMode resizeMode, byte ep1, byte ep2, byte ep3, byte ep4)
    {
        // Arrange
        var expectedPixels = new L8[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PixelBuffer<L8> pixelBuffer = new(8, 8, [
                                                    11, 12, 13, 14, 15, 16, 17, 18,
                                                    21, 22, 23, 24, 25, 26, 27, 28,
                                                    31, 32, 33, 34, 35, 36, 37, 38,
                                                    41, 42, 43, 44, 45, 46, 47, 48,
                                                    51, 52, 53, 54, 55, 56, 57, 58,
                                                    61, 62, 63, 64, 65, 66, 67, 68,
                                                    71, 72, 73, 74, 75, 76, 77, 78,
                                                    81, 82, 83, 84, 85, 86, 87, 88
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        Assert.InRange(((PixelBuffer<L8>)result).Pixels[0].Value, 0, 255);
        Assert.Equal(expectedPixels, ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 1100, 1500, 5100, 5500)]
    [InlineData(ResizeMode.Bilinear, 1100, 1450, 4600, 4950)]
    [InlineData(ResizeMode.Bicubic, 1581, 2243, 8206, 8868)]
    public void Test_Resize_Mono16(ResizeMode resizeMode, ushort ep1, ushort ep2, ushort ep3, ushort ep4)
    {
        // Arrange
        var expectedPixels = new L16[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PixelBuffer<L16> pixelBuffer = new(8, 8, [
                                                    1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800,
                                                    2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800,
                                                    3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800,
                                                    4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800,
                                                    5100, 5200, 5300, 5400, 5500, 5600, 5700, 5800,
                                                    6100, 6200, 6300, 6400, 6500, 6600, 6700, 6800,
                                                    7100, 7200, 7300, 7400, 7500, 7600, 7700, 7800,
                                                    8100, 8200, 8300, 8400, 8500, 8600, 8700, 8800
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        Assert.InRange(((PixelBuffer<L16>)result).Pixels[0].Value, 0, ushort.MaxValue);
        Assert.Equal(expectedPixels, ((PixelBuffer<L16>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 0.2844f, 0.32439998f, 0.4431f, 0.48310003f)]
    [InlineData(ResizeMode.Bilinear, 0.2844f, 0.3194f, 0.41375f, 0.44875002f)]
    [InlineData(ResizeMode.Bicubic, 0.3174125f, 0.3836625f, 0.55114377f, 0.61739373f)]
    public void Test_Resize_RgbFFF(ResizeMode resizeMode, float ep1, float ep2, float ep3, float ep4)
    {
        // Arrange
        var expectedPixels = new LS[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PlanarPixelBuffer<LS> pixelBuffer = new(8, 8, [
                                                    0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f,
                                                    0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f,
                                                    0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f,
                                                    0.21f, 0.22f, 0.23f, 0.24f, 0.25f, 0.26f, 0.27f, 0.28f,
                                                    0.21f, 0.22f, 0.23f, 0.24f, 0.25f, 0.26f, 0.27f, 0.28f,
                                                    0.21f, 0.22f, 0.23f, 0.24f, 0.25f, 0.26f, 0.27f, 0.28f,
                                                    0.31f, 0.32f, 0.33f, 0.34f, 0.35f, 0.36f, 0.37f, 0.38f,
                                                    0.31f, 0.32f, 0.33f, 0.34f, 0.35f, 0.36f, 0.37f, 0.38f,
                                                    0.31f, 0.32f, 0.33f, 0.34f, 0.35f, 0.36f, 0.37f, 0.38f,
                                                    0.41f, 0.42f, 0.43f, 0.44f, 0.45f, 0.46f, 0.47f, 0.48f,
                                                    0.41f, 0.42f, 0.43f, 0.44f, 0.45f, 0.46f, 0.47f, 0.48f,
                                                    0.41f, 0.42f, 0.43f, 0.44f, 0.45f, 0.46f, 0.47f, 0.48f,
                                                    0.51f, 0.52f, 0.53f, 0.54f, 0.55f, 0.56f, 0.57f, 0.58f,
                                                    0.51f, 0.52f, 0.53f, 0.54f, 0.55f, 0.56f, 0.57f, 0.58f,
                                                    0.51f, 0.52f, 0.53f, 0.54f, 0.55f, 0.56f, 0.57f, 0.58f,
                                                    0.61f, 0.62f, 0.63f, 0.64f, 0.65f, 0.66f, 0.67f, 0.68f,
                                                    0.61f, 0.62f, 0.63f, 0.64f, 0.65f, 0.66f, 0.67f, 0.68f,
                                                    0.61f, 0.62f, 0.63f, 0.64f, 0.65f, 0.66f, 0.67f, 0.68f,
                                                    0.71f, 0.72f, 0.73f, 0.74f, 0.75f, 0.76f, 0.77f, 0.78f,
                                                    0.71f, 0.72f, 0.73f, 0.74f, 0.75f, 0.76f, 0.77f, 0.78f,
                                                    0.71f, 0.72f, 0.73f, 0.74f, 0.75f, 0.76f, 0.77f, 0.78f,
                                                    0.81f, 0.82f, 0.83f, 0.84f, 0.85f, 0.86f, 0.87f, 0.88f,
                                                    0.81f, 0.82f, 0.83f, 0.84f, 0.85f, 0.86f, 0.87f, 0.88f,
                                                    0.81f, 0.82f, 0.83f, 0.84f, 0.85f, 0.86f, 0.87f, 0.88f
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LS>>(result);
        Assert.InRange(((PlanarPixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        var filter = new GrayscaleFilter();
        IPixelBuffer grayResult = filter.Execute(new GrayscaleFilterParameters
        {
            Input = result.AsReadOnly()
        });
        Assert.Equal(expectedPixels, ((PixelBuffer<LS>)grayResult).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 28, 32, 44, 48)]
    [InlineData(ResizeMode.Bilinear, 28, 31, 41, 44)]
    [InlineData(ResizeMode.Bicubic, 31, 38, 54, 61)]
    public void Test_Resize_Rgb888(ResizeMode resizeMode, byte ep1, byte ep2, byte ep3, byte ep4)
    {
        // Arrange
        var expectedPixels = new L8[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PlanarPixelBuffer<L8> pixelBuffer = new(8, 8, [
                                                    11, 12, 13, 14, 15, 16, 17, 18,
                                                    11, 12, 13, 14, 15, 16, 17, 18,
                                                    11, 12, 13, 14, 15, 16, 17, 18,
                                                    21, 22, 23, 24, 25, 26, 27, 28,
                                                    21, 22, 23, 24, 25, 26, 27, 28,
                                                    21, 22, 23, 24, 25, 26, 27, 28,
                                                    31, 32, 33, 34, 35, 36, 37, 38,
                                                    31, 32, 33, 34, 35, 36, 37, 38,
                                                    31, 32, 33, 34, 35, 36, 37, 38,
                                                    41, 42, 43, 44, 45, 46, 47, 48,
                                                    41, 42, 43, 44, 45, 46, 47, 48,
                                                    41, 42, 43, 44, 45, 46, 47, 48,
                                                    51, 52, 53, 54, 55, 56, 57, 58,
                                                    51, 52, 53, 54, 55, 56, 57, 58,
                                                    51, 52, 53, 54, 55, 56, 57, 58,
                                                    61, 62, 63, 64, 65, 66, 67, 68,
                                                    61, 62, 63, 64, 65, 66, 67, 68,
                                                    61, 62, 63, 64, 65, 66, 67, 68,
                                                    71, 72, 73, 74, 75, 76, 77, 78,
                                                    71, 72, 73, 74, 75, 76, 77, 78,
                                                    71, 72, 73, 74, 75, 76, 77, 78,
                                                    81, 82, 83, 84, 85, 86, 87, 88,
                                                    81, 82, 83, 84, 85, 86, 87, 88,
                                                    81, 82, 83, 84, 85, 86, 87, 88
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(result);
        Assert.InRange(((PlanarPixelBuffer<L8>)result).Pixels[0].Value, 0, byte.MaxValue);
        var filter = new GrayscaleFilter();
        IPixelBuffer grayResult = filter.Execute(new GrayscaleFilterParameters
        {
            Input = result.AsReadOnly()
        });
        Assert.Equal(expectedPixels, ((PixelBuffer<L8>)grayResult).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 2844, 3244, 4431, 4831)]
    [InlineData(ResizeMode.Bilinear, 2844, 3194, 4137, 4487)]
    [InlineData(ResizeMode.Bicubic, 3173, 3836, 5510, 6173)]
    public void Test_Resize_Rgb161616(ResizeMode resizeMode, ushort ep1, ushort ep2, ushort ep3, ushort ep4)
    {
        // Arrange
        var expectedPixels = new L16[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PlanarPixelBuffer<L16> pixelBuffer = new(8, 8, [
                                                    1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800,
                                                    1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800,
                                                    1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800,
                                                    2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800,
                                                    2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800,
                                                    2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800,
                                                    3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800,
                                                    3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800,
                                                    3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800,
                                                    4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800,
                                                    4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800,
                                                    4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800,
                                                    5100, 5200, 5300, 5400, 5500, 5600, 5700, 5800,
                                                    5100, 5200, 5300, 5400, 5500, 5600, 5700, 5800,
                                                    5100, 5200, 5300, 5400, 5500, 5600, 5700, 5800,
                                                    6100, 6200, 6300, 6400, 6500, 6600, 6700, 6800,
                                                    6100, 6200, 6300, 6400, 6500, 6600, 6700, 6800,
                                                    6100, 6200, 6300, 6400, 6500, 6600, 6700, 6800,
                                                    7100, 7200, 7300, 7400, 7500, 7600, 7700, 7800,
                                                    7100, 7200, 7300, 7400, 7500, 7600, 7700, 7800,
                                                    7100, 7200, 7300, 7400, 7500, 7600, 7700, 7800,
                                                    8100, 8200, 8300, 8400, 8500, 8600, 8700, 8800,
                                                    8100, 8200, 8300, 8400, 8500, 8600, 8700, 8800,
                                                    8100, 8200, 8300, 8400, 8500, 8600, 8700, 8800
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L16>>(result);
        Assert.InRange(((PlanarPixelBuffer<L16>)result).Pixels[0].Value, 0, ushort.MaxValue);
        var filter = new GrayscaleFilter();
        IPixelBuffer grayResult = filter.Execute(new GrayscaleFilterParameters
        {
            Input = result.AsReadOnly()
        });
        Assert.Equal(expectedPixels, ((PixelBuffer<L16>)grayResult).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 0.11f, 0.15f, 0.51f, 0.5500001f)]
    [InlineData(ResizeMode.Bilinear, 0.11f, 0.14500001f, 0.45999998f, 0.49500003f)]
    [InlineData(ResizeMode.Bicubic, 0.158125f, 0.224375f, 0.82062507f, 0.886875f)]
    public void Test_Resize_Rgb(ResizeMode resizeMode, float ep1, float ep2, float ep3, float ep4)
    {
        // Arrange
        var expectedPixels = new LS[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PixelBuffer<Rgb> pixelBuffer = new(8, 8, [
                                                    new Rgb(0.11f, 0.11f, 0.11f), new Rgb(0.12f, 0.12f, 0.12f), new Rgb(0.13f, 0.13f, 0.13f), new Rgb(0.14f, 0.14f, 0.14f), new Rgb(0.15f, 0.15f, 0.15f), new Rgb(0.16f, 0.16f, 0.16f), new Rgb(0.17f, 0.17f, 0.17f), new Rgb(0.18f, 0.18f, 0.18f),
                                                    new Rgb(0.21f, 0.21f, 0.21f), new Rgb(0.22f, 0.22f, 0.22f), new Rgb(0.23f, 0.23f, 0.23f), new Rgb(0.24f, 0.24f, 0.24f), new Rgb(0.25f, 0.25f, 0.25f), new Rgb(0.26f, 0.26f, 0.26f), new Rgb(0.27f, 0.27f, 0.27f), new Rgb(0.28f, 0.28f, 0.28f),
                                                    new Rgb(0.31f, 0.31f, 0.31f), new Rgb(0.32f, 0.32f, 0.32f), new Rgb(0.33f, 0.33f, 0.33f), new Rgb(0.34f, 0.34f, 0.34f), new Rgb(0.35f, 0.35f, 0.35f), new Rgb(0.36f, 0.36f, 0.36f), new Rgb(0.37f, 0.37f, 0.37f), new Rgb(0.38f, 0.38f, 0.38f),
                                                    new Rgb(0.41f, 0.41f, 0.41f), new Rgb(0.42f, 0.42f, 0.42f), new Rgb(0.43f, 0.43f, 0.43f), new Rgb(0.44f, 0.44f, 0.44f), new Rgb(0.45f, 0.45f, 0.45f), new Rgb(0.46f, 0.46f, 0.46f), new Rgb(0.47f, 0.47f, 0.47f), new Rgb(0.48f, 0.48f, 0.48f),
                                                    new Rgb(0.51f, 0.51f, 0.51f), new Rgb(0.52f, 0.52f, 0.52f), new Rgb(0.53f, 0.53f, 0.53f), new Rgb(0.54f, 0.54f, 0.54f), new Rgb(0.55f, 0.55f, 0.55f), new Rgb(0.56f, 0.56f, 0.56f), new Rgb(0.57f, 0.57f, 0.57f), new Rgb(0.58f, 0.58f, 0.58f),
                                                    new Rgb(0.61f, 0.61f, 0.61f), new Rgb(0.62f, 0.62f, 0.62f), new Rgb(0.63f, 0.63f, 0.63f), new Rgb(0.64f, 0.64f, 0.64f), new Rgb(0.65f, 0.65f, 0.65f), new Rgb(0.66f, 0.66f, 0.66f), new Rgb(0.67f, 0.67f, 0.67f), new Rgb(0.68f, 0.68f, 0.68f),
                                                    new Rgb(0.71f, 0.71f, 0.71f), new Rgb(0.72f, 0.72f, 0.72f), new Rgb(0.73f, 0.73f, 0.73f), new Rgb(0.74f, 0.74f, 0.74f), new Rgb(0.75f, 0.75f, 0.75f), new Rgb(0.76f, 0.76f, 0.76f), new Rgb(0.77f, 0.77f, 0.77f), new Rgb(0.78f, 0.78f, 0.78f),
                                                    new Rgb(0.81f, 0.81f, 0.81f), new Rgb(0.82f, 0.82f, 0.82f), new Rgb(0.83f, 0.83f, 0.83f), new Rgb(0.84f, 0.84f, 0.84f), new Rgb(0.85f, 0.85f, 0.85f), new Rgb(0.86f, 0.86f, 0.86f), new Rgb(0.87f, 0.87f, 0.87f), new Rgb(0.88f, 0.88f, 0.88f)
                                                    ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(result);
        Assert.InRange(((PixelBuffer<Rgb>)result).Pixels[0].R, 0f, 1f);
        Assert.InRange(((PixelBuffer<Rgb>)result).Pixels[0].G, 0f, 1f);
        Assert.InRange(((PixelBuffer<Rgb>)result).Pixels[0].B, 0f, 1f);
        var filter = new GrayscaleFilter();
        IPixelBuffer grayResult = filter.Execute(new GrayscaleFilterParameters
        {
            Input = result.AsReadOnly()
        });
        Assert.Equal(expectedPixels, ((PixelBuffer<LS>)grayResult).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 11, 15, 51, 55)]
    [InlineData(ResizeMode.Bilinear, 11, 14, 46, 49)]
    [InlineData(ResizeMode.Bicubic, 15, 18, 46, 49)]
    public void Test_Resize_Rgb24(ResizeMode resizeMode, byte ep1, byte ep2, byte ep3, byte ep4)
    {
        // Arrange
        var expectedPixels = new L8[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PixelBuffer<Rgb24> pixelBuffer = new(8, 8, [
                                                    new Rgb24(11, 11, 11), new Rgb24(12, 12, 12), new Rgb24(13, 13, 13), new Rgb24(14, 14, 14), new Rgb24(15, 15, 15), new Rgb24(16, 16, 16), new Rgb24(17, 17, 17), new Rgb24(18, 18, 18),
                                                    new Rgb24(21, 21, 21), new Rgb24(22, 22, 22), new Rgb24(23, 23, 23), new Rgb24(24, 24, 24), new Rgb24(25, 25, 25), new Rgb24(26, 26, 26), new Rgb24(27, 27, 27), new Rgb24(28, 28, 28),
                                                    new Rgb24(31, 31, 31), new Rgb24(32, 32, 32), new Rgb24(33, 33, 33), new Rgb24(34, 34, 34), new Rgb24(35, 35, 35), new Rgb24(36, 36, 36), new Rgb24(37, 37, 37), new Rgb24(38, 38, 38),
                                                    new Rgb24(41, 41, 41), new Rgb24(42, 42, 42), new Rgb24(43, 43, 43), new Rgb24(44, 44, 44), new Rgb24(45, 45, 45), new Rgb24(46, 46, 46), new Rgb24(47, 47, 47), new Rgb24(48, 48, 48),
                                                    new Rgb24(51, 51, 51), new Rgb24(52, 52, 52), new Rgb24(53, 53, 53), new Rgb24(54, 54, 54), new Rgb24(55, 55, 55), new Rgb24(56, 56, 56), new Rgb24(57, 57, 57), new Rgb24(58, 58, 58),
                                                    new Rgb24(61, 61, 61), new Rgb24(62, 62, 62), new Rgb24(63, 63, 63), new Rgb24(64, 64, 64), new Rgb24(65, 65, 65), new Rgb24(66, 66, 66), new Rgb24(67, 67, 67), new Rgb24(68, 68, 68),
                                                    new Rgb24(71, 71, 71), new Rgb24(72, 72, 72), new Rgb24(73, 73, 73), new Rgb24(74, 74, 74), new Rgb24(75, 75, 75), new Rgb24(76, 76, 76), new Rgb24(77, 77, 77), new Rgb24(78, 78, 78),
                                                    new Rgb24(81, 81, 81), new Rgb24(82, 82, 82), new Rgb24(83, 83, 83), new Rgb24(84, 84, 84), new Rgb24(85, 85, 85), new Rgb24(86, 86, 86), new Rgb24(87, 87, 87), new Rgb24(88, 88, 88)
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(result);
        Assert.InRange(((PixelBuffer<Rgb24>)result).Pixels[0].R, 0, byte.MaxValue);
        Assert.InRange(((PixelBuffer<Rgb24>)result).Pixels[0].G, 0, byte.MaxValue);
        Assert.InRange(((PixelBuffer<Rgb24>)result).Pixels[0].B, 0, byte.MaxValue);
        var filter = new GrayscaleFilter();
        IPixelBuffer grayResult = filter.Execute(new GrayscaleFilterParameters
        {
            Input = result.AsReadOnly()
        });
        Assert.Equal(expectedPixels, ((PixelBuffer<L8>)grayResult).Pixels.ToArray());
    }

    [Theory]
    [InlineData(ResizeMode.NearestNeighbor, 1100, 1500, 5100, 5500)]
    [InlineData(ResizeMode.Bilinear, 1100, 1450, 4600, 4950)]
    [InlineData(ResizeMode.Bicubic, 1581, 2243, 8206, 8868)]
    public void Test_Resize_Rgb48(ResizeMode resizeMode, ushort ep1, ushort ep2, ushort ep3, ushort ep4)
    {
        // Arrange
        var expectedPixels = new L16[] { ep1, ep2, ep3, ep4 };
        var resizer = new Resizer();
        PixelBuffer<Rgb48> pixelBuffer = new(8, 8, [
                                                    new Rgb48(1100, 1100, 1100), new Rgb48(1200, 1200, 1200), new Rgb48(1300, 1300, 1300), new Rgb48(1400, 1400, 1400), new Rgb48(1500, 1500, 1500), new Rgb48(1600, 1600, 1600), new Rgb48(1700, 1700, 1700), new Rgb48(1800, 1800, 1800),
                                                    new Rgb48(2100, 2100, 2100), new Rgb48(2200, 2200, 2200), new Rgb48(2300, 2300, 2300), new Rgb48(2400, 2400, 2400), new Rgb48(2500, 2500, 2500), new Rgb48(2600, 2600, 2600), new Rgb48(2700, 2700, 2700), new Rgb48(2800, 2800, 2800),
                                                    new Rgb48(3100, 3100, 3100), new Rgb48(3200, 3200, 3200), new Rgb48(3300, 3300, 3300), new Rgb48(3400, 3400, 3400), new Rgb48(3500, 3500, 3500), new Rgb48(3600, 3600, 3600), new Rgb48(3700, 3700, 3700), new Rgb48(3800, 3800, 3800),
                                                    new Rgb48(4100, 4100, 4100), new Rgb48(4200, 4200, 4200), new Rgb48(4300, 4300, 4300), new Rgb48(4400, 4400, 4400), new Rgb48(4500, 4500, 4500), new Rgb48(4600, 4600, 4600), new Rgb48(4700, 4700, 4700), new Rgb48(4800, 4800, 4800),
                                                    new Rgb48(5100, 5100, 5100), new Rgb48(5200, 5200, 5200), new Rgb48(5300, 5300, 5300), new Rgb48(5400, 5400, 5400), new Rgb48(5500, 5500, 5500), new Rgb48(5600, 5600, 5600), new Rgb48(5700, 5700, 5700), new Rgb48(5800, 5800, 5800),
                                                    new Rgb48(6100, 6100, 6100), new Rgb48(6200, 6200, 6200), new Rgb48(6300, 6300, 6300), new Rgb48(6400, 6400, 6400), new Rgb48(6500, 6500, 6500), new Rgb48(6600, 6600, 6600), new Rgb48(6700, 6700, 6700), new Rgb48(6800, 6800, 6800),
                                                    new Rgb48(7100, 7100, 7100), new Rgb48(7200, 7200, 7200), new Rgb48(7300, 7300, 7300), new Rgb48(7400, 7400, 7400), new Rgb48(7500, 7500, 7500), new Rgb48(7600, 7600, 7600), new Rgb48(7700, 7700, 7700), new Rgb48(7800, 7800, 7800),
                                                    new Rgb48(8100, 8100, 8100), new Rgb48(8200, 8200, 8200), new Rgb48(8300, 8300, 8300), new Rgb48(8400, 8400, 8400), new Rgb48(8500, 8500, 8500), new Rgb48(8600, 8600, 8600), new Rgb48(8700, 8700, 8700), new Rgb48(8800, 8800, 8800)
                                                ]);

        // Act
        IPixelBuffer result = resizer.Execute(new ResizerParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Width = 2,
            Height = 2,
            ResizeMode = resizeMode
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(result);
        Assert.InRange(((PixelBuffer<Rgb48>)result).Pixels[0].R, 0, ushort.MaxValue);
        Assert.InRange(((PixelBuffer<Rgb48>)result).Pixels[0].G, 0, ushort.MaxValue);
        Assert.InRange(((PixelBuffer<Rgb48>)result).Pixels[0].B, 0, ushort.MaxValue);
        var filter = new GrayscaleFilter();
        IPixelBuffer grayResult = filter.Execute(new GrayscaleFilterParameters
        {
            Input = result.AsReadOnly()
        });
        Assert.Equal(expectedPixels, ((PixelBuffer<L16>)grayResult).Pixels.ToArray());
    }
}
