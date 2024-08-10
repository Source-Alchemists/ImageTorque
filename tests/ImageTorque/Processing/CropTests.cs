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
    [InlineData("./lena8.bmp", 300, 300, 200, 200, 0, "c573ee9d07310077be44e9d9bc2993123715e70e57d33bd29b2a32a30bc51ce6")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 90, "6d108681bbaf445f3235184f8819f6c441933ea7608ca3ec6b523dff4b7a0817")]
    [InlineData("./lena8.bmp", 300, 300, 200, 200, 180, "9205cf1472addc6a78533da887edebf320eff29650baa683a4de08f152e2767a")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 270, "f5533d54175c958e3229104d5f816f17b01ebbc1ad251423c2674c49d707d5e3")]
    [InlineData("./lena8.bmp", 150, 150, 200, 200, 55, "a8609b47b229da3692c71db7b61598c5bbce3b341a66f0a6ffe086dfe634e17b")]
    public void Test_CropMono8(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
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
    [InlineData("./lena16.png", 150, 150, 200, 200, 90, "97426baa1542c4ccaabafffaf2bf9f946042edfcefc8af4a1cbf99216b515816")]
    [InlineData("./lena16.png", 259, 259, 200, 200, 180, "fd88bb24ac9db3f528be9e376aa9a269fad2144a4250430a2025aea0e0b36be3")]
    [InlineData("./lena16.png", 150, 150, 200, 200, 270, "ed613b4030459c89d64e6dea7d259c3480eec3330784a6918ded7831d26e2a9a")]
    [InlineData("./lena16.png", 150, 150, 200, 200, 55, "357bb7e192ff8d5cd8bea7f80aa28bfea280ae61012192942bc4037169be58af")]
    public void Test_CropMono16(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
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
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 0, "203c2cce6c92a97d13ef9ea483846c83c8535f0ae105acfd2be303826d29c3e9")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 90, "d93cef7ac393bba872beb39d58da477d5da4999ea7e2a8aee9edea632550b627")]
    [InlineData("./lena24.bmp", 300, 300, 200, 200, 180, "b4024749a6c1f50fadbf9cb18dd7b807cc5dce426e94f9502a50d3392759c142")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 270, "2e3aa235a8ac54f96f93efc2fe636ef18c0ff2d0b73c1efdb70fdec2f9d7dda0")]
    [InlineData("./lena24.bmp", 150, 150, 200, 200, 55, "7e453124b25c674ea608c99ab3d8e167252d205951901ae9a93657d9672dcec9")]
    public void Test_CropRgb24(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
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
    [InlineData("./lena48.png", 150, 150, 200, 200, 90, "698cbd438197e320b1812c3644085289b402aa3e90f37043da4160c2e65153c6")]
    [InlineData("./lena48.png", 259, 259, 200, 200, 180, "1d5d02b01b1cce553d3a32987c60eb4f7cd93e3cd2c58c13c6dcd8e61424db77")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 270, "ca377751f660cb6ea4c5ee6d99d81faa63d24759b6a46ee449b662772111047e")]
    [InlineData("./lena48.png", 150, 150, 200, 200, 55, "22c4ef317bf376dbc577b9331a09077a61b5fba3f48a5370bf6be713839b42af")]
    public void Test_CropRgb48(string imagePath, int x, int y, int width, int height, int rotationDegree, string expectedHash)
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

    [Fact]
    public void Test_Crop_Mono()
    {
        // Arrange
        var crop = new Crop();
        PixelBuffer<LS> pixelBuffer = new(8, 8, [
                                                    0.11f, 0.12f, 0.13f, 0.14f, 0.15f, 0.16f, 0.17f, 0.18f,
                                                    0.21f, 0.22f, 0.23f, 0.24f, 0.25f, 0.26f, 0.27f, 0.28f,
                                                    0.31f, 0.32f, 0.33f, 0.34f, 0.35f, 0.36f, 0.37f, 0.38f,
                                                    0.41f, 0.42f, 0.43f, 0.44f, 0.45f, 0.46f, 0.47f, 0.48f,
                                                    0.51f, 0.52f, 0.53f, 0.54f, 0.55f, 0.56f, 0.57f, 0.58f,
                                                    0.61f, 0.62f, 0.63f, 0.64f, 0.65f, 0.66f, 0.67f, 0.68f,
                                                    0.71f, 0.72f, 0.73f, 0.74f, 0.75f, 0.76f, 0.77f, 0.78f,
                                                    0.81f, 0.82f, 0.83f, 0.84f, 0.85f, 0.86f, 0.87f, 0.88f
                                                ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        Assert.InRange(((PixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal([0.33f, 0.34f, 0.35f, 0.36f,
                      0.43f, 0.44f, 0.45f, 0.46f,
                      0.53f, 0.54f, 0.55f, 0.56f,
                      0.63f, 0.64f, 0.65f, 0.66f], ((PixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb()
    {
        // Arrange
        var crop = new Crop();
        PixelBuffer<Rgb> pixelBuffer = new(8, 8, [
                                                new Rgb(0.11f, 0.12f, 0.13f), new Rgb(0.14f, 0.15f, 0.16f), new Rgb(0.17f, 0.18f, 0.19f), new Rgb(0.20f, 0.21f, 0.22f), new Rgb(0.23f, 0.24f, 0.25f), new Rgb(0.26f, 0.27f, 0.28f), new Rgb(0.29f, 0.30f, 0.31f), new Rgb(0.32f, 0.33f, 0.34f),
                                                new Rgb(0.23f, 0.24f, 0.25f), new Rgb(0.26f, 0.27f, 0.28f), new Rgb(0.29f, 0.30f, 0.31f), new Rgb(0.32f, 0.33f, 0.34f), new Rgb(0.35f, 0.36f, 0.37f), new Rgb(0.38f, 0.39f, 0.40f), new Rgb(0.41f, 0.42f, 0.43f), new Rgb(0.44f, 0.45f, 0.46f),
                                                new Rgb(0.35f, 0.36f, 0.37f), new Rgb(0.38f, 0.39f, 0.40f), new Rgb(0.41f, 0.42f, 0.43f), new Rgb(0.44f, 0.45f, 0.46f), new Rgb(0.47f, 0.48f, 0.49f), new Rgb(0.50f, 0.51f, 0.52f), new Rgb(0.53f, 0.54f, 0.55f), new Rgb(0.56f, 0.57f, 0.58f),
                                                new Rgb(0.47f, 0.48f, 0.49f), new Rgb(0.50f, 0.51f, 0.52f), new Rgb(0.53f, 0.54f, 0.55f), new Rgb(0.56f, 0.57f, 0.58f), new Rgb(0.59f, 0.60f, 0.61f), new Rgb(0.62f, 0.63f, 0.64f), new Rgb(0.65f, 0.66f, 0.67f), new Rgb(0.68f, 0.69f, 0.70f),
                                                new Rgb(0.59f, 0.60f, 0.61f), new Rgb(0.62f, 0.63f, 0.64f), new Rgb(0.65f, 0.66f, 0.67f), new Rgb(0.68f, 0.69f, 0.70f), new Rgb(0.71f, 0.72f, 0.73f), new Rgb(0.74f, 0.75f, 0.76f), new Rgb(0.77f, 0.78f, 0.79f), new Rgb(0.80f, 0.81f, 0.82f),
                                                new Rgb(0.71f, 0.72f, 0.73f), new Rgb(0.74f, 0.75f, 0.76f), new Rgb(0.77f, 0.78f, 0.79f), new Rgb(0.80f, 0.81f, 0.82f), new Rgb(0.83f, 0.84f, 0.85f), new Rgb(0.86f, 0.87f, 0.88f), new Rgb(0.89f, 0.90f, 0.91f), new Rgb(0.92f, 0.93f, 0.94f),
                                                new Rgb(0.83f, 0.84f, 0.85f), new Rgb(0.86f, 0.87f, 0.88f), new Rgb(0.89f, 0.90f, 0.91f), new Rgb(0.92f, 0.93f, 0.94f), new Rgb(0.95f, 0.96f, 0.97f), new Rgb(0.98f, 0.99f, 1.00f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f),
                                                new Rgb(0.95f, 0.96f, 0.97f), new Rgb(0.98f, 0.99f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f), new Rgb(1f, 1f, 1f)
                                            ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(result);
        Assert.InRange(((PixelBuffer<Rgb>)result).Pixels[0].Red, 0f, 1f);
        Assert.Equal([
            new Rgb(0.41f, 0.42f, 0.43f), new Rgb(0.44f, 0.45f, 0.46f), new Rgb(0.47f, 0.48f, 0.49f), new Rgb(0.50f, 0.51f, 0.52f),
            new Rgb(0.53f, 0.54f, 0.55f), new Rgb(0.56f, 0.57f, 0.58f), new Rgb(0.59f, 0.60f, 0.61f), new Rgb(0.62f, 0.63f, 0.64f),
            new Rgb(0.65f, 0.66f, 0.67f), new Rgb(0.68f, 0.69f, 0.70f), new Rgb(0.71f, 0.72f, 0.73f), new Rgb(0.74f, 0.75f, 0.76f),
            new Rgb(0.77f, 0.78f, 0.79f), new Rgb(0.80f, 0.81f, 0.82f), new Rgb(0.83f, 0.84f, 0.85f), new Rgb(0.86f, 0.87f, 0.88f)
        ], ((PixelBuffer<Rgb>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_RgbFFF()
    {
        // Arrange
        var crop = new Crop();
        PlanarPixelBuffer<LS> pixelBuffer = new(8, 8, [
                                                        // R
                                                        new LS(0.11f), new LS(0.12f), new LS(0.13f), new LS(0.14f), new LS(0.15f), new LS(0.16f), new LS(0.17f), new LS(0.18f),
                                                        new LS(0.21f), new LS(0.22f), new LS(0.23f), new LS(0.24f), new LS(0.25f), new LS(0.26f), new LS(0.27f), new LS(0.28f),
                                                        new LS(0.31f), new LS(0.32f), new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f), new LS(0.37f), new LS(0.38f),
                                                        new LS(0.41f), new LS(0.42f), new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f), new LS(0.47f), new LS(0.48f),
                                                        new LS(0.51f), new LS(0.52f), new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f), new LS(0.57f), new LS(0.58f),
                                                        new LS(0.61f), new LS(0.62f), new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f), new LS(0.67f), new LS(0.68f),
                                                        new LS(0.71f), new LS(0.72f), new LS(0.73f), new LS(0.74f), new LS(0.75f), new LS(0.76f), new LS(0.77f), new LS(0.78f),
                                                        new LS(0.81f), new LS(0.82f), new LS(0.83f), new LS(0.84f), new LS(0.85f), new LS(0.86f), new LS(0.87f), new LS(0.88f),
                                                        // G
                                                        new LS(0.11f), new LS(0.12f), new LS(0.13f), new LS(0.14f), new LS(0.15f), new LS(0.16f), new LS(0.17f), new LS(0.18f),
                                                        new LS(0.21f), new LS(0.22f), new LS(0.23f), new LS(0.24f), new LS(0.25f), new LS(0.26f), new LS(0.27f), new LS(0.28f),
                                                        new LS(0.31f), new LS(0.32f), new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f), new LS(0.37f), new LS(0.38f),
                                                        new LS(0.41f), new LS(0.42f), new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f), new LS(0.47f), new LS(0.48f),
                                                        new LS(0.51f), new LS(0.52f), new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f), new LS(0.57f), new LS(0.58f),
                                                        new LS(0.61f), new LS(0.62f), new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f), new LS(0.67f), new LS(0.68f),
                                                        new LS(0.71f), new LS(0.72f), new LS(0.73f), new LS(0.74f), new LS(0.75f), new LS(0.76f), new LS(0.77f), new LS(0.78f),
                                                        new LS(0.81f), new LS(0.82f), new LS(0.83f), new LS(0.84f), new LS(0.85f), new LS(0.86f), new LS(0.87f), new LS(0.88f),
                                                        // B
                                                        new LS(0.11f), new LS(0.12f), new LS(0.13f), new LS(0.14f), new LS(0.15f), new LS(0.16f), new LS(0.17f), new LS(0.18f),
                                                        new LS(0.21f), new LS(0.22f), new LS(0.23f), new LS(0.24f), new LS(0.25f), new LS(0.26f), new LS(0.27f), new LS(0.28f),
                                                        new LS(0.31f), new LS(0.32f), new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f), new LS(0.37f), new LS(0.38f),
                                                        new LS(0.41f), new LS(0.42f), new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f), new LS(0.47f), new LS(0.48f),
                                                        new LS(0.51f), new LS(0.52f), new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f), new LS(0.57f), new LS(0.58f),
                                                        new LS(0.61f), new LS(0.62f), new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f), new LS(0.67f), new LS(0.68f),
                                                        new LS(0.71f), new LS(0.72f), new LS(0.73f), new LS(0.74f), new LS(0.75f), new LS(0.76f), new LS(0.77f), new LS(0.78f),
                                                        new LS(0.81f), new LS(0.82f), new LS(0.83f), new LS(0.84f), new LS(0.85f), new LS(0.86f), new LS(0.87f), new LS(0.88f)
                                                    ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LS>>(result);
        Assert.InRange(((PlanarPixelBuffer<LS>)result).Pixels[0].Value, 0f, 1f);
        Assert.Equal([
            new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f),
            new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f),
            new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f),
            new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f),
            new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f),
            new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f),
            new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f),
            new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f),
            new LS(0.33f), new LS(0.34f), new LS(0.35f), new LS(0.36f),
            new LS(0.43f), new LS(0.44f), new LS(0.45f), new LS(0.46f),
            new LS(0.53f), new LS(0.54f), new LS(0.55f), new LS(0.56f),
            new LS(0.63f), new LS(0.64f), new LS(0.65f), new LS(0.66f)
        ], ((PlanarPixelBuffer<LS>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb888()
    {
        // Arrange
        var crop = new Crop();
        PlanarPixelBuffer<L8> pixelBuffer = new(8, 8, [
                                                    // R
                                                    new L8(11), new L8(12), new L8(13), new L8(14), new L8(15), new L8(16), new L8(17), new L8(18),
                                                    new L8(21), new L8(22), new L8(23), new L8(24), new L8(25), new L8(26), new L8(27), new L8(28),
                                                    new L8(31), new L8(32), new L8(33), new L8(34), new L8(35), new L8(36), new L8(37), new L8(38),
                                                    new L8(41), new L8(42), new L8(43), new L8(44), new L8(45), new L8(46), new L8(47), new L8(48),
                                                    new L8(51), new L8(52), new L8(53), new L8(54), new L8(55), new L8(56), new L8(57), new L8(58),
                                                    new L8(61), new L8(62), new L8(63), new L8(64), new L8(65), new L8(66), new L8(67), new L8(68),
                                                    new L8(71), new L8(72), new L8(73), new L8(74), new L8(75), new L8(76), new L8(77), new L8(78),
                                                    new L8(81), new L8(82), new L8(83), new L8(84), new L8(85), new L8(86), new L8(87), new L8(88),
                                                    // G
                                                    new L8(11), new L8(12), new L8(13), new L8(14), new L8(15), new L8(16), new L8(17), new L8(18),
                                                    new L8(21), new L8(22), new L8(23), new L8(24), new L8(25), new L8(26), new L8(27), new L8(28),
                                                    new L8(31), new L8(32), new L8(33), new L8(34), new L8(35), new L8(36), new L8(37), new L8(38),
                                                    new L8(41), new L8(42), new L8(43), new L8(44), new L8(45), new L8(46), new L8(47), new L8(48),
                                                    new L8(51), new L8(52), new L8(53), new L8(54), new L8(55), new L8(56), new L8(57), new L8(58),
                                                    new L8(61), new L8(62), new L8(63), new L8(64), new L8(65), new L8(66), new L8(67), new L8(68),
                                                    new L8(71), new L8(72), new L8(73), new L8(74), new L8(75), new L8(76), new L8(77), new L8(78),
                                                    new L8(81), new L8(82), new L8(83), new L8(84), new L8(85), new L8(86), new L8(87), new L8(88),
                                                    // B
                                                    new L8(11), new L8(12), new L8(13), new L8(14), new L8(15), new L8(16), new L8(17), new L8(18),
                                                    new L8(21), new L8(22), new L8(23), new L8(24), new L8(25), new L8(26), new L8(27), new L8(28),
                                                    new L8(31), new L8(32), new L8(33), new L8(34), new L8(35), new L8(36), new L8(37), new L8(38),
                                                    new L8(41), new L8(42), new L8(43), new L8(44), new L8(45), new L8(46), new L8(47), new L8(48),
                                                    new L8(51), new L8(52), new L8(53), new L8(54), new L8(55), new L8(56), new L8(57), new L8(58),
                                                    new L8(61), new L8(62), new L8(63), new L8(64), new L8(65), new L8(66), new L8(67), new L8(68),
                                                    new L8(71), new L8(72), new L8(73), new L8(74), new L8(75), new L8(76), new L8(77), new L8(78),
                                                    new L8(81), new L8(82), new L8(83), new L8(84), new L8(85), new L8(86), new L8(87), new L8(88)
                                                ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L8>>(result);
        Assert.InRange(((PlanarPixelBuffer<L8>)result).Pixels[0].Value, byte.MinValue, byte.MaxValue);
        Assert.Equal([
            new L8(33), new L8(34), new L8(35), new L8(36),
            new L8(43), new L8(44), new L8(45), new L8(46),
            new L8(53), new L8(54), new L8(55), new L8(56),
            new L8(63), new L8(64), new L8(65), new L8(66),
            new L8(33), new L8(34), new L8(35), new L8(36),
            new L8(43), new L8(44), new L8(45), new L8(46),
            new L8(53), new L8(54), new L8(55), new L8(56),
            new L8(63), new L8(64), new L8(65), new L8(66),
            new L8(33), new L8(34), new L8(35), new L8(36),
            new L8(43), new L8(44), new L8(45), new L8(46),
            new L8(53), new L8(54), new L8(55), new L8(56),
            new L8(63), new L8(64), new L8(65), new L8(66)
        ], ((PixelBuffer<L8>)result).Pixels.ToArray());
    }

    [Fact]
    public void Test_Crop_Rgb161616()
    {
        // Arrange
        var crop = new Crop();
        PlanarPixelBuffer<L16> pixelBuffer = new(8, 8, [
                                                        // R
                                                        new L16(1100), new L16(1200), new L16(1300), new L16(1400), new L16(1500), new L16(1600), new L16(1700), new L16(1800),
                                                        new L16(2100), new L16(2200), new L16(2300), new L16(2400), new L16(2500), new L16(2600), new L16(2700), new L16(2800),
                                                        new L16(3100), new L16(3200), new L16(3300), new L16(3400), new L16(3500), new L16(3600), new L16(3700), new L16(3800),
                                                        new L16(4100), new L16(4200), new L16(4300), new L16(4400), new L16(4500), new L16(4600), new L16(4700), new L16(4800),
                                                        new L16(5100), new L16(5200), new L16(5300), new L16(5400), new L16(5500), new L16(5600), new L16(5700), new L16(5800),
                                                        new L16(6100), new L16(6200), new L16(6300), new L16(6400), new L16(6500), new L16(6600), new L16(6700), new L16(6800),
                                                        new L16(7100), new L16(7200), new L16(7300), new L16(7400), new L16(7500), new L16(7600), new L16(7700), new L16(7800),
                                                        new L16(8100), new L16(8200), new L16(8300), new L16(8400), new L16(8500), new L16(8600), new L16(8700), new L16(8800),
                                                        // G
                                                        new L16(1100), new L16(1200), new L16(1300), new L16(1400), new L16(1500), new L16(1600), new L16(1700), new L16(1800),
                                                        new L16(2100), new L16(2200), new L16(2300), new L16(2400), new L16(2500), new L16(2600), new L16(2700), new L16(2800),
                                                        new L16(3100), new L16(3200), new L16(3300), new L16(3400), new L16(3500), new L16(3600), new L16(3700), new L16(3800),
                                                        new L16(4100), new L16(4200), new L16(4300), new L16(4400), new L16(4500), new L16(4600), new L16(4700), new L16(4800),
                                                        new L16(5100), new L16(5200), new L16(5300), new L16(5400), new L16(5500), new L16(5600), new L16(5700), new L16(5800),
                                                        new L16(6100), new L16(6200), new L16(6300), new L16(6400), new L16(6500), new L16(6600), new L16(6700), new L16(6800),
                                                        new L16(7100), new L16(7200), new L16(7300), new L16(7400), new L16(7500), new L16(7600), new L16(7700), new L16(7800),
                                                        new L16(8100), new L16(8200), new L16(8300), new L16(8400), new L16(8500), new L16(8600), new L16(8700), new L16(8800),
                                                        // B
                                                        new L16(1100), new L16(1200), new L16(1300), new L16(1400), new L16(1500), new L16(1600), new L16(1700), new L16(1800),
                                                        new L16(2100), new L16(2200), new L16(2300), new L16(2400), new L16(2500), new L16(2600), new L16(2700), new L16(2800),
                                                        new L16(3100), new L16(3200), new L16(3300), new L16(3400), new L16(3500), new L16(3600), new L16(3700), new L16(3800),
                                                        new L16(4100), new L16(4200), new L16(4300), new L16(4400), new L16(4500), new L16(4600), new L16(4700), new L16(4800),
                                                        new L16(5100), new L16(5200), new L16(5300), new L16(5400), new L16(5500), new L16(5600), new L16(5700), new L16(5800),
                                                        new L16(6100), new L16(6200), new L16(6300), new L16(6400), new L16(6500), new L16(6600), new L16(6700), new L16(6800),
                                                        new L16(7100), new L16(7200), new L16(7300), new L16(7400), new L16(7500), new L16(7600), new L16(7700), new L16(7800),
                                                        new L16(8100), new L16(8200), new L16(8300), new L16(8400), new L16(8500), new L16(8600), new L16(8700), new L16(8800)
                                                    ]);

        // Act
        IPixelBuffer result = crop.Execute(new CropParameters
        {
            Input = pixelBuffer.AsReadOnly(),
            Rectangle = new(4, 4, 4, 4)
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<L16>>(result);
        Assert.InRange(((PlanarPixelBuffer<L16>)result).Pixels[0].Value, ushort.MinValue, ushort.MaxValue);
        Assert.Equal([
            new L16(3300), new L16(3400), new L16(3500), new L16(3600),
            new L16(4300), new L16(4400), new L16(4500), new L16(4600),
            new L16(5300), new L16(5400), new L16(5500), new L16(5600),
            new L16(6300), new L16(6400), new L16(6500), new L16(6600),
            new L16(3300), new L16(3400), new L16(3500), new L16(3600),
            new L16(4300), new L16(4400), new L16(4500), new L16(4600),
            new L16(5300), new L16(5400), new L16(5500), new L16(5600),
            new L16(6300), new L16(6400), new L16(6500), new L16(6600),
            new L16(3300), new L16(3400), new L16(3500), new L16(3600),
            new L16(4300), new L16(4400), new L16(4500), new L16(4600),
            new L16(5300), new L16(5400), new L16(5500), new L16(5600),
            new L16(6300), new L16(6400), new L16(6500), new L16(6600)
        ], ((PlanarPixelBuffer<L16>)result).Pixels.ToArray());
    }
}
