using ImageTorque.Buffers;
using ImageTorque.Pixels;
using ImageTorque.Processing;

namespace ImageTorque.Tests.Encoding;

public class EncoderTests : IDisposable
{
    private readonly PackedPixelBuffer<Mono16> _packedPixelBufferMono16;
    private readonly PackedPixelBuffer<Mono8> _packedPixelBufferMono8;
    private readonly PackedPixelBuffer<Rgb24> _packedPixelBufferRgb24;
    private readonly PackedPixelBuffer<Rgb48> _packedPixelBufferRgb48;
    private bool _disposedValue;

    public EncoderTests()
    {
        _packedPixelBufferMono8 = new PackedPixelBuffer<Mono8>(2, 2, new Mono8[] { 0x00, 0x01, 0x80, 0xFF });
        _packedPixelBufferMono16 = new PackedPixelBuffer<Mono16>(2, 2, new Mono16[] { 0x0000, 0x0101, 0x8080, 0xFFFF });
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
    }

    [Theory]
    [InlineData(EncoderType.Png)]
    [InlineData(EncoderType.Bmp)]
    public void EncodeMono8(EncoderType encodeType)
    {
        // Arrange
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferMono8.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType
        });

        // Assert
        Assert.NotEqual(0, stream.Length);
    }

    [Theory]
    [InlineData(EncoderType.Png)]
    [InlineData(EncoderType.Bmp)]
    public void EncodeMono16(EncoderType encodeType)
    {
        // Arrange
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferMono16.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType
        });

        // Assert
        Assert.NotEqual(0, stream.Length);
    }

    [Theory]
    [InlineData(EncoderType.Png)]
    [InlineData(EncoderType.Bmp)]
    public void EncodeRgb24(EncoderType encodeType)
    {
        // Arrange
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType
        });

        // Assert
        Assert.NotEqual(0, stream.Length);
    }

    [Theory]
    [InlineData(EncoderType.Png)]
    [InlineData(EncoderType.Bmp)]
    public void EncodeRgb48(EncoderType encodeType)
    {
        // Arrange
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType
        });

        // Assert
        Assert.NotEqual(0, stream.Length);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _packedPixelBufferMono8.Dispose();
                _packedPixelBufferMono16.Dispose();
                _packedPixelBufferRgb24.Dispose();
                _packedPixelBufferRgb48.Dispose();
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
