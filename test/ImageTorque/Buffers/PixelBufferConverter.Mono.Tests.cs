using ImageTorque.Buffers;
using ImageTorque.Pixels;
using ImageTorque.Processing;

namespace ImageTorque.Tests.Buffers;

public class PixelBufferConverterMonoTests
{
    private readonly PackedPixelBuffer<LF> _packedPixelBufferMono;
    private readonly PackedPixelBuffer<L16> _packedPixelBufferMono16;
    private readonly PackedPixelBuffer<L8> _packedPixelBufferMono8;
    private readonly PixelBufferConverter _converter = new();

    public PixelBufferConverterMonoTests()
    {
        _packedPixelBufferMono = new PackedPixelBuffer<LF>(2, 2, new LF[] { 0f, 0.003921569f, 0.5019608f, 1f });
        _packedPixelBufferMono8 = new PackedPixelBuffer<L8>(2, 2, new L8[] { 0x00, 0x01, 0x80, 0xFF });
        _packedPixelBufferMono16 = new PackedPixelBuffer<L16>(2, 2, new L16[] { 0x0000, 0x0101, 0x8080, 0xFFFF });
    }

    [Fact]
    public void TestMonoToMono8()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono8, resultBuffer);
    }

    [Fact]
    public void TestMonoToMono16()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono16, resultBuffer);
    }

    [Fact]
    public void TestMono8ToMono()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono8.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<LF>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<LF>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono, resultBuffer);
    }

    [Fact]
    public void TestMono8ToMono16()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono8.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<L16>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<L16>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono16, resultBuffer);
    }

    [Fact]
    public void TestMono16ToMono()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono16.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<LF>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<LF>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono, resultBuffer);
    }

    [Fact]
    public void TestMono16ToMono8()
    {
        // Act
        using IPixelBuffer resultBuffer = _converter.Execute(new PixelBufferConvertParameters
        {
            Input = _packedPixelBufferMono16.AsReadOnly(),
            OutputType = typeof(PackedPixelBuffer<L8>)
        });

        // Assert
        Assert.IsType<PackedPixelBuffer<L8>>(resultBuffer);
        Assert.Equal(_packedPixelBufferMono8, resultBuffer);
    }
}
