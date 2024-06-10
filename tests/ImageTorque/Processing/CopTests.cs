using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class CropTests
{
    [Fact]
    public void Test_Crop_Mono()
    {
        // Arrange
        var crop = new Crop();
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
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal([0.33f, 0.34f, 0.35f, 0.36f,
                      0.43f, 0.44f, 0.45f, 0.46f,
                      0.53f, 0.54f, 0.55f, 0.56f,
                      0.63f, 0.64f, 0.65f, 0.66f], ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Mono8()
    {
        // Arrange
        var crop = new Crop();
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
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        Assert.InRange(((PixelBuffer<L8>)result).Pixels[0].Value, 0, byte.MaxValue);
        Assert.Equal([33, 34, 35, 36,
                      43, 44, 45, 46,
                      53, 54, 55, 56,
                      63, 64, 65, 66], ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Mono16()
    {
        // Arrange
        var crop = new Crop();
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
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        Assert.InRange(((PixelBuffer<L16>)result).Pixels[0].Value, 0, ushort.MaxValue);
        Assert.Equal([3300, 3400, 3500, 3600,
                      4300, 4400, 4500, 4600,
                      5300, 5400, 5500, 5600,
                      6300, 6400, 6500, 6600], ((PixelBuffer<L16>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb()
    {
        // Arrange
        var crop = new Crop();
        PixelBuffer<Rgb> pixelBuffer = new(8, 8, [
                                                new Rgb(0.11f, 0.12f, 0.13f), new Rgb(0.14f, 0.15f, 0.16f), new Rgb(0.17f, 0.18f, 0.19f), new Rgb(0.20f, 0.21f, 0.22f), new Rgb(0.23f, 0.24f, 0.25f), new Rgb(0.26f, 0.27f, 0.28f), new Rgb(0.29f, 0.30f, 0.31f), new Rgb(0.32f, 0.33f, 0.34f),
                                                new Rgb(0.23f, 0.24f, 0.25f), new Rgb(0.26f, 0.27f, 0.28f), new Rgb(0.29f, 0.30f, 0.31f), new Rgb(0.32f, 0.33f, 0.34f), new Rgb(0.35f, 0.36f, 0.37f), new Rgb(0.38f, 0.39f, 0.40f), new Rgb(0.41f, 0.42f, 0.43f), new Rgb(0.44f, 0.45f, 0.46f),
                                                new Rgb(0.35f, 0.36f, 0.37f), new Rgb(0.38f, 0.39f, 0.40f), new Rgb(0.41f, 0.42f, 0.43f), new Rgb(0.44f, 0.45f, 0.46f), new Rgb(0.47f, 0.48f, 0.49f), new Rgb(0.50f, 0.51f, 0.52f), new Rgb(0.53f, 0.54f, 0.55f), new Rgb(0.56f, 0.57f, 0.58f),
                                                new Rgb(0.47f, 0.48f, 0.49f), new Rgb(0.50f, 0.51f, 0.52f), new Rgb(0.53f, 0.54f, 0.55f), new Rgb(0.56f, 0.57f, 0.58f), new Rgb(0.59f, 0.60f, 0.61f), new Rgb(0.62f, 0.63f, 0.64f), new Rgb(0.65f, 0.66f, 0.67f), new Rgb(0.68f, 0.69f, 0.70f),
                                                new Rgb(0.59f, 0.60f, 0.61f), new Rgb(0.62f, 0.63f, 0.64f), new Rgb(0.65f, 0.66f, 0.67f), new Rgb(0.68f, 0.69f, 0.70f), new Rgb(0.71f, 0.72f, 0.73f), new Rgb(0.74f, 0.75f, 0.76f), new Rgb(0.77f, 0.78f, 0.79f), new Rgb(0.80f, 0.81f, 0.82f),
                                                new Rgb(0.71f, 0.72f, 0.73f), new Rgb(0.74f, 0.75f, 0.76f), new Rgb(0.77f, 0.78f, 0.79f), new Rgb(0.80f, 0.81f, 0.82f), new Rgb(0.83f, 0.84f, 0.85f), new Rgb(0.86f, 0.87f, 0.88f), new Rgb(0.89f, 0.90f, 0.91f), new Rgb(0.92f, 0.93f, 0.94f),
                                                new Rgb(0.83f, 0.84f, 0.85f), new Rgb(0.86f, 0.87f, 0.88f), new Rgb(0.89f, 0.90f, 0.91f), new Rgb(0.92f, 0.93f, 0.94f), new Rgb(0.95f, 0.96f, 0.97f), new Rgb(0.98f, 0.99f, 1.00f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f),
                                                new Rgb(0.95f, 0.96f, 0.97f), new Rgb(0.98f, 0.99f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f)
                                            ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(result);
        Assert.InRange(((PixelBuffer<Rgb>)result).Pixels[0].Red, 0f, 1f);
        Assert.Equal([
            new Rgb(0.41f, 0.42f, 0.43f), new Rgb(0.44f, 0.45f, 0.46f), new Rgb(0.47f, 0.48f, 0.49f), new Rgb(0.50f, 0.51f, 0.52f),
            new Rgb(0.53f, 0.54f, 0.55f), new Rgb(0.56f, 0.57f, 0.58f), new Rgb(0.59f, 0.60f, 0.61f), new Rgb(0.62f, 0.63f, 0.64f),
            new Rgb(0.65f, 0.66f, 0.67f), new Rgb(0.68f, 0.69f, 0.70f), new Rgb(0.71f, 0.72f, 0.73f), new Rgb(0.74f, 0.75f, 0.76f),
            new Rgb(0.77f, 0.78f, 0.79f), new Rgb(0.80f, 0.81f, 0.82f), new Rgb(0.83f, 0.84f, 0.85f), new Rgb(0.86f, 0.87f, 0.88f)
        ], ((PixelBuffer<Rgb>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb24()
    {
        // Arrange
        var crop = new Crop();
        PixelBuffer<Rgb24> pixelBuffer = new(8, 8, [
                                                    new Rgb24(11, 12, 13), new Rgb24(14, 15, 16), new Rgb24(17, 18, 19), new Rgb24(20, 21, 22), new Rgb24(23, 24, 25), new Rgb24(26, 27, 28), new Rgb24(29, 30, 31), new Rgb24(32, 33, 34),
                                                    new Rgb24(23, 24, 25), new Rgb24(26, 27, 28), new Rgb24(29, 30, 31), new Rgb24(32, 33, 34), new Rgb24(35, 36, 37), new Rgb24(38, 39, 40), new Rgb24(41, 42, 43), new Rgb24(44, 45, 46),
                                                    new Rgb24(35, 36, 37), new Rgb24(38, 39, 40), new Rgb24(41, 42, 43), new Rgb24(44, 45, 46), new Rgb24(47, 48, 49), new Rgb24(50, 51, 52), new Rgb24(53, 54, 55), new Rgb24(56, 57, 58),
                                                    new Rgb24(47, 48, 49), new Rgb24(50, 51, 52), new Rgb24(53, 54, 55), new Rgb24(56, 57, 58), new Rgb24(59, 60, 61), new Rgb24(62, 63, 64), new Rgb24(65, 66, 67), new Rgb24(68, 69, 70),
                                                    new Rgb24(59, 60, 61), new Rgb24(62, 63, 64), new Rgb24(65, 66, 67), new Rgb24(68, 69, 70), new Rgb24(71, 72, 73), new Rgb24(74, 75, 76), new Rgb24(77, 78, 79), new Rgb24(80, 81, 82),
                                                    new Rgb24(71, 72, 73), new Rgb24(74, 75, 76), new Rgb24(77, 78, 79), new Rgb24(80, 81, 82), new Rgb24(83, 84, 85), new Rgb24(86, 87, 88), new Rgb24(89, 90, 91), new Rgb24(92, 93, 94),
                                                    new Rgb24(83, 84, 85), new Rgb24(86, 87, 88), new Rgb24(89, 90, 91), new Rgb24(92, 93, 94), new Rgb24(95, 96, 97), new Rgb24(98, 99, 100), new Rgb24(101, 102, 103), new Rgb24(104, 105, 106),
                                                    new Rgb24(95, 96, 97), new Rgb24(98, 99, 100), new Rgb24(101, 102, 103), new Rgb24(104, 105, 106), new Rgb24(107, 108, 109), new Rgb24(110, 111, 112), new Rgb24(113, 114, 115), new Rgb24(116, 117, 118)
                                                ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(result);
        Assert.InRange(((PixelBuffer<Rgb24>)result).Pixels[0].Red, byte.MinValue, byte.MaxValue);
        Assert.Equal([
            new Rgb24(41, 42, 43), new Rgb24(44, 45, 46), new Rgb24(47, 48, 49), new Rgb24(50, 51, 52),
            new Rgb24(53, 54, 55), new Rgb24(56, 57, 58), new Rgb24(59, 60, 61), new Rgb24(62, 63, 64),
            new Rgb24(65, 66, 67), new Rgb24(68, 69, 70), new Rgb24(71, 72, 73), new Rgb24(74, 75, 76),
            new Rgb24(77, 78, 79), new Rgb24(80, 81, 82), new Rgb24(83, 84, 85), new Rgb24(86, 87, 88)
        ], ((PixelBuffer<Rgb24>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb48()
    {
        // Arrange
        var crop = new Crop();
        PixelBuffer<Rgb48> pixelBuffer = new(8, 8, [
                                                    new Rgb48(1100, 1200, 1300), new Rgb48(1400, 1500, 1600), new Rgb48(1700, 1800, 1900), new Rgb48(2000, 2100, 2200), new Rgb48(2300, 2400, 2500), new Rgb48(2600, 2700, 2800), new Rgb48(2900, 3000, 3100), new Rgb48(3200, 3300, 3400),
                                                    new Rgb48(2300, 2400, 2500), new Rgb48(2600, 2700, 2800), new Rgb48(2900, 3000, 3100), new Rgb48(3200, 3300, 3400), new Rgb48(3500, 3600, 3700), new Rgb48(3800, 3900, 4000), new Rgb48(4100, 4200, 4300), new Rgb48(4400, 4500, 4600),
                                                    new Rgb48(3500, 3600, 3700), new Rgb48(3800, 3900, 4000), new Rgb48(4100, 4200, 4300), new Rgb48(4400, 4500, 4600), new Rgb48(4700, 4800, 4900), new Rgb48(5000, 5100, 5200), new Rgb48(5300, 5400, 5500), new Rgb48(5600, 5700, 5800),
                                                    new Rgb48(4700, 4800, 4900), new Rgb48(5000, 5100, 5200), new Rgb48(5300, 5400, 5500), new Rgb48(5600, 5700, 5800), new Rgb48(5900, 6000, 6100), new Rgb48(6200, 6300, 6400), new Rgb48(6500, 6600, 6700), new Rgb48(6800, 6900, 7000),
                                                    new Rgb48(5900, 6000, 6100), new Rgb48(6200, 6300, 6400), new Rgb48(6500, 6600, 6700), new Rgb48(6800, 6900, 7000), new Rgb48(7100, 7200, 7300), new Rgb48(7400, 7500, 7600), new Rgb48(7700, 7800, 7900), new Rgb48(8000, 8100, 8200),
                                                    new Rgb48(7100, 7200, 7300), new Rgb48(7400, 7500, 7600), new Rgb48(7700, 7800, 7900), new Rgb48(8000, 8100, 8200), new Rgb48(8300, 8400, 8500), new Rgb48(8600, 8700, 8800), new Rgb48(8900, 9000, 9100), new Rgb48(9200, 9300, 9400),
                                                    new Rgb48(8300, 8400, 8500), new Rgb48(8600, 8700, 8800), new Rgb48(8900, 9000, 9100), new Rgb48(9200, 9300, 9400), new Rgb48(9500, 9600, 9700), new Rgb48(9800, 9900, 10000), new Rgb48(10100, 10200, 10300), new Rgb48(10400, 10500, 10600),
                                                    new Rgb48(9500, 9600, 9700), new Rgb48(9800, 9900, 10000), new Rgb48(10100, 10200, 10300), new Rgb48(10400, 10500, 10600), new Rgb48(10700, 10800, 10900), new Rgb48(11000, 11100, 11200), new Rgb48(11300, 11400, 11500), new Rgb48(11600, 11700, 11800)
                                                ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(result);
        Assert.InRange(((PixelBuffer<Rgb48>)result).Pixels[0].Red, ushort.MinValue, ushort.MaxValue);
        Assert.Equal([
            new Rgb48(4100, 4200, 4300), new Rgb48(4400, 4500, 4600), new Rgb48(4700, 4800, 4900), new Rgb48(5000, 5100, 5200),
            new Rgb48(5300, 5400, 5500), new Rgb48(5600, 5700, 5800), new Rgb48(5900, 6000, 6100), new Rgb48(6200, 6300, 6400),
            new Rgb48(6500, 6600, 6700), new Rgb48(6800, 6900, 7000), new Rgb48(7100, 7200, 7300), new Rgb48(7400, 7500, 7600),
            new Rgb48(7700, 7800, 7900), new Rgb48(8000, 8100, 8200), new Rgb48(8300, 8400, 8500), new Rgb48(8600, 8700, 8800)
        ], ((PixelBuffer<Rgb48>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_RgbFFF()
    {
        // Arrange
        var crop = new Crop();
        PlanarPixelBuffer<LS> pixelBuffer = new(8, 8, [
                                                        // R
                                                        new LS(0.11f), new LS(0.12f), new LS(0.13f), new LS(0.14f), new LS(0.15f), new LS(0.16f), new LS(0.17f), new LS(0.18f),
                                                        new LS(0.21f), new LS(0.22f), new LS(0.23f), new LS(0.24f), new LS(0.25f), new LS(0.26f), new LS(0.27f), new LS(0.28f),
                                                        new LS(0.31f), new LS(0.32f), new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f), new LS(0.37f), new LS(0.38f),
                                                        new LS(0.41f), new LS(0.42f), new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f), new LS(0.47f), new LS(0.48f),
                                                        new LS(0.51f), new LS(0.52f), new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f), new LS(0.57f), new LS(0.58f),
                                                        new LS(0.61f), new LS(0.62f), new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f), new LS(0.67f), new LS(0.68f),
                                                        new LS(0.71f), new LS(0.72f), new LS(0.73f), new LS(0.74f), new LS(0.75f), new LS(0.76f), new LS(0.77f), new LS(0.78f),
                                                        new LS(0.81f), new LS(0.82f), new LS(0.83f), new LS(0.84f), new LS(0.85f), new LS(0.86f), new LS(0.87f), new LS(0.88f),
                                                        // G
                                                        new LS(0.11f), new LS(0.12f), new LS(0.13f), new LS(0.14f), new LS(0.15f), new LS(0.16f), new LS(0.17f), new LS(0.18f),
                                                        new LS(0.21f), new LS(0.22f), new LS(0.23f), new LS(0.24f), new LS(0.25f), new LS(0.26f), new LS(0.27f), new LS(0.28f),
                                                        new LS(0.31f), new LS(0.32f), new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f), new LS(0.37f), new LS(0.38f),
                                                        new LS(0.41f), new LS(0.42f), new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f), new LS(0.47f), new LS(0.48f),
                                                        new LS(0.51f), new LS(0.52f), new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f), new LS(0.57f), new LS(0.58f),
                                                        new LS(0.61f), new LS(0.62f), new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f), new LS(0.67f), new LS(0.68f),
                                                        new LS(0.71f), new LS(0.72f), new LS(0.73f), new LS(0.74f), new LS(0.75f), new LS(0.76f), new LS(0.77f), new LS(0.78f),
                                                        new LS(0.81f), new LS(0.82f), new LS(0.83f), new LS(0.84f), new LS(0.85f), new LS(0.86f), new LS(0.87f), new LS(0.88f),
                                                        // B
                                                        new LS(0.11f), new LS(0.12f), new LS(0.13f), new LS(0.14f), new LS(0.15f), new LS(0.16f), new LS(0.17f), new LS(0.18f),
                                                        new LS(0.21f), new LS(0.22f), new LS(0.23f), new LS(0.24f), new LS(0.25f), new LS(0.26f), new LS(0.27f), new LS(0.28f),
                                                        new LS(0.31f), new LS(0.32f), new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f), new LS(0.37f), new LS(0.38f),
                                                        new LS(0.41f), new LS(0.42f), new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f), new LS(0.47f), new LS(0.48f),
                                                        new LS(0.51f), new LS(0.52f), new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f), new LS(0.57f), new LS(0.58f),
                                                        new LS(0.61f), new LS(0.62f), new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f), new LS(0.67f), new LS(0.68f),
                                                        new LS(0.71f), new LS(0.72f), new LS(0.73f), new LS(0.74f), new LS(0.75f), new LS(0.76f), new LS(0.77f), new LS(0.78f),
                                                        new LS(0.81f), new LS(0.82f), new LS(0.83f), new LS(0.84f), new LS(0.85f), new LS(0.86f), new LS(0.87f), new LS(0.88f)
                                                    ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LS>>(result);
        Assert.InRange(((PlanarPixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal([
            new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f),
            new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f),
            new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f),
            new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f),
            new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f),
            new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f),
            new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f),
            new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f),
            new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f),
            new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f),
            new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f),
            new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f)
        ], ((PlanarPixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb888()
    {
        // Arrange
        var crop = new Crop();
        PlanarPixelBuffer<L8> pixelBuffer = new(8, 8, [
                                                    // R
                                                    new L8(11), new L8(12), new L8(13), new L8(14), new L8(15), new L8(16), new L8(17), new L8(18),
                                                    new L8(21), new L8(22), new L8(23), new L8(24), new L8(25), new L8(26), new L8(27), new L8(28),
                                                    new L8(31), new L8(32), new L8(33), new L8(34), new L8(35), new L8(36), new L8(37), new L8(38),
                                                    new L8(41), new L8(42), new L8(43), new L8(44), new L8(45), new L8(46), new L8(47), new L8(48),
                                                    new L8(51), new L8(52), new L8(53), new L8(54), new L8(55), new L8(56), new L8(57), new L8(58),
                                                    new L8(61), new L8(62), new L8(63), new L8(64), new L8(65), new L8(66), new L8(67), new L8(68),
                                                    new L8(71), new L8(72), new L8(73), new L8(74), new L8(75), new L8(76), new L8(77), new L8(78),
                                                    new L8(81), new L8(82), new L8(83), new L8(84), new L8(85), new L8(86), new L8(87), new L8(88),
                                                    // G
                                                    new L8(11), new L8(12), new L8(13), new L8(14), new L8(15), new L8(16), new L8(17), new L8(18),
                                                    new L8(21), new L8(22), new L8(23), new L8(24), new L8(25), new L8(26), new L8(27), new L8(28),
                                                    new L8(31), new L8(32), new L8(33), new L8(34), new L8(35), new L8(36), new L8(37), new L8(38),
                                                    new L8(41), new L8(42), new L8(43), new L8(44), new L8(45), new L8(46), new L8(47), new L8(48),
                                                    new L8(51), new L8(52), new L8(53), new L8(54), new L8(55), new L8(56), new L8(57), new L8(58),
                                                    new L8(61), new L8(62), new L8(63), new L8(64), new L8(65), new L8(66), new L8(67), new L8(68),
                                                    new L8(71), new L8(72), new L8(73), new L8(74), new L8(75), new L8(76), new L8(77), new L8(78),
                                                    new L8(81), new L8(82), new L8(83), new L8(84), new L8(85), new L8(86), new L8(87), new L8(88),
                                                    // B
                                                    new L8(11), new L8(12), new L8(13), new L8(14), new L8(15), new L8(16), new L8(17), new L8(18),
                                                    new L8(21), new L8(22), new L8(23), new L8(24), new L8(25), new L8(26), new L8(27), new L8(28),
                                                    new L8(31), new L8(32), new L8(33), new L8(34), new L8(35), new L8(36), new L8(37), new L8(38),
                                                    new L8(41), new L8(42), new L8(43), new L8(44), new L8(45), new L8(46), new L8(47), new L8(48),
                                                    new L8(51), new L8(52), new L8(53), new L8(54), new L8(55), new L8(56), new L8(57), new L8(58),
                                                    new L8(61), new L8(62), new L8(63), new L8(64), new L8(65), new L8(66), new L8(67), new L8(68),
                                                    new L8(71), new L8(72), new L8(73), new L8(74), new L8(75), new L8(76), new L8(77), new L8(78),
                                                    new L8(81), new L8(82), new L8(83), new L8(84), new L8(85), new L8(86), new L8(87), new L8(88)
                                                ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(result);
        Assert.InRange(((PlanarPixelBuffer<L8>)result).Pixels[0].Value, byte.MinValue, byte.MaxValue);
        Assert.Equal([
            new L8(33), new L8(34), new L8(35), new L8(36),
            new L8(43), new L8(44), new L8(45), new L8(46),
            new L8(53), new L8(54), new L8(55), new L8(56),
            new L8(63), new L8(64), new L8(65), new L8(66),
            new L8(33), new L8(34), new L8(35), new L8(36),
            new L8(43), new L8(44), new L8(45), new L8(46),
            new L8(53), new L8(54), new L8(55), new L8(56),
            new L8(63), new L8(64), new L8(65), new L8(66),
            new L8(33), new L8(34), new L8(35), new L8(36),
            new L8(43), new L8(44), new L8(45), new L8(46),
            new L8(53), new L8(54), new L8(55), new L8(56),
            new L8(63), new L8(64), new L8(65), new L8(66)
        ], ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb161616()
    {
        // Arrange
        var crop = new Crop();
        PlanarPixelBuffer<L16> pixelBuffer = new(8, 8, [
                                                        // R
                                                        new L16(1100), new L16(1200), new L16(1300), new L16(1400), new L16(1500), new L16(1600), new L16(1700), new L16(1800),
                                                        new L16(2100), new L16(2200), new L16(2300), new L16(2400), new L16(2500), new L16(2600), new L16(2700), new L16(2800),
                                                        new L16(3100), new L16(3200), new L16(3300), new L16(3400), new L16(3500), new L16(3600), new L16(3700), new L16(3800),
                                                        new L16(4100), new L16(4200), new L16(4300), new L16(4400), new L16(4500), new L16(4600), new L16(4700), new L16(4800),
                                                        new L16(5100), new L16(5200), new L16(5300), new L16(5400), new L16(5500), new L16(5600), new L16(5700), new L16(5800),
                                                        new L16(6100), new L16(6200), new L16(6300), new L16(6400), new L16(6500), new L16(6600), new L16(6700), new L16(6800),
                                                        new L16(7100), new L16(7200), new L16(7300), new L16(7400), new L16(7500), new L16(7600), new L16(7700), new L16(7800),
                                                        new L16(8100), new L16(8200), new L16(8300), new L16(8400), new L16(8500), new L16(8600), new L16(8700), new L16(8800),
                                                        // G
                                                        new L16(1100), new L16(1200), new L16(1300), new L16(1400), new L16(1500), new L16(1600), new L16(1700), new L16(1800),
                                                        new L16(2100), new L16(2200), new L16(2300), new L16(2400), new L16(2500), new L16(2600), new L16(2700), new L16(2800),
                                                        new L16(3100), new L16(3200), new L16(3300), new L16(3400), new L16(3500), new L16(3600), new L16(3700), new L16(3800),
                                                        new L16(4100), new L16(4200), new L16(4300), new L16(4400), new L16(4500), new L16(4600), new L16(4700), new L16(4800),
                                                        new L16(5100), new L16(5200), new L16(5300), new L16(5400), new L16(5500), new L16(5600), new L16(5700), new L16(5800),
                                                        new L16(6100), new L16(6200), new L16(6300), new L16(6400), new L16(6500), new L16(6600), new L16(6700), new L16(6800),
                                                        new L16(7100), new L16(7200), new L16(7300), new L16(7400), new L16(7500), new L16(7600), new L16(7700), new L16(7800),
                                                        new L16(8100), new L16(8200), new L16(8300), new L16(8400), new L16(8500), new L16(8600), new L16(8700), new L16(8800),
                                                        // B
                                                        new L16(1100), new L16(1200), new L16(1300), new L16(1400), new L16(1500), new L16(1600), new L16(1700), new L16(1800),
                                                        new L16(2100), new L16(2200), new L16(2300), new L16(2400), new L16(2500), new L16(2600), new L16(2700), new L16(2800),
                                                        new L16(3100), new L16(3200), new L16(3300), new L16(3400), new L16(3500), new L16(3600), new L16(3700), new L16(3800),
                                                        new L16(4100), new L16(4200), new L16(4300), new L16(4400), new L16(4500), new L16(4600), new L16(4700), new L16(4800),
                                                        new L16(5100), new L16(5200), new L16(5300), new L16(5400), new L16(5500), new L16(5600), new L16(5700), new L16(5800),
                                                        new L16(6100), new L16(6200), new L16(6300), new L16(6400), new L16(6500), new L16(6600), new L16(6700), new L16(6800),
                                                        new L16(7100), new L16(7200), new L16(7300), new L16(7400), new L16(7500), new L16(7600), new L16(7700), new L16(7800),
                                                        new L16(8100), new L16(8200), new L16(8300), new L16(8400), new L16(8500), new L16(8600), new L16(8700), new L16(8800)
                                                    ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L16>>(result);
        Assert.InRange(((PlanarPixelBuffer<L16>)result).Pixels[0].Value, ushort.MinValue, ushort.MaxValue);
        Assert.Equal([
            new L16(3300), new L16(3400), new L16(3500), new L16(3600),
            new L16(4300), new L16(4400), new L16(4500), new L16(4600),
            new L16(5300), new L16(5400), new L16(5500), new L16(5600),
            new L16(6300), new L16(6400), new L16(6500), new L16(6600),
            new L16(3300), new L16(3400), new L16(3500), new L16(3600),
            new L16(4300), new L16(4400), new L16(4500), new L16(4600),
            new L16(5300), new L16(5400), new L16(5500), new L16(5600),
            new L16(6300), new L16(6400), new L16(6500), new L16(6600),
            new L16(3300), new L16(3400), new L16(3500), new L16(3600),
            new L16(4300), new L16(4400), new L16(4500), new L16(4600),
            new L16(5300), new L16(5400), new L16(5500), new L16(5600),
            new L16(6300), new L16(6400), new L16(6500), new L16(6600)
        ], ((PlanarPixelBuffer<L16>)result).Pixels.ToArray());
    }
}
