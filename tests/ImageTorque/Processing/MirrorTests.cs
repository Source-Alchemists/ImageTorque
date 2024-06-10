using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class MirrorTests
{
    [Theory]
    [InlineData(MirrorMode.Horizontal, 0.4f, 0f, 0.6f, 0.5f)]
    [InlineData(MirrorMode.Vertical, 0.5f, 0.6f, 0f, 0.4f)]
    [InlineData(MirrorMode.VerticalHorizontal, 0.6f, 0.5f, 0.4f, 0f)]
    public void Test_Mirror_Mono(MirrorMode mirrorMode, float ep1, float ep2, float ep3, float ep4)
    {
        // Arrange
        var expectedPixels = new LS[] { ep1, ep2, ep3, ep4 };
        var mirror = new Mirror();
        PixelBuffer<LS> pixelBuffer = new(2, 2, [0f, 0.4f, 0.5f, 0.6f]);

        // Act
        IPixelBuffer result = mirror.Execute(new MirrorParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal(expectedPixels, ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(MirrorMode.Horizontal, 102, 0, 153, 128)]
    [InlineData(MirrorMode.Vertical, 128, 153, 0, 102)]
    [InlineData(MirrorMode.VerticalHorizontal, 153, 128, 102, 0)]
    public void Test_Mirror_Mono8(MirrorMode mirrorMode, byte ep1, byte ep2, byte ep3, byte ep4)
    {
        // Arrange
        var expectedPixels = new L8[] { ep1, ep2, ep3, ep4 };
        var mirror = new Mirror();
        PixelBuffer<L8> pixelBuffer = new(2, 2, [0, 102, 128, 153]);

        // Act
        IPixelBuffer result = mirror.Execute(new MirrorParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        Assert.InRange(((PixelBuffer<L8>)result).Pixels[0].Value, 0, byte.MaxValue);
        Assert.Equal(expectedPixels, ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Theory]
    [InlineData(MirrorMode.Horizontal, 26214, 0, 39321, 32768)]
    [InlineData(MirrorMode.Vertical, 32768, 39321, 0, 26214)]
    [InlineData(MirrorMode.VerticalHorizontal, 39321, 32768, 26214, 0)]
    public void Test_Mirror_Mono16(MirrorMode mirrorMode, ushort ep1, ushort ep2, ushort ep3, ushort ep4)
    {
        // Arrange
        var expectedPixels = new L16[] { ep1, ep2, ep3, ep4 };
        var mirror = new Mirror();
        PixelBuffer<L16> pixelBuffer = new(2, 2, [0, 26214, 32768, 39321]);

        // Act
        IPixelBuffer result = mirror.Execute(new MirrorParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        Assert.InRange(((PixelBuffer<L16>)result).Pixels[0].Value, 0, ushort.MaxValue);
        Assert.Equal(expectedPixels, ((PixelBuffer<L16>)result).Pixels.ToArray());
    }

}
