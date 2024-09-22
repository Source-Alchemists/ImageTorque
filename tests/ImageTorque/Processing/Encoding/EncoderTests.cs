/*
 * Copyright 2024 Source Alchemists
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing.Tests;

public class EncoderTests : IDisposable
{
    private readonly PixelBuffer<L16> _packedPixelBufferMono16;
    private readonly PixelBuffer<L8> _packedPixelBufferMono8;
    private readonly PixelBuffer<Rgb24> _packedPixelBufferRgb24;
    private readonly PixelBuffer<Rgb48> _packedPixelBufferRgb48;
    private bool _disposedValue;

    public EncoderTests()
    {
        _packedPixelBufferMono8 = new PixelBuffer<L8>(2, 2, new L8[] { 0x00, 0x01, 0x80, 0xFF });
        _packedPixelBufferMono16 = new PixelBuffer<L16>(2, 2, new L16[] { 0x0000, 0x0101, 0x8080, 0xFFFF });
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
    }

    [Theory]
    [InlineData("png")]
    [InlineData("bmp")]
    public void EncodeMono8(string encodeType)
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec(), new Codecs.ImageSharp.BmpCodec()]);
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferMono8.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType,
            Configuration = configuration
        });

        // Assert
        Assert.NotEqual(0, stream.Length);
    }

    [Theory]
    [InlineData("png")]
    [InlineData("bmp")]
    public void EncodeMono16(string encodeType)
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec(), new Codecs.ImageSharp.BmpCodec()]);
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferMono16.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType,
            Configuration = configuration
        });

        // Assert
        Assert.NotEqual(0, stream.Length);
    }

    [Theory]
    [InlineData("png")]
    [InlineData("bmp")]
    public void EncodeRgb24(string encodeType)
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec(), new Codecs.ImageSharp.BmpCodec()]);
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferRgb24.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType,
            Configuration = configuration
        });

        // Assert
        Assert.NotEqual(0, stream.Length);
    }

    [Theory]
    [InlineData("png")]
    [InlineData("bmp")]
    public void EncodeRgb48(string encodeType)
    {
        // Arrange
        IConfiguration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec(), new Codecs.ImageSharp.BmpCodec()]);
        var encoder = new Encoder();

        // Act
        using var stream = new MemoryStream();
        encoder.Execute(new EncoderParameters
        {
            Input = _packedPixelBufferRgb48.AsReadOnly(),
            Stream = stream,
            EncoderType = encodeType,
            Configuration = configuration
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
