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
using ImageTorque.Codecs.Bmp;
using ImageTorque.Pixels;
using ImageTorque.Tests;

namespace ImageTorque.Processing.Tests;

public class BmpEncoderTests
{
    [Fact]
    public void Test_Encode_L8()
    {
        // Arrange
        var encoder = new BmpEncoder();
        var decoder = new BmpDecoder();
        using var inputStream = new FileStream("./lena8.bmp", FileMode.Open, FileAccess.Read);
        var pixelBuffer = decoder.Decode(inputStream) as PixelBuffer<L8>;
        using var outputStream = new MemoryStream();

        // Act
        encoder.Encode(outputStream, (ReadOnlyPackedPixelBuffer<L8>)pixelBuffer!.AsReadOnly(), "bmp");

        // Assert
        Assert.True(outputStream.Length > 0);
        outputStream.Position = 0;
        string hash = TestHelper.CreateHash(outputStream.ToArray());
        Assert.Equal("51878087d22511c68c24449e2ef68286a685df6d5c734762bdbd8e052a1235cf", hash);
    }

    [Fact]
    public void Test_Encode_Rgb24()
    {
        // Arrange
        var encoder = new BmpEncoder();
        var decoder = new BmpDecoder();
        using var inputStream = new FileStream("./lena24.bmp", FileMode.Open, FileAccess.Read);
        var pixelBuffer = decoder.Decode(inputStream) as PixelBuffer<Rgb24>;
        using var outputStream = new MemoryStream();

        // Act
        encoder.Encode(outputStream, (ReadOnlyPackedPixelBuffer<Rgb24>)pixelBuffer!.AsReadOnly(), "bmp");

        // Assert
        Assert.True(outputStream.Length > 0);
        outputStream.Position = 0;
        string hash = TestHelper.CreateHash(outputStream.ToArray());
        Assert.Equal("d414a5c847b02454113912841b27f9eb469d5234c3fc6e6d0ddc66eeef2eb56f", hash);
    }
}
