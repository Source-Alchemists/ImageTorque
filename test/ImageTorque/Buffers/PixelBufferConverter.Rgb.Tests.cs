using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Tests.Buffers;

public class PixelBufferConverterRgbTests
{
    private readonly PackedPixelBuffer<Rgb> _packedPixelBufferRgb;
    private readonly PackedPixelBuffer<Rgb24> _packedPixelBufferRgb24;
    private readonly PackedPixelBuffer<Rgb48> _packedPixelBufferRgb48;
    private readonly PlanarPixelBuffer<RgbFFF> _planarPixelBufferRgbFFF;
    private readonly PlanarPixelBuffer<Rgb888> _planarPixelBufferRgb888;
    private readonly PlanarPixelBuffer<Rgb161616> _planarPixelBufferRgb161616;
    private readonly PixelBufferConverter _converter = new();

    public PixelBufferConverterRgbTests()
    {
        _packedPixelBufferRgb = new PackedPixelBuffer<Rgb>(2, 2, new[] {
                                                                new Rgb(0f, 0f, 0f),
                                                                new Rgb(0.0039215687f, 0.0078431373f, 0.0117647059f),
                                                                new Rgb(0.015686275f, 0.0196078432f, 0.0235294118f),
                                                                new Rgb(1f, 1f, 1f) });
        _packedPixelBufferRgb24 = new PackedPixelBuffer<Rgb24>(2, 2, new[] {
                                                                new Rgb24(0x00, 0x00, 0x00),
                                                                new Rgb24(0x01, 0x02, 0x03),
                                                                new Rgb24(0x04, 0x05, 0x06),
                                                                new Rgb24(0xFF, 0xFF, 0xFF) });
        _packedPixelBufferRgb48 = new PackedPixelBuffer<Rgb48>(2, 2, new[] {
                                                                new Rgb48(0x0000, 0x0000, 0x0000),
                                                                new Rgb48(0x101, 0x202, 0x303),
                                                                new Rgb48(0x404, 0x505, 0x606),
                                                                new Rgb48(0xFFFF, 0xFFFF, 0xFFFF) });

        _planarPixelBufferRgbFFF = new PlanarPixelBuffer<RgbFFF>(2, 2, new[] {
                                                                new RgbFFF(0x00), new RgbFFF(0.0039215687f), new RgbFFF(0.015686275f), new RgbFFF(1.0f),
                                                                new RgbFFF(0x00), new RgbFFF(0.0078431373f), new RgbFFF(0.0196078432f), new RgbFFF(1.0f),
                                                                new RgbFFF(0x00), new RgbFFF(0.0117647059f), new RgbFFF(0.0235294118f), new RgbFFF(1.0f) });
        _planarPixelBufferRgb888 = new PlanarPixelBuffer<Rgb888>(2, 2, new[] {
                                                                new Rgb888(0x00), new Rgb888(0x01), new Rgb888(0x04), new Rgb888(0xFF),
                                                                new Rgb888(0x00), new Rgb888(0x02), new Rgb888(0x05), new Rgb888(0xFF),
                                                                new Rgb888(0x00), new Rgb888(0x03), new Rgb888(0x06), new Rgb888(0xFF) });
        _planarPixelBufferRgb161616 = new PlanarPixelBuffer<Rgb161616>(2, 2, new[] {
                                                                new Rgb161616(0x0000), new Rgb161616(0x0101), new Rgb161616(0x0404), new Rgb161616(0xFFFF),
                                                                new Rgb161616(0x0000), new Rgb161616(0x0202), new Rgb161616(0x0505), new Rgb161616(0xFFFF),
                                                                new Rgb161616(0x0000), new Rgb161616(0x0303), new Rgb161616(0x0606), new Rgb161616(0xFFFF) });
    }

    [Fact]
    public void TestRgbToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb161616>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb161616>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb161616>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb161616>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb161616>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb161616>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb888>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb888>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb888>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb888>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb888>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb888>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgbToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<RgbFFF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<RgbFFF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb24ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<RgbFFF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<RgbFFF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb48ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<RgbFFF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<RgbFFF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb888>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb888>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<RgbFFF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<RgbFFF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgbFFF()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<RgbFFF>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<RgbFFF>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgbFFF, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb161616()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb161616>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb161616>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb161616, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb888()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<Rgb888>)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<Rgb888>>(resultBuffer);
        Assert.Equal(_planarPixelBufferRgb888, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgb161616ToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb161616.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgb888ToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgb888.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb24()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb24>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb24>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb24, resultBuffer);
    }

    [Fact]
    public void TestRgbFFFToRgb48()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new ConvertParameters
        {
            Input = _planarPixelBufferRgbFFF.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<Rgb48>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<Rgb48>>(resultBuffer);
        Assert.Equal(_packedPixelBufferRgb48, resultBuffer);
    }
}
