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
using ImageTorque.Codecs.Bmp;
using ImageTorque.Pixels;
using ImageTorque.Tests;

namespace ImageTorque.Processing.Tests;

public class MirrorTests
{
    [Theory]
    [InlineData("./lena8.bmp", MirrorMode.Horizontal, "443dd8036924955fe0352bd8b693ba88729b061b22cf85360bc497cccd96d688")]
    [InlineData("./lena8.bmp", MirrorMode.Vertical, "5f25ffbb86f800aa18a364ead6c675ee99e159352b06c95cc5a1cfacddd7d743")]
    [InlineData("./lena8.bmp", MirrorMode.VerticalHorizontal, "cdf73509748cec90179d737ec3de9a3b7ed1d8ce686a36dd82b41ecfaad7e1a8")]
    public void Test_Mirror_Mono(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new BmpDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PixelBuffer<LS>)
        });

        // Act
        PixelBuffer<LS> result = (PixelBuffer<LS>)mirror.Execute(new MirrorParameters
        {
            Input = convertedPixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<LS, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena24.bmp", MirrorMode.Horizontal, "6713e23e288af9716d4ff1a45a5440289bdf9081a290bae6b4ac34fa87bed49d")]
    [InlineData("./lena24.bmp", MirrorMode.Vertical, "d3ffa8f0badeabe5a8b991b0c11fcd9add2a87c94a2873f115f7c53592b25ab0")]
    [InlineData("./lena24.bmp", MirrorMode.VerticalHorizontal, "dbf758cd57c308367203c75d8f5a2738819f2fdc837adbac3d31ad79befd3456")]
    public void Test_Mirror_Rgb(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new BmpDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Pixels.Rgb>)
        });

        // Act
        PixelBuffer<Pixels.Rgb> result = (PixelBuffer<Pixels.Rgb>)mirror.Execute(new MirrorParameters
        {
            Input = convertedPixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Pixels.Rgb, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena8.bmp", MirrorMode.Horizontal, "ce45cb50a97b5da1353d8bffdfd65badd46ef0ea2cd7fe658e5ccd48fe55dc62")]
    [InlineData("./lena8.bmp", MirrorMode.Vertical, "66491229dfdce301ae861fb6973c75106baff102d43f41c1ebfd2d130cf01ef5")]
    [InlineData("./lena8.bmp", MirrorMode.VerticalHorizontal, "1119c846cd068455d3db1bf47a797dd54c7a9721a88034231055682cfb211654")]
    public void Test_Mirror_Mono8(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new BmpDecoder();
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<L8> result = (PixelBuffer<L8>)mirror.Execute(new MirrorParameters
        {
            Input = pixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena16.png", MirrorMode.Horizontal, "b9e817adc2a3b8e93ed365e9daca112011845a2f2d43c9d62eb6e060b94bcd5e")]
    [InlineData("./lena16.png", MirrorMode.Vertical, "f279bfcedaaaf72e6e60d84921461e8e953c9c3092315594dc6ac87853587afb")]
    [InlineData("./lena16.png", MirrorMode.VerticalHorizontal, "debead252749dbd9b351a5cff4e54ba247e5020ab0c48ad09503df3715d40366")]
    public void Test_Mirror_Mono16(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new Codecs.Png.PngDecoder();
        Configuration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<L16> result = (PixelBuffer<L16>)mirror.Execute(new MirrorParameters
        {
            Input = pixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L16, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena24.bmp", MirrorMode.Horizontal, "16ab3f29d0d37cb2ed161125c8084d960d010d5df0de0d5e0699598abae63874")]
    [InlineData("./lena24.bmp", MirrorMode.Vertical, "564bef941be8e6ef971332b7ead4c9821e0b680861bfe5a6fd6069118e13a969")]
    [InlineData("./lena24.bmp", MirrorMode.VerticalHorizontal, "44e255860f64800dda80eab1941104721ae5ecc39eba53d4635b9f168eac7183")]
    public void Test_Mirror_Rgb24(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new BmpDecoder();
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<Rgb24> result = (PixelBuffer<Rgb24>)mirror.Execute(new MirrorParameters
        {
            Input = pixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena48.png", MirrorMode.Horizontal, "4460611dc46d7396379ad831e035de6dd4e4bd94703bbd8423b5f119dd083e7d")]
    [InlineData("./lena48.png", MirrorMode.Vertical, "4ba356310b4d2141af154db1d615b81e4ea9f6a3c9a65faa1ef64f51737341bf")]
    [InlineData("./lena48.png", MirrorMode.VerticalHorizontal, "21a1904bfbdfb20d52d457e01430c9abbea2e105b3eadb9a7bb9666a495f5b1a")]
    public void Test_Mirror_Rgb48(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new Codecs.Png.PngDecoder();
        Configuration configuration = ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<Rgb48> result = (PixelBuffer<Rgb48>)mirror.Execute(new MirrorParameters
        {
            Input = pixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb48, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena24.bmp", MirrorMode.Horizontal, "28a7478a054d84969e7bd70e521d0ef0e0d5ad0ca4f94a7b2d000d9f6c502842")]
    [InlineData("./lena24.bmp", MirrorMode.Vertical, "66d97d349a9468bc10a6af3974b761026035a406f51bf85f1765580c548c1299")]
    [InlineData("./lena24.bmp", MirrorMode.VerticalHorizontal, "764b50347f85a706ece07470fadfadc99f0a5af4322237d5cf787f0584115351")]
    public void Test_Mirror_Rgb_Planar(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new BmpDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);
        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<LS>)
        });

        // Act
        PixelBuffer<LS> result = (PlanarPixelBuffer<LS>)mirror.Execute(new MirrorParameters
        {
            Input = convertedPixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<LS, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena24.bmp", MirrorMode.Horizontal, "a493241fb7195ac4f90cbbc7d76d1ee1a18427ae9d24506205d4aa190484f331")]
    [InlineData("./lena24.bmp", MirrorMode.Vertical, "fdc6ca6e399cda81ee06fb6c4feb076f927f26b86c2f53d696102be3b1f4ca9d")]
    [InlineData("./lena24.bmp", MirrorMode.VerticalHorizontal, "d76e25b0aedc659ab4cdc8862ad29741bad508c2b5ccbfd4caff551afd98384f")]
    public void Test_Mirror_Rgb24_Planar(string imagePath, MirrorMode mirrorMode, string expectedHash)
    {
        // Arrange
        var mirror = new Mirror();
        var decoder = new BmpDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration = ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);
        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L8>)
        });

        // Act
        PixelBuffer<L8> result = (PlanarPixelBuffer<L8>)mirror.Execute(new MirrorParameters
        {
            Input = convertedPixelBuffer!.AsReadOnly(),
            MirrorMode = mirrorMode
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }
}
