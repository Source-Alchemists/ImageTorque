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

using System.Runtime.InteropServices;

using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick.Tests;

public class TiffCodecTests
{
    private readonly IConfiguration _configuration = ConfigurationFactory.Build([new TiffCodec()]);

    [Fact]
    public void Test_DecodeAndEncode_L8()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena8.tif");

        // Act
        using Buffers.IPixelBuffer<L8>? pixelBuffer = decoder.Decode(stream, _configuration) as Buffers.IPixelBuffer<L8>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(pixelBuffer!.Pixels));
        Assert.Equal("eb6cd30ede15f638d0fccba36977d7c6a6ae3f31b8bd8f78dffdfcb47437acc7", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<L8>(), "tif");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("b8cd9dd7ea8f1eea4bf843a733c3d4fda05d49a345a9aeca1e4a63b19f3d29c0", outHash);
    }

    [Fact]
    public void Test_DecodeAndEncode_Rgb24()
    {
        // Arrange
        var codec = new PngCodec();
        IImageDecoder decoder = codec.Decoder;
        IImageEncoder encoder = codec.Encoder;

        using FileStream stream = File.OpenRead("./lena24.tif");

        // Act
        using Buffers.IPixelBuffer<Rgb24>? pixelBuffer = decoder.Decode(stream, _configuration) as Buffers.IPixelBuffer<Rgb24>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(pixelBuffer!.Pixels));
        Assert.Equal("01f8ff0e23a809255ae52a9857a7aeb0f89d92610205fef29c36c3d359ac3339", hash);
        Assert.Equal(512, pixelBuffer.Width);
        Assert.Equal(512, pixelBuffer.Height);

        // Act
        using var outStream = new MemoryStream();
        var image = new Image(pixelBuffer!);
        encoder.Encode(outStream, image.AsPacked<Rgb24>(), "tif");

        // Assert
        Assert.True(outStream.Length > 0);
        outStream.Position = 0;
        string outHash = TestHelper.CreateHash(outStream.ToArray());
        Assert.Equal("030043fc3ee9921af99d7040f185aea6bc46247808b54dc24d261d96c206d587", outHash);
    }
}
