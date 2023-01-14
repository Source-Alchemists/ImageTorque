using ImageTorque.Buffers;
using ImageTorque.Pixels;
using ImageTorque.Processing;

namespace ImageTorque.Tests.Buffers;

public class PixelBufferConverterRgbTests
{
    private readonly PixelBuffer<Rgb> _packedPixelBufferRgb;
    private readonly PixelBuffer<Rgb24> _packedPixelBufferRgb24;
    private readonly PixelBuffer<Rgb48> _packedPixelBufferRgb48;
    private readonly PlanarPixelBuffer<LF> _planarPixelBufferRgbFFF;
    private readonly PlanarPixelBuffer<L8> _planarPixelBufferRgb888;
    private readonly PlanarPixelBuffer<L16> _planarPixelBufferRgb161616;
    private readonly PixelBufferConverter _converter = new();

    public PixelBufferConverterRgbTests()
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
        _packedPixelBufferRgb48 = new PixelBuffer<Rgb48>(2, 2, new[] {
                                                                new Rgb48(0x0000, 0x0000, 0x0000),
                                                                new Rgb48(0x101, 0x202, 0x303),
                                                                new Rgb48(0x404, 0x505, 0x606),
                                                                new Rgb48(0xFFFF, 0xFFFF, 0xFFFF) });

        _planarPixelBufferRgbFFF = new PlanarPixelBuffer<LF>(2, 2, new[] {
                                                                new LF(0x00), new LF(0.0039215687f), new LF(0.015686275f), new LF(1.0f),
                                                                new LF(0x00), new LF(0.0078431373f), new LF(0.0196078432f), new LF(1.0f),
                                                                new LF(0x00), new LF(0.0117647059f), new LF(0.0235294118f), new LF(1.0f) });
        _planarPixelBufferRgb888 = new PlanarPixelBuffer<L8>(2, 2, new[] {
                                                                new L8(0x00), new L8(0x01), new L8(0x04), new L8(0xFF),
                                                                new L8(0x00), new L8(0x02), new L8(0x05), new L8(0xFF),
                                                                new L8(0x00), new L8(0x03), new L8(0x06), new L8(0xFF) });
        _planarPixelBufferRgb161616 = new PlanarPixelBuffer<L16>(2, 2, new[] {
                                                                new L16(0x0000), new L16(0x0101), new L16(0x0404), new L16(0xFFFF),
                                                                new L16(0x0000), new L16(0x0202), new L16(0x0505), new L16(0xFFFF),
                                                                new L16(0x0000), new L16(0x0303), new L16(0x0606), new L16(0xFFFF) });
    }

    [Fact]
    public void TestRgbToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<LF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<LF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<LF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<LF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<LF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }
}
