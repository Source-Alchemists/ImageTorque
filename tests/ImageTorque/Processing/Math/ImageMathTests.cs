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

public class ImageMathTests
{
    private readonly ImageMath _imageMath = new();

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_LS(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<LS> pixelBufferA = new(2, 2, new LS[] { 0f, 0.4f, 0.5f, 0.6f });
        PixelBuffer<LS> pixelBufferB = new(2, 2, new LS[] { 0.5f, 0.3f, 0.6f, 0f });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters
        {
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<LS>>(result);
        switch (mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new LS[] { 0.5f, 0.70000005f, 1f, 0.6f }, ((PixelBuffer<LS>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new LS[] { 0f, 0.099999994f, 0f, 0.6f }, ((PixelBuffer<LS>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new LS[] { 0f, 0.120000005f, 0.3f, 0f }, ((PixelBuffer<LS>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_L8(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<L8> pixelBufferA = new(2, 2, new L8[] { 0, 102, 128, 153 });
        PixelBuffer<L8> pixelBufferB = new(2, 2, new L8[] { 128, 77, 153, 0 });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters
        {
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<L8>>(result);
        switch (mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new L8[] { 128, 179, byte.MaxValue, 153 }, ((PixelBuffer<L8>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new L8[] { 0, 25, 0, 153 }, ((PixelBuffer<L8>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new L8[] { 0, byte.MaxValue, byte.MaxValue, 0 }, ((PixelBuffer<L8>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_L16(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<L16> pixelBufferA = new(2, 2, new L16[] { 0, 10200, 12800, 15300 });
        PixelBuffer<L16> pixelBufferB = new(2, 2, new L16[] { 12800, 7700, 15300, 0 });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters
        {
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<L16>>(result);
        switch (mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new L16[] { 12800, 17900, 28100, 15300 }, ((PixelBuffer<L16>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new L16[] { 0, 2500, 0, 15300 }, ((PixelBuffer<L16>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new L16[] { 0, ushort.MaxValue, ushort.MaxValue, 0 }, ((PixelBuffer<L16>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_Rgb(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<Rgb> pixelBufferA = new(2, 2, new Rgb[] {
                                                            new Rgb(0f, 0f, 0f),
                                                            new Rgb(0.1f, 0.2f, 0.4f),
                                                            new Rgb(0.3f, 0.5f, 0.7f),
                                                            new Rgb(0.4f, 0.6f, 0.8f) });
        PixelBuffer<Rgb> pixelBufferB = new(2, 2, new Rgb[] {
                                                            new Rgb(0.4f, 0.2f, 0.1f),
                                                            new Rgb(0.8f, 0.6f, 0.4f),
                                                            new Rgb(0.7f, 0.5f, 0.3f),
                                                            new Rgb(0f, 0f, 0f) });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters
        {
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb>>(result);
        switch (mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new Rgb[] {
                                        new Rgb(0.4f, 0.2f, 0.1f),
                                        new Rgb(0.90000004f, 0.8f, 0.8f),
                                        new Rgb(1f, 1f, 1f),
                                        new Rgb(0.4f, 0.6f, 0.8f)
                }, ((PixelBuffer<Rgb>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new Rgb[] {
                                        new Rgb(0f, 0f, 0f),
                                        new Rgb(0f, 0f, 0f),
                                        new Rgb(0f, 0f, 0.39999998f),
                                        new Rgb(0.4f, 0.6f, 0.8f)
                 }, ((PixelBuffer<Rgb>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new Rgb[] {
                                        new Rgb(0f, 0f, 0f),
                                        new Rgb(0.080000006f, 0.120000005f, 0.16000001f),
                                        new Rgb(0.21000001f, 0.25f, 0.21000001f),
                                        new Rgb(0f, 0f, 0f)
                 }, ((PixelBuffer<Rgb>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_Rgb24(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<Rgb24> pixelBufferA = new(2, 2, new Rgb24[] {
                                                            new Rgb24(0, 128, 53),
                                                            new Rgb24(53, 187, 42),
                                                            new Rgb24(128, 28, 0),
                                                            new Rgb24(0, 0, 0) });
        PixelBuffer<Rgb24> pixelBufferB = new(2, 2, new Rgb24[] {
                                                            new Rgb24(0, 0, 0),
                                                            new Rgb24(128, 150, 87),
                                                            new Rgb24(42, 240, 21),
                                                            new Rgb24(255, 128, 255) });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters
        {
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb24>>(result);
        switch (mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new Rgb24[] {
                                        new Rgb24(0, 128, 53),
                                        new Rgb24(181, byte.MaxValue, 129),
                                        new Rgb24(170, byte.MaxValue, 21),
                                        new Rgb24(byte.MaxValue, 128, byte.MaxValue)
                }, ((PixelBuffer<Rgb24>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new Rgb24[] {
                                        new Rgb24(0, 128, 53),
                                        new Rgb24(0, 37, 0),
                                        new Rgb24(86, 0, 0),
                                        new Rgb24(0, 0, 0)
                 }, ((PixelBuffer<Rgb24>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new Rgb24[] {
                                        new Rgb24(0, 0, 0),
                                        new Rgb24(byte.MaxValue, byte.MaxValue, byte.MaxValue),
                                        new Rgb24(byte.MaxValue, byte.MaxValue, 0),
                                        new Rgb24(0, 0, 0)
                 }, ((PixelBuffer<Rgb24>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Packed_Rgb48(ImageMathMode mode)
    {
        // Arrange
        PixelBuffer<Rgb48> pixelBufferA = new(2, 2, new Rgb48[] {
                                                            new Rgb48(0, 12800, 5300),
                                                            new Rgb48(5300, 18700, 4200),
                                                            new Rgb48(12800, 2800, 0),
                                                            new Rgb48(0, 0, 0) });
        PixelBuffer<Rgb48> pixelBufferB = new(2, 2, new Rgb48[] {
                                                            new Rgb48(0, 0, 0),
                                                            new Rgb48(12800, 15000, 8700),
                                                            new Rgb48(4200, 24000, 2100),
                                                            new Rgb48(25500, 12800, 25500) });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters
        {
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PixelBuffer<Rgb48>>(result);
        switch (mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new Rgb48[] {
                                        new Rgb48(0, 12800, 5300),
                                        new Rgb48(18100, 33700, 12900),
                                        new Rgb48(17000, 26800, 2100),
                                        new Rgb48(25500, 12800, 25500)
                }, ((PixelBuffer<Rgb48>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new Rgb48[] {
                                        new Rgb48(0, 12800, 5300),
                                        new Rgb48(0, 3700, 0),
                                        new Rgb48(8600, 0, 0),
                                        new Rgb48(0, 0, 0)
                 }, ((PixelBuffer<Rgb48>)result).Pixels[..4].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new Rgb48[] {
                                        new Rgb48(0, 0, 0),
                                        new Rgb48(ushort.MaxValue, ushort.MaxValue, ushort.MaxValue),
                                        new Rgb48(ushort.MaxValue, ushort.MaxValue, 0),
                                        new Rgb48(0, 0, 0)
                 }, ((PixelBuffer<Rgb48>)result).Pixels[..4].ToArray());
                break;
        }
    }

    [Theory]
    [InlineData(ImageMathMode.Add)]
    [InlineData(ImageMathMode.Subtract)]
    [InlineData(ImageMathMode.Multiply)]
    public void Test_Planar_Rgb(ImageMathMode mode)
    {
        // Arrange
        PlanarPixelBuffer<LS> pixelBufferA = new(2, 2, new LS[] {
                                                            new LS(0f), new LS(0.1f), new LS(0.3f), new LS(0.4f),
                                                            new LS(0f), new LS(0.2f), new LS(0.5f), new LS(0.6f),
                                                            new LS(0f), new LS(0.4f), new LS(0.7f), new LS(0.8f) });
        PlanarPixelBuffer<LS> pixelBufferB = new(2, 2, new LS[] {
                                                            new LS(0.4f), new LS(0.8f), new LS(0.7f), new LS(0f),
                                                            new LS(0.2f), new LS(0.6f), new LS(0.5f), new LS(0f),
                                                            new LS(0.1f), new LS(0.4f), new LS(0.3f), new LS(0f) });

        // Act
        IPixelBuffer result = _imageMath.Execute(new ImageMathParameters
        {
            InputA = pixelBufferA.AsReadOnly(),
            InputB = pixelBufferB.AsReadOnly(),
            ImageMathMode = mode
        });

        // Assert
        Assert.IsType<PlanarPixelBuffer<LS>>(result);
        switch (mode)
        {
            case ImageMathMode.Add:
                Assert.Equal(new LS[] {
                                        new LS(0.4f), new LS(0.90000004f), new LS(1f), new LS(0.4f),
                                        new LS(0.2f), new LS(0.8f), new LS(1f), new LS(0.6f),
                                        new LS(0.1f), new LS(0.8f), new LS(1f), new LS(0.8f)
                }, ((PixelBuffer<LS>)result).Pixels[..12].ToArray());
                break;
            case ImageMathMode.Subtract:
                Assert.Equal(new LS[] {
                                        new LS(0f), new LS(0f), new LS(0f), new LS(0.4f),
                                        new LS(0f), new LS(0f), new LS(0f), new LS(0.6f),
                                        new LS(0f), new LS(0f), new LS(0.39999998f), new LS(0.8f)
                 }, ((PixelBuffer<LS>)result).Pixels[..12].ToArray());
                break;
            case ImageMathMode.Multiply:
                Assert.Equal(new LS[] {
                                        new LS(0f), new LS(0.080000006f), new LS(0.21000001f), new LS(0f),
                                        new LS(0f), new LS(0.120000005f), new LS(0.25f), new LS(0f),
                                        new LS(0f), new LS( 0.16000001f), new LS(0.21000001f), new LS(0f)
                 }, ((PixelBuffer<LS>)result).Pixels[..12].ToArray());
                break;
        }
    }
}
