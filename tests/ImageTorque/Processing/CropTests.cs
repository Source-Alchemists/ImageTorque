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

public class CropTests
{
    [Theory]
    [InlineData("./lena8.bmp", 300, 300, 200, 200, 0, "7a07d43cbdcd7383286195a6a66f492c521b875fd9897e9ae01543fd026cc572")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 90, "d1d9ba2819ffbf90713dbd7eee635db378ef0f032bbd348c50c290ad56e3920f")]
    [InlineData("./lena8.bmp", 300, 300, 200, 200, 180, "882939da2e7a771aa8fb6334af0694bc82f6c740aaa3f1dd23c77b8b68f25eef")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 270, "c7a76ae7934277a51cef5b27914a10f5f1740403592be77a6922e088fe16333d")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 55, "01ead7aaac63dedbcdf50f1298b45d005a8ca52e0d88278496dfc3744219e032")]
    public void Test_Crop_Mono(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new BmpDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration =  ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);
        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PixelBuffer<LS>)
        });

        // Act
        PixelBuffer<LS> result = (PixelBuffer<LS>)crop.Execute(new CropParameters
        {
            Input = convertedPixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<LS, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 0, "ec750622a90e7a11c6c77b855c4c9028a8b1f35e6a3cbfc5a6f7c84c7ecbb55f")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 90, "57d4787481ea7528bb77436b7c7899b86433d44f89bf4521379b1d3cc6c0ef5f")]
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 180, "db09753245004b6f32058b29286ed0bdbfe254fc3078f393c9187b4190203b01")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 270, "9648ef7ab02d87702f93ce453d0d7a7c0a55b9f24d9cedf074294e28b0b96c8f")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 55, "e49ed9206052481d0664bd2b5c9b659165f9f06f07912cceb903bb3c484415db")]
    public void Test_Crop_Rgb(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new BmpDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration =  ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);
        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PixelBuffer<Rgb>)
        });

        // Act
        PixelBuffer<Rgb> result = (PixelBuffer<Rgb>)crop.Execute(new CropParameters
        {
            Input = convertedPixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena8.bmp", 300, 300, 200, 200, 0, "c573ee9d07310077be44e9d9bc2993123715e70e57d33bd29b2a32a30bc51ce6")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 90, "61f886368b8bf5890fec6d1455e428fdde78ffc44384683de763c3a079a53387")]
    [InlineData("./lena8.bmp", 300, 300, 200, 200, 180, "b106f9d499ebef8162cc461156427b7a4441ff2a410e26922016ebb53d764381")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 270, "23ea1abe46919ca7ea23cd1dd4f4b8ff51ee5cd511fcabf575194d8705097fd0")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 55, "a8609b47b229da3692c71db7b61598c5bbce3b341a66f0a6ffe086dfe634e17b")]
    public void Test_Crop_Mono8(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new BmpDecoder();
        Configuration configuration =  ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<L8> result = (PixelBuffer<L8>)crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena16.png", 259, 259, 200, 200, 0, "ef73c81cdb3952a60e29d8fc2b74091fd4efed69251dd046fb04d9a8416a00f7")]
    [InlineData("./lena16.png", 150, 150, 200, 200, 90, "2a59315e204eb2c438ac484fd8e5def099171729861b37bbaa5d6947899d7064")]
    [InlineData("./lena16.png", 259, 259, 200, 200, 180, "cf45bdff3d66b5d52f2c4049725d1ddf9ec34e79649d8e6a29acf7fc974f3534")]
    [InlineData("./lena16.png", 150, 150, 200, 200, 270, "5008bc9595af4957936c3e300d0d97daac886724965a842e3434a8399182f2bb")]
    [InlineData("./lena16.png", 150, 150, 200, 200, 55, "357bb7e192ff8d5cd8bea7f80aa28bfea280ae61012192942bc4037169be58af")]
    public void Test_Crop_Mono16(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new Codecs.Png.PngDecoder();
        Configuration configuration =  ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<L16> result = (PixelBuffer<L16>)crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L16, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 0, "29fbe0906484165ed0e3297f23531f56319cf12fadfa56a5a1e39da60a57b0b6")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 90, "50923c365e18fa56cabe70be1d5e0d8de29782417aaa92687b1e4e729b0d6452")]
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 180, "bddabaa568cc9db4d3ba933f4f0b84ac231a23d68a678335dec7c7475c263d6d")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 270, "36a2efd3b9501830d16ebfde9eda4833f7d357344e6ed53920e9fcafd9beb280")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 55, "628942f75eeb8673a4fc2ee53c3ae4e5e705b6bf29b859c1649af320dad292c1")]
    public void Test_Crop_Rgb24_Planar(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new BmpDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration =  ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);
        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L8>)
        });

        // Act
        PixelBuffer<L8> result = (PlanarPixelBuffer<L8>)crop.Execute(new CropParameters
        {
            Input = convertedPixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L8, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 0, "203c2cce6c92a97d13ef9ea483846c83c8535f0ae105acfd2be303826d29c3e9")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 90, "56cc18b659b6f3e2aa72b23721ec471112af7603f8a51780945ad99e61b98a4d")]
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 180, "b9d74b6f38a410505248e7baece9ededcf0dce18c42343500c02370c1ba20cb2")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 270, "67dff73ff036d8217ab5ea2e7088ca93d2e2c840239fc86378beaf76a9840996")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 55, "7e453124b25c674ea608c99ab3d8e167252d205951901ae9a93657d9672dcec9")]
    public void Test_Crop_Rgb24(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new BmpDecoder();
        Configuration configuration =  ConfigurationFactory.Build([new BmpCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<Rgb24> result = (PixelBuffer<Rgb24>)crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb24, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena48.png", 259, 259, 200, 200, 0, "13b952390db69ec74e7a177a24bc7c5e585dc9a9081de0dc020ee57138f3a731")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 90, "cb6f4ec7ff08f26a35fe1317425dbed2ae256ee8cdc54a031393457160b5bcb8")]
    [InlineData("./lena48.png", 259, 259, 200, 200, 180, "b854869303ca7322b334a4be67d8689f15e6985f30c809743e329700c34b469d")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 270, "a2891510dcf5649c45b8874c4dbd345e0382571dcb9d0f40ba74be99d3fbcc09")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 55, "22c4ef317bf376dbc577b9331a09077a61b5fba3f48a5370bf6be713839b42af")]
    public void Test_Crop_Rgb48(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new Codecs.Png.PngDecoder();
        Configuration configuration =  ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);

        // Act
        PixelBuffer<Rgb48> result = (PixelBuffer<Rgb48>)crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<Rgb48, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }

    [Theory]
    [InlineData("./lena48.png", 259, 259, 200, 200, 0, "4f563af8cda35a4e93c7fadb37ef2486eefaca56f6b4aa7a4659f9ebaffb4d67")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 90, "eeb28e082a7305b0abb188e0ff3d1603dd538202303dec91ca439cd8f41ec033")]
    [InlineData("./lena48.png", 259, 259, 200, 200, 180, "8db9c7eca9defa7d661faef757b65676ce3cd1e22d40133e1b7d2223d6629bee")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 270, "f7c18f3f37f1fb497c594374314aa98e78a25e71c34c106fda9d1cf43badb789")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 55, "08768eb1093a7abe9517a60fa7253ad62a283c1ddb1f735fa4c923850b4fb99f")]
    public void Test_Crop_Rgb48_Planar(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
    {
        // Arrange
        var crop = new Crop();
        var decoder = new Codecs.Png.PngDecoder();
        var converter = new PixelBufferConverter();
        Configuration configuration =  ConfigurationFactory.Build([new Codecs.ImageSharp.PngCodec()]);
        using var inputStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        IPixelBuffer pixelBuffer = decoder.Decode(inputStream, configuration);
        IPixelBuffer convertedPixelBuffer = converter.Execute(new PixelBufferConvertParameters {
            Input = pixelBuffer.AsReadOnly(),
            OutputType = typeof(PlanarPixelBuffer<L16>)
        });

        // Act
        PixelBuffer<L16> result = (PlanarPixelBuffer<L16>)crop.Execute(new CropParameters
        {
            Input = convertedPixelBuffer.AsReadOnly(),
            Rectangle = new(x, y, width, height, rotationDegree)
        });

        // Assert
        string hash = TestHelper.CreateHash(MemoryMarshal.Cast<L16, byte>(result!.Pixels));
        Assert.Equal(expectedHash, hash);
    }
}
