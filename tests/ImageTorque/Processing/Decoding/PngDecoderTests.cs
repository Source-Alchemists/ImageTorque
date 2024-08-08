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

using ImageTorque.Buffers;
using ImageTorque.Codecs.Png;
using ImageTorque.Pixels;
using ImageTorque.Tests;

namespace ImageTorque.Processing.Tests;

public class PngDecoderTests
{
    [Fact]
    public void Test_Identify()
    {
        // Arrange
        Configuration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        var decoder = new PngDecoder();
        using var stream = new FileStream("./lena24.png", FileMode.Open, FileAccess.Read);

        // Act
        ImageInfo imageInfo = decoder.Identify(stream, configuration);

        // Assert
        Assert.Equal(512, imageInfo.Width);
        Assert.Equal(512, imageInfo.Height);
        Assert.Equal(24, imageInfo.BitsPerPixel);
    }

    [Fact]
    public void Test_Decode_L8()
    {
        // Arrange
        Configuration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        var decoder = new PngDecoder();
        using var stream = new FileStream("./lena8.png", FileMode.Open, FileAccess.Read);

        // Act
        var pixelBuffer = decoder.Decode(stream, configuration) as PixelBuffer<L8>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(pixelBuffer!.Pixels));
        Assert.Equal("4ba5d347f3d6b97e75aa1aa543042552f5ac8489468c2a6b046f03d5e648eaab", hash);
    }

    [Fact]
    public void Test_Decode_Rgb24()
    {
        // Arrange
        Configuration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        var decoder = new PngDecoder();
        using var stream = new FileStream("./lena24.png", FileMode.Open, FileAccess.Read);

        // Act
        var pixelBuffer = decoder.Decode(stream, configuration) as PixelBuffer<Rgb24>;

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(pixelBuffer!.Pixels));
        Assert.Equal("01f8ff0e23a809255ae52a9857a7aeb0f89d92610205fef29c36c3d359ac3339", hash);
    }
}
