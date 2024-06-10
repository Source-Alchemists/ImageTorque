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
        Assert.Equal([
                        0.33f, 0.34f, 0.35f, 0.36f,
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
        Assert.Equal([
                        33, 34, 35, 36,
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
}
