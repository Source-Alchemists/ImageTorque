using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class PixelBufferConverterMonoTests
{
    private readonly PixelBuffer<LS> _packedPixelBufferMono;
    private readonly PixelBuffer<L8> _packedPixelBufferMono8;
    private readonly PixelBuffer<L16> _packedPixelBufferMono16;
    private readonly PixelBuffer<Rgb24> _packedPixelBufferRgb24;
    private readonly PixelBufferConverter _converter = new();

    public PixelBufferConverterMonoTests()
    {
        _packedPixelBufferMono = new PixelBuffer<LS>(2, 2, new LS[] { 0f, 0.003921569f, 0.5019608f, 1f });
        _packedPixelBufferMono8 = new PixelBuffer<L8>(2, 2, new L8[] { 0x00, 0x01, 0x80, 0xFF });
        _packedPixelBufferMono16 = new PixelBuffer<L16>(2, 2, new L16[] { 0x0000, 0x0101, 0x8080, 0xFFFF });
        _packedPixelBufferRgb24 = new PixelBuffer<Rgb24>(2, 2, new [] {
                                                                    new Rgb24(0x00, 0x00, 0x00),
                                                                    new Rgb24(0x01, 0x01, 0x01),
                                                                    new Rgb24(0x80, 0x80, 0x80),
                                                                    new Rgb24(0xFF, 0xFF, 0xFF)});
    }

    [Fact]
    public void TestMonoToMono8()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono.AsReadOnly(),
            OutputType = typeof(PixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono8, resultBuffer);
    }

    [Fact]
    public void TestMonoToMono16()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono.AsReadOnly(),
            OutputType = typeof(PixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono16, resultBuffer);
    }

    [Fact]
    public void TestMono8ToMono()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono8.AsReadOnly(),
            OutputType = typeof(PixelBuffer<LS>)
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono, resultBuffer);
    }

    [Fact]
    public void TestMono8ToMono16()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono8.AsReadOnly(),
            OutputType = typeof(PixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono16, resultBuffer);
    }

    [Fact]
    public void TestMono8ToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono8.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestMono16ToMono()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono16.AsReadOnly(),
            OutputType = typeof(PixelBuffer<LS>)
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono, resultBuffer);
    }

    [Fact]
    public void TestMono16ToMono8()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono16.AsReadOnly(),
            OutputType = typeof(PixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono8, resultBuffer);
    }
}
