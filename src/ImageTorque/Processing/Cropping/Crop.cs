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

using System.Runtime.CompilerServices;

using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal sealed class Crop : IProcessor<CropParameters, IPixelBuffer>
{
    private const double D270 = 3d * Math.PI / 2d;
    private const double D90 = Math.PI / 2d;

    public IPixelBuffer Execute(CropParameters parameters)
    {
        Type inputType = parameters.Input!.GetType();

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<LS>))
        {
            return CropMono(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<L8>))
        {
            return CropMono8(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<L16>))
        {
            return CropMono16(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            return CropRgb(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return CropRgb24(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return CropRgb48(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<LS>))
        {
            return CropRgbFFF(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<L8>))
        {
            return CropRgb888(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<L16>))
        {
            return CropRgb161616(parameters);
        }

        throw new NotSupportedException("The input type is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<LS> CropMono(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<LS>)parameters.Input!;
        var targetBuffer = new PixelBuffer<LS>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropSingle(sourceBuffer.Pixels.AsSingle(), targetBuffer.Pixels.AsSingle(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<L8> CropMono8(CropParameters parameters)
    {
        var rectangle = new CropRectangle(parameters.Rectangle);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<L8>)parameters.Input!;
        var targetBuffer = new PixelBuffer<L8>(rectangle.Width, rectangle.Height);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, targetBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropByte(sourceBuffer.Pixels.AsByte(), targetBuffer.Pixels.AsByte(), sourceBuffer.Width, sourceBuffer.Height, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<L16> CropMono16(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<L16>)parameters.Input!;
        var targetBuffer = new PixelBuffer<L16>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, targetBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropUInt16(sourceBuffer.Pixels.AsUInt16(), targetBuffer.Pixels.AsUInt16(), sourceBuffer.Width, sourceBuffer.Height, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<Rgb> CropRgb(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input!;
        var targetBuffer = new PixelBuffer<Rgb>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, targetBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropRgb(sourceBuffer.Pixels, targetBuffer.Pixels, sourceBuffer.Width, sourceBuffer.Height, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void CropRgb(ReadOnlySpan<Rgb> src, Span<Rgb> dest, int imageWidth, int imageHeight, int yDest, CropRectangle rectangle)
    {
        float yc = yDest - rectangle.HalfHeight;
        for (int xDest = 0; xDest < rectangle.Width; xDest++)
        {
            float xc = xDest - rectangle.HalfWidth;
            float xr = xc * rectangle.Cos - yc * rectangle.Sin;
            float yr = xc * rectangle.Sin + yc * rectangle.Cos;
            float x = xr + rectangle.Top;
            float y = yr + rectangle.Left;

            int x1 = (int)x;
            int y1 = (int)y;

            int indexDest = (yDest * rectangle.Width) + xDest;
            if (x1 >= 0 && y1 >= 0)
            {
                int x2 = x1 + 1;
                int y2 = y1 + 1;

                int y1ImageWidth = y1 * imageWidth;
                int y2ImageWidth = y1ImageWidth + imageWidth;

                // See https://en.wikipedia.org/wiki/Bilinear_interpolation
                if (x2 < imageWidth && y2 < imageHeight)
                {
                    Rgb q11 = src[y1ImageWidth + x1];
                    Rgb q21 = src[y1ImageWidth + x2];
                    Rgb q12 = src[y2ImageWidth + x1];
                    Rgb q22 = src[y2ImageWidth + x2];

                    float fx1R = Interpolate(x, x1, x2, q11.Red, q21.Red);
                    float fx1G = Interpolate(x, x1, x2, q11.Green, q21.Green);
                    float fx1B = Interpolate(x, x1, x2, q11.Blue, q21.Blue);
                    float fx2R = Interpolate(x, x1, x2, q12.Red, q22.Red);
                    float fx2G = Interpolate(x, x1, x2, q12.Green, q22.Green);
                    float fx2B = Interpolate(x, x1, x2, q12.Blue, q22.Blue);
                    float fxyR = Interpolate(y, y1, y2, fx1R, fx2R);
                    float fxyG = Interpolate(y, y1, y2, fx1G, fx2G);
                    float fxyB = Interpolate(y, y1, y2, fx1B, fx2B);

                    dest[indexDest] = new Rgb(fxyR, fxyG, fxyB);
                }
                else if (x2 < imageWidth && y1 < imageHeight)
                {
                    Rgb q11 = src[y1ImageWidth + x1];
                    Rgb q21 = src[y1ImageWidth + x2];

                    float fxR = Interpolate(x, x1, x2, q11.Red, q21.Red);
                    float fxG = Interpolate(x, x1, x2, q11.Green, q21.Green);
                    float fxB = Interpolate(x, x1, x2, q11.Blue, q21.Blue);

                    dest[indexDest] = new Rgb(fxR, fxG, fxB);
                }
                else if (x1 < imageWidth && y2 < imageHeight)
                {
                    Rgb q11 = src[y1ImageWidth + x1];
                    Rgb q12 = src[y2ImageWidth + x1];

                    float fyR = Interpolate(y, y1, y2, q11.Red, q12.Red);
                    float fyG = Interpolate(y, y1, y2, q11.Green, q12.Green);
                    float fyB = Interpolate(y, y1, y2, q11.Blue, q12.Blue);

                    dest[indexDest] = new Rgb(fyR, fyG, fyB);
                }
                else if (x1 < imageWidth && y1 < imageHeight)
                {
                    dest[indexDest] = src[y1ImageWidth + x1];
                }
                else
                {
                    dest[indexDest] = default;
                }
            }
            else
            {
                dest[indexDest] = default;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<Rgb24> CropRgb24(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input!;
        var targetBuffer = new PixelBuffer<Rgb24>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, targetBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropRgb24(sourceBuffer.Pixels, targetBuffer.Pixels, sourceBuffer.Width, sourceBuffer.Height, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void CropRgb24(ReadOnlySpan<Rgb24> src, Span<Rgb24> dest, int imageWidth, int imageHeight, int yDest, CropRectangle rectangle)
    {
        float yc = yDest - rectangle.HalfHeight;
        for (int xDest = 0; xDest < rectangle.Width; xDest++)
        {
            float xc = xDest - rectangle.HalfWidth;
            float xr = xc * rectangle.Cos - yc * rectangle.Sin;
            float yr = xc * rectangle.Sin + yc * rectangle.Cos;
            float x = xr + rectangle.Left;
            float y = yr + rectangle.Top;

            int x1 = (int)x;
            int y1 = (int)y;

            int indexDest = (yDest * rectangle.Width) + xDest;
            if (x1 >= 0 && y1 >= 0)
            {
                int x2 = x1 + 1;
                int y2 = y1 + 1;

                int y1ImageWidth = y1 * imageWidth;
                int y2ImageWidth = y1ImageWidth + imageWidth;

                // See https://en.wikipedia.org/wiki/Bilinear_interpolation
                if (x2 < imageWidth && y2 < imageHeight)
                {
                    Rgb24 q11 = src[y1ImageWidth + x1];
                    Rgb24 q21 = src[y1ImageWidth + x2];
                    Rgb24 q12 = src[y2ImageWidth + x1];
                    Rgb24 q22 = src[y2ImageWidth + x2];

                    float fx1R = Interpolate(x, x1, x2, q11.Red, q21.Red);
                    float fx1G = Interpolate(x, x1, x2, q11.Green, q21.Green);
                    float fx1B = Interpolate(x, x1, x2, q11.Blue, q21.Blue);
                    float fx2R = Interpolate(x, x1, x2, q12.Red, q22.Red);
                    float fx2G = Interpolate(x, x1, x2, q12.Green, q22.Green);
                    float fx2B = Interpolate(x, x1, x2, q12.Blue, q22.Blue);
                    float fxyR = Interpolate(y, y1, y2, fx1R, fx2R);
                    float fxyG = Interpolate(y, y1, y2, fx1G, fx2G);
                    float fxyB = Interpolate(y, y1, y2, fx1B, fx2B);

                    dest[indexDest] = new Rgb24((byte)Math.Round(fxyR), (byte)Math.Round(fxyG), (byte)Math.Round(fxyB));
                }
                else if (x2 < imageWidth && y1 < imageHeight)
                {
                    Rgb24 q11 = src[y1ImageWidth + x1];
                    Rgb24 q21 = src[y1ImageWidth + x2];

                    float fxR = Interpolate(x, x1, x2, q11.Red, q21.Red);
                    float fxG = Interpolate(x, x1, x2, q11.Green, q21.Green);
                    float fxB = Interpolate(x, x1, x2, q11.Blue, q21.Blue);

                    dest[indexDest] = new Rgb24((byte)Math.Round(fxR), (byte)Math.Round(fxG), (byte)Math.Round(fxB));
                }
                else if (x1 < imageWidth && y2 < imageHeight)
                {
                    Rgb24 q11 = src[y1ImageWidth + x1];
                    Rgb24 q12 = src[y2ImageWidth + x1];

                    float fyR = Interpolate(y, y1, y2, q11.Red, q12.Red);
                    float fyG = Interpolate(y, y1, y2, q11.Green, q12.Green);
                    float fyB = Interpolate(y, y1, y2, q11.Blue, q12.Blue);

                    dest[indexDest] = new Rgb24((byte)Math.Round(fyR), (byte)Math.Round(fyG), (byte)Math.Round(fyB));
                }
                else if (x1 < imageWidth && y1 < imageHeight)
                {
                    dest[indexDest] = src[y1ImageWidth + x1];
                }
                else
                {
                    dest[indexDest] = default;
                }
            }
            else
            {
                dest[indexDest] = default;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<Rgb48> CropRgb48(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input!;
        var targetBuffer = new PixelBuffer<Rgb48>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, targetBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropRgb48(sourceBuffer.Pixels, targetBuffer.Pixels, sourceBuffer.Width, sourceBuffer.Height, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void CropRgb48(ReadOnlySpan<Rgb48> src, Span<Rgb48> dest, int imageWidth, int imageHeight, int yDest, CropRectangle rectangle)
    {
        float yc = yDest - rectangle.HalfHeight;
        for (int xDest = 0; xDest < rectangle.Width; xDest++)
        {
            float xc = xDest - rectangle.HalfWidth;
            float xr = xc * rectangle.Cos - yc * rectangle.Sin;
            float yr = xc * rectangle.Sin + yc * rectangle.Cos;
            float x = xr + rectangle.Left;
            float y = yr + rectangle.Top;

            int x1 = (int)x;
            int y1 = (int)y;

            int indexDest = (yDest * rectangle.Width) + xDest;
            if (x1 >= 0 && y1 >= 0)
            {
                int x2 = x1 + 1;
                int y2 = y1 + 1;

                int y1ImageWidth = y1 * imageWidth;
                int y2ImageWidth = y1ImageWidth + imageWidth;

                // See https://en.wikipedia.org/wiki/Bilinear_interpolation
                if (x2 < imageWidth && y2 < imageHeight)
                {
                    Rgb48 q11 = src[y1ImageWidth + x1];
                    Rgb48 q21 = src[y1ImageWidth + x2];
                    Rgb48 q12 = src[y2ImageWidth + x1];
                    Rgb48 q22 = src[y2ImageWidth + x2];

                    float fx1R = Interpolate(x, x1, x2, q11.Red, q21.Red);
                    float fx1G = Interpolate(x, x1, x2, q11.Green, q21.Green);
                    float fx1B = Interpolate(x, x1, x2, q11.Blue, q21.Blue);
                    float fx2R = Interpolate(x, x1, x2, q12.Red, q22.Red);
                    float fx2G = Interpolate(x, x1, x2, q12.Green, q22.Green);
                    float fx2B = Interpolate(x, x1, x2, q12.Blue, q22.Blue);
                    float fxyR = Interpolate(y, y1, y2, fx1R, fx2R);
                    float fxyG = Interpolate(y, y1, y2, fx1G, fx2G);
                    float fxyB = Interpolate(y, y1, y2, fx1B, fx2B);

                    dest[indexDest] = new Rgb48((ushort)Math.Round(fxyR), (ushort)Math.Round(fxyG), (ushort)Math.Round(fxyB));
                }
                else if (x2 < imageWidth && y1 < imageHeight)
                {
                    Rgb48 q11 = src[y1ImageWidth + x1];
                    Rgb48 q21 = src[y1ImageWidth + x2];

                    float fxR = Interpolate(x, x1, x2, q11.Red, q21.Red);
                    float fxG = Interpolate(x, x1, x2, q11.Green, q21.Green);
                    float fxB = Interpolate(x, x1, x2, q11.Blue, q21.Blue);

                    dest[indexDest] = new Rgb48((ushort)Math.Round(fxR), (ushort)Math.Round(fxG), (ushort)Math.Round(fxB));
                }
                else if (x1 < imageWidth && y2 < imageHeight)
                {
                    Rgb48 q11 = src[y1ImageWidth + x1];
                    Rgb48 q12 = src[y2ImageWidth + x1];

                    float fyR = Interpolate(y, y1, y2, q11.Red, q12.Red);
                    float fyG = Interpolate(y, y1, y2, q11.Green, q12.Green);
                    float fyB = Interpolate(y, y1, y2, q11.Blue, q12.Blue);

                    dest[indexDest] = new Rgb48((ushort)Math.Round(fyR), (ushort)Math.Round(fyG), (ushort)Math.Round(fyB));
                }
                else if (x1 < imageWidth && y1 < imageHeight)
                {
                    dest[indexDest] = src[y1ImageWidth + x1];
                }
                else
                {
                    dest[indexDest] = default;
                }
            }
            else
            {
                dest[indexDest] = default;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PlanarPixelBuffer<LS> CropRgbFFF(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<LS>)parameters.Input!;
        var targetBuffer = new PlanarPixelBuffer<LS>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropSingle(sourceBuffer.GetChannel(0).AsSingle(), targetBuffer.GetChannel(0).AsSingle(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
                CropSingle(sourceBuffer.GetChannel(1).AsSingle(), targetBuffer.GetChannel(1).AsSingle(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
                CropSingle(sourceBuffer.GetChannel(2).AsSingle(), targetBuffer.GetChannel(2).AsSingle(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PlanarPixelBuffer<L8> CropRgb888(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input!;
        var targetBuffer = new PlanarPixelBuffer<L8>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropByte(sourceBuffer.GetChannel(0).AsByte(), targetBuffer.GetChannel(0).AsByte(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
                CropByte(sourceBuffer.GetChannel(1).AsByte(), targetBuffer.GetChannel(1).AsByte(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
                CropByte(sourceBuffer.GetChannel(2).AsByte(), targetBuffer.GetChannel(2).AsByte(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PlanarPixelBuffer<L16> CropRgb161616(CropParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input!;
        var targetBuffer = new PlanarPixelBuffer<L16>(parameters.Rectangle.Width, parameters.Rectangle.Height);
        var rectangle = new CropRectangle(parameters.Rectangle);

        if (!TryCrop(sourceBuffer, targetBuffer, parameters.ParallelOptions, rectangle))
        {
            Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
            {
                CropUInt16(sourceBuffer.GetChannel(0).AsUInt16(), targetBuffer.GetChannel(0).AsUInt16(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
                CropUInt16(sourceBuffer.GetChannel(1).AsUInt16(), targetBuffer.GetChannel(1).AsUInt16(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
                CropUInt16(sourceBuffer.GetChannel(2).AsUInt16(), targetBuffer.GetChannel(2).AsUInt16(), sourceBuffer.Width, targetBuffer.Width, rowIndex, rectangle);
            });
        }

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool TryCrop<TPixel>(ReadOnlyPixelBuffer<TPixel> sourcePixelBuffer, IPixelBuffer<TPixel> targetPixelBuffer, ParallelOptions parallelOptions, CropRectangle rectangle)
            where TPixel : unmanaged, IPixel
    {
        if (Math.Abs(rectangle.Rotation) <= float.Epsilon)
        {
            Crop0Degree(sourcePixelBuffer, targetPixelBuffer, parallelOptions, rectangle);
            return true;
        }
        else if (Math.Abs(rectangle.Rotation - D90) <= float.Epsilon)
        {
            Crop90Degree(sourcePixelBuffer, targetPixelBuffer, parallelOptions, rectangle);
            return true;
        }
        else if (Math.Abs(rectangle.Rotation - Math.PI) <= float.Epsilon)
        {
            Crop180Degree(sourcePixelBuffer, targetPixelBuffer, parallelOptions, rectangle);
            return true;
        }
        else if (Math.Abs(rectangle.Rotation - D270) <= float.Epsilon)
        {
            Crop270Degree(sourcePixelBuffer, targetPixelBuffer, parallelOptions, rectangle);
            return true;
        }
        else
        {
            return false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void Crop0Degree<TPixel>(ReadOnlyPixelBuffer<TPixel> sourcePixelBuffer, IPixelBuffer<TPixel> targetPixelBuffer, ParallelOptions parallelOptions, CropRectangle rectangle)
            where TPixel : unmanaged, IPixel
    {
        int x0 = (int)(rectangle.X - rectangle.HalfWidth);
        int y0 = (int)(rectangle.Y - rectangle.HalfHeight);

        _ = Parallel.For(0, targetPixelBuffer.Height, parallelOptions, yDest =>
        {
            for (int channel = 0; channel < sourcePixelBuffer.NumberOfChannels; channel++)
            {
                Span<TPixel> channelDest = targetPixelBuffer.GetChannel(channel);
                ReadOnlySpan<TPixel> channelSrc = sourcePixelBuffer.GetChannel(channel);

                int yDestWidth = yDest * targetPixelBuffer.Width;
                int y = y0 + yDest;
                if (y >= 0 && y < sourcePixelBuffer.Height)
                {
                    int yImageWidth = y * sourcePixelBuffer.Width;
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int x = x0 + xDest;
                        int indexDest = yDestWidth + xDest;
                        if (x >= 0 && x < sourcePixelBuffer.Width)
                        {
                            int indexSrc = yImageWidth + x;
                            channelDest[indexDest] = channelSrc[indexSrc];
                        }
                        else
                        {
                            channelDest[indexDest] = default;
                        }
                    }
                }
                else
                {
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int indexDest = yDestWidth + xDest;
                        channelDest[indexDest] = default;
                    }
                }
            }
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void Crop90Degree<TPixel>(ReadOnlyPixelBuffer<TPixel> sourcePixelBuffer, IPixelBuffer<TPixel> targetPixelBuffer, ParallelOptions parallelOptions, CropRectangle rectangle)
            where TPixel : unmanaged, IPixel
    {
        int x0 = (int)(rectangle.X + rectangle.HalfWidth - 1);
        int y0 = (int)(rectangle.Y - rectangle.HalfHeight);

        _ = Parallel.For(0, targetPixelBuffer.Height, parallelOptions, yDest =>
        {
            for (int channel = 0; channel < sourcePixelBuffer.NumberOfChannels; channel++)
            {
                Span<TPixel> channelDest = targetPixelBuffer.GetChannel(channel);
                ReadOnlySpan<TPixel> channelSrc = sourcePixelBuffer.GetChannel(channel);

                int yDestWidth = yDest * targetPixelBuffer.Width;
                int x = x0 - yDest;
                if (x >= 0 && x < sourcePixelBuffer.Width)
                {
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int y = y0 + xDest;
                        int indexDest = yDestWidth + xDest;
                        if (y >= 0 && y < sourcePixelBuffer.Height)
                        {
                            int indexSrc = (y * sourcePixelBuffer.Width) + x;
                            channelDest[indexDest] = channelSrc[indexSrc];
                        }
                        else
                        {
                            channelDest[indexDest] = default;
                        }
                    }
                }
                else
                {
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int indexDest = yDestWidth + xDest;
                        channelDest[indexDest] = default;
                    }
                }
            }
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void Crop180Degree<TPixel>(ReadOnlyPixelBuffer<TPixel> sourcePixelBuffer, IPixelBuffer<TPixel> targetPixelBuffer, ParallelOptions parallelOptions, CropRectangle rectangle)
            where TPixel : unmanaged, IPixel
    {
        int x0 = (int)(rectangle.X + rectangle.HalfWidth - 1);
        int y0 = (int)(rectangle.Y + rectangle.HalfHeight - 1);

        _ = Parallel.For(0, targetPixelBuffer.Height, parallelOptions, yDest =>
        {
            for (int channel = 0; channel < sourcePixelBuffer.NumberOfChannels; channel++)
            {
                Span<TPixel> channelDest = targetPixelBuffer.GetChannel(channel);
                ReadOnlySpan<TPixel> channelSrc = sourcePixelBuffer.GetChannel(channel);

                int yDestWidth = yDest * targetPixelBuffer.Width;
                int y = y0 - yDest;
                if (y >= 0 && y < sourcePixelBuffer.Height)
                {
                    int yImageWidth = y * sourcePixelBuffer.Width;
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int x = x0 - xDest;
                        int indexDest = yDestWidth + xDest;
                        if (x >= 0 && x < sourcePixelBuffer.Width)
                        {
                            int indexSrc = yImageWidth + x;
                            channelDest[indexDest] = channelSrc[indexSrc];
                        }
                        else
                        {
                            channelDest[indexDest] = default;
                        }
                    }
                }
                else
                {
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int indexDest = yDestWidth + xDest;
                        channelDest[indexDest] = default;
                    }
                }
            }
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void Crop270Degree<TPixel>(ReadOnlyPixelBuffer<TPixel> sourcePixelBuffer, IPixelBuffer<TPixel> targetPixelBuffer, ParallelOptions parallelOptions, CropRectangle rectangle)
            where TPixel : unmanaged, IPixel
    {
        int x0 = (int)(rectangle.X - rectangle.HalfWidth);
        int y0 = (int)(rectangle.Y + rectangle.HalfHeight - 1);

        _ = Parallel.For(0, targetPixelBuffer.Height, parallelOptions, yDest =>
        {
            for (int channel = 0; channel < sourcePixelBuffer.NumberOfChannels; channel++)
            {
                Span<TPixel> channelDest = targetPixelBuffer.GetChannel(channel);
                ReadOnlySpan<TPixel> channelSrc = sourcePixelBuffer.GetChannel(channel);

                int yDestWidth = yDest * targetPixelBuffer.Width;
                int x = x0 + yDest;
                if (x >= 0 && x < sourcePixelBuffer.Width)
                {
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int y = y0 - xDest;
                        int indexDest = yDestWidth + xDest;
                        if (y >= 0 && y < sourcePixelBuffer.Height)
                        {
                            int indexSrc = (y * sourcePixelBuffer.Width) + x;
                            channelDest[indexDest] = channelSrc[indexSrc];
                        }
                        else
                        {
                            channelDest[indexDest] = default;
                        }
                    }
                }
                else
                {
                    for (int xDest = 0; xDest < targetPixelBuffer.Width; xDest++)
                    {
                        int indexDest = yDestWidth + xDest;
                        channelDest[indexDest] = default;
                    }
                }
            }
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void CropByte(ReadOnlySpan<byte> byteSrc, Span<byte> byteDest, int imageWidth, int imageHeight, int yDest, CropRectangle rectangle)
    {
        float yc = yDest - rectangle.HalfHeight;
        for (int xDest = 0; xDest < rectangle.Width; xDest++)
        {
            float xc = xDest - rectangle.HalfWidth;
            float xr = xc * rectangle.Cos - yc * rectangle.Sin;
            float yr = xc * rectangle.Sin + yc * rectangle.Cos;
            float x = xr + rectangle.Left;
            float y = yr + rectangle.Top;

            int x1 = (int)x;
            int y1 = (int)y;

            int indexDest = (yDest * rectangle.Width) + xDest;
            if (x1 >= 0 && y1 >= 0)
            {
                int x2 = x1 + 1;
                int y2 = y1 + 1;

                int y1ImageWidth = y1 * imageWidth;
                int y2ImageWidth = y1ImageWidth + imageWidth;

                // See https://en.wikipedia.org/wiki/Bilinear_interpolation
                if (x2 < imageWidth && y2 < imageHeight)
                {
                    byte q11 = byteSrc[y1ImageWidth + x1];
                    byte q21 = byteSrc[y1ImageWidth + x2];
                    byte q12 = byteSrc[y2ImageWidth + x1];
                    byte q22 = byteSrc[y2ImageWidth + x2];

                    float fx1 = Interpolate(x, x1, x2, q11, q21);
                    float fx2 = Interpolate(x, x1, x2, q12, q22);
                    float fxy = Interpolate(y, y1, y2, fx1, fx2);

                    byteDest[indexDest] = (byte)Math.Round(fxy);
                }
                else if (x2 < imageWidth && y1 < imageHeight)
                {
                    byte q11 = byteSrc[y1ImageWidth + x1];
                    byte q21 = byteSrc[y1ImageWidth + x2];

                    float fx = Interpolate(x, x1, x2, q11, q21);

                    byteDest[indexDest] = (byte)Math.Round(fx);
                }
                else if (x1 < imageWidth && y2 < imageHeight)
                {
                    byte q11 = byteSrc[y1ImageWidth + x1];
                    byte q12 = byteSrc[y2ImageWidth + x1];

                    float fy = Interpolate(y, y1, y2, q11, q12);

                    byteDest[indexDest] = (byte)Math.Round(fy);
                }
                else if (x1 < imageWidth && y1 < imageHeight)
                {
                    byteDest[indexDest] = byteSrc[y1ImageWidth + x1];
                }
                else
                {
                    byteDest[indexDest] = default;
                }
            }
            else
            {
                byteDest[indexDest] = default;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void CropSingle(ReadOnlySpan<float> floatSrc, Span<float> floatDest, int imageWidth, int imageHeight, int yDest, CropRectangle rectangle)
    {
        float yc = yDest - rectangle.HalfHeight;
        for (int xDest = 0; xDest < rectangle.Width; xDest++)
        {
            float xc = xDest - rectangle.HalfWidth;
            float xr = xc * rectangle.Cos - yc * rectangle.Sin;
            float yr = xc * rectangle.Sin + yc * rectangle.Cos;
            float x = xr + rectangle.Left;
            float y = yr + rectangle.Top;

            int x1 = (int)x;
            int y1 = (int)y;

            int indexDest = (yDest * rectangle.Width) + xDest;
            if (x1 >= 0 && y1 >= 0)
            {
                int x2 = x1 + 1;
                int y2 = y1 + 1;

                int y1ImageWidth = y1 * imageWidth;
                int y2ImageWidth = y1ImageWidth + imageWidth;

                // See https://en.wikipedia.org/wiki/Bilinear_interpolation
                if (x2 < imageWidth && y2 < imageHeight)
                {
                    float q11 = floatSrc[y1ImageWidth + x1];
                    float q21 = floatSrc[y1ImageWidth + x2];
                    float q12 = floatSrc[y2ImageWidth + x1];
                    float q22 = floatSrc[y2ImageWidth + x2];

                    float fx1 = Interpolate(x, x1, x2, q11, q21);
                    float fx2 = Interpolate(x, x1, x2, q12, q22);
                    float fxy = Interpolate(y, y1, y2, fx1, fx2);

                    floatDest[indexDest] = (float)fxy;
                }
                else if (x2 < imageWidth && y1 < imageHeight)
                {
                    float q11 = floatSrc[y1ImageWidth + x1];
                    float q21 = floatSrc[y1ImageWidth + x2];

                    float fx = Interpolate(x, x1, x2, q11, q21);

                    floatDest[indexDest] = (float)fx;
                }
                else if (x1 < imageWidth && y2 < imageHeight)
                {
                    float q11 = floatSrc[y1ImageWidth + x1];
                    float q12 = floatSrc[y2ImageWidth + x1];

                    float fy = Interpolate(y, y1, y2, q11, q12);

                    floatDest[indexDest] = (float)fy;
                }
                else if (x1 < imageWidth && y1 < imageHeight)
                {
                    floatDest[indexDest] = floatSrc[y1ImageWidth + x1];
                }
                else
                {
                    floatDest[indexDest] = default;
                }
            }
            else
            {
                floatDest[indexDest] = default;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void CropUInt16(ReadOnlySpan<ushort> byteSrc, Span<ushort> byteDest, int imageWidth, int imageHeight, int yDest, CropRectangle rectangle)
    {
        float yc = yDest - rectangle.HalfHeight;
        for (int xDest = 0; xDest < rectangle.Width; xDest++)
        {
            float xc = xDest - rectangle.HalfWidth;
            float xr = xc * rectangle.Cos - yc * rectangle.Sin;
            float yr = xc * rectangle.Sin + yc * rectangle.Cos;
            float x = xr + rectangle.Left;
            float y = yr + rectangle.Top;

            int x1 = (int)x;
            int y1 = (int)y;

            int indexDest = (yDest * rectangle.Width) + xDest;
            if (x1 >= 0 && y1 >= 0)
            {
                int x2 = x1 + 1;
                int y2 = y1 + 1;

                int y1ImageWidth = y1 * imageWidth;
                int y2ImageWidth = y1ImageWidth + imageWidth;

                // See https://en.wikipedia.org/wiki/Bilinear_interpolation
                if (x2 < imageWidth && y2 < imageHeight)
                {
                    ushort q11 = byteSrc[y1ImageWidth + x1];
                    ushort q21 = byteSrc[y1ImageWidth + x2];
                    ushort q12 = byteSrc[y2ImageWidth + x1];
                    ushort q22 = byteSrc[y2ImageWidth + x2];

                    float fx1 = Interpolate(x, x1, x2, q11, q21);
                    float fx2 = Interpolate(x, x1, x2, q12, q22);
                    float fxy = Interpolate(y, y1, y2, fx1, fx2);

                    byteDest[indexDest] = (ushort)Math.Round(fxy);
                }
                else if (x2 < imageWidth && y1 < imageHeight)
                {
                    ushort q11 = byteSrc[y1ImageWidth + x1];
                    ushort q21 = byteSrc[y1ImageWidth + x2];

                    float fx = Interpolate(x, x1, x2, q11, q21);

                    byteDest[indexDest] = (ushort)Math.Round(fx);
                }
                else if (x1 < imageWidth && y2 < imageHeight)
                {
                    ushort q11 = byteSrc[y1ImageWidth + x1];
                    ushort q12 = byteSrc[y2ImageWidth + x1];

                    float fy = Interpolate(y, y1, y2, q11, q12);

                    byteDest[indexDest] = (ushort)Math.Round(fy);
                }
                else if (x1 < imageWidth && y1 < imageHeight)
                {
                    byteDest[indexDest] = byteSrc[y1ImageWidth + x1];
                }
                else
                {
                    byteDest[indexDest] = default;
                }
            }
            else
            {
                byteDest[indexDest] = default;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static float Interpolate(float v, float v1, float v2, float t1, float t2)
    {
        float p1 = (v2 - v) * t1;
        float p2 = (v - v1) * t2;
        return p1 + p2;
    }
}
