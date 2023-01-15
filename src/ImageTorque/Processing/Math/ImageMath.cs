using System.Numerics;
using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal sealed class ImageMath : IProcessor<ImageMathParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(ImageMathParameters parameters)
    {
        Type inputType = parameters.InputA!.GetType();
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<LF>))
        {
            return PackedL1Operation<LF, float>((ReadOnlyPackedPixelBuffer<LF>)parameters.InputA, (ReadOnlyPackedPixelBuffer<LF>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L8>))
        {
            return PackedL1Operation<L8, byte>((ReadOnlyPackedPixelBuffer<L8>)parameters.InputA, (ReadOnlyPackedPixelBuffer<L8>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L16>))
        {
            return PackedL1Operation<L16, ushort>((ReadOnlyPackedPixelBuffer<L16>)parameters.InputA, (ReadOnlyPackedPixelBuffer<L16>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            return PackedL3Operation<Rgb, float>((ReadOnlyPackedPixelBuffer<Rgb>)parameters.InputA, (ReadOnlyPackedPixelBuffer<Rgb>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return PackedL3Operation<Rgb24, byte>((ReadOnlyPackedPixelBuffer<Rgb24>)parameters.InputA, (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return PackedL3Operation<Rgb48, ushort>((ReadOnlyPackedPixelBuffer<Rgb48>)parameters.InputA, (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<LF>))
        {
            return PlanarOperation<LF, float>((ReadOnlyPlanarPixelBuffer<LF>)parameters.InputA, (ReadOnlyPlanarPixelBuffer<LF>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L8>))
        {
            return PlanarOperation<L8, byte>((ReadOnlyPlanarPixelBuffer<L8>)parameters.InputA, (ReadOnlyPlanarPixelBuffer<L8>)parameters.InputB!, parameters);
        }
        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L16>))
        {
            return PlanarOperation<L16, ushort>((ReadOnlyPlanarPixelBuffer<L16>)parameters.InputA, (ReadOnlyPlanarPixelBuffer<L16>)parameters.InputB!, parameters);
        }

        throw new NotSupportedException($"The input type is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static IPixelBuffer PackedL1Operation<TPixel, T>(ReadOnlyPackedPixelBuffer<TPixel> sourceA, ReadOnlyPackedPixelBuffer<TPixel> sourceB, ImageMathParameters parameters)
        where TPixel : unmanaged, IL1Pixel<T>
        where T : unmanaged, INumber<T>
    {
        var targetBuffer = new PixelBuffer<TPixel>(sourceA.Width, sourceA.Height);

        _ = Parallel.For(0, sourceA.Height, parameters.ParallelOptions, rowIndex =>
        {
            ReadOnlySpan<TPixel> rowA = sourceA.GetRow(rowIndex);
            ReadOnlySpan<TPixel> rowB = sourceB.GetRow(rowIndex);
            Span<TPixel> targetRow = targetBuffer.GetRow(rowIndex);

            L1ChannelOperation<TPixel, T>(rowA, rowB, targetRow, parameters.ImageMathMode);
        });

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static IPixelBuffer PackedL3Operation<TPixel, T>(ReadOnlyPackedPixelBuffer<TPixel> sourceA, ReadOnlyPackedPixelBuffer<TPixel> sourceB, ImageMathParameters parameters)
        where TPixel : unmanaged, IL3Pixel<T>
        where T : unmanaged, INumber<T>
    {
        var targetBuffer = new PixelBuffer<TPixel>(sourceA.Width, sourceA.Height);

        _ = Parallel.For(0, sourceA.Height, parameters.ParallelOptions, rowIndex =>
        {
            ReadOnlySpan<TPixel> rowA = sourceA.GetRow(rowIndex);
            ReadOnlySpan<TPixel> rowB = sourceB.GetRow(rowIndex);
            Span<TPixel> targetRow = targetBuffer.GetRow(rowIndex);

            int length = rowA.Length;
            T min = rowA[0].Min;
            T max = rowA[0].Max;
            switch (parameters.ImageMathMode)
            {
                case ImageMathMode.Add:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].R = Numerics.Clamp(rowA[x].R + rowB[x].R, min, max);
                            targetRow[x].G = Numerics.Clamp(rowA[x].G + rowB[x].G, min, max);
                            targetRow[x].B = Numerics.Clamp(rowA[x].B + rowB[x].B, min, max);
                        }
                        break;
                    }
                case ImageMathMode.Subtract:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].R = Numerics.Clamp(rowA[x].R - rowB[x].R, min, max);
                            targetRow[x].G = Numerics.Clamp(rowA[x].G - rowB[x].G, min, max);
                            targetRow[x].B = Numerics.Clamp(rowA[x].B - rowB[x].B, min, max);
                        }
                        break;
                    }
                case ImageMathMode.Multiply:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].R = Numerics.Clamp(rowA[x].R * rowB[x].R, min, max);
                            targetRow[x].G = Numerics.Clamp(rowA[x].G * rowB[x].G, min, max);
                            targetRow[x].B = Numerics.Clamp(rowA[x].B * rowB[x].B, min, max);
                        }
                        break;
                    }
            }
        });

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static IPixelBuffer PlanarOperation<TPixel, T>(ReadOnlyPlanarPixelBuffer<TPixel> sourceA, ReadOnlyPlanarPixelBuffer<TPixel> sourceB, ImageMathParameters parameters)
        where TPixel : unmanaged, IL1Pixel<T>
        where T : unmanaged, INumber<T>
    {
        var targetBuffer = new PlanarPixelBuffer<TPixel>(sourceA.Width, sourceA.Height);

        _ = Parallel.For(0, sourceA.Height, parameters.ParallelOptions, rowIndex =>
        {
            for (int channelIndex = 0; channelIndex < 3; channelIndex++)
            {
                ReadOnlySpan<TPixel> rowA = sourceA.GetRow(channelIndex, rowIndex);
                ReadOnlySpan<TPixel> rowB = sourceB.GetRow(channelIndex, rowIndex);
                Span<TPixel> targetRow = targetBuffer.GetRow(channelIndex, rowIndex);

                L1ChannelOperation<TPixel, T>(rowA, rowB, targetRow, parameters.ImageMathMode);
            }
        });

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void L1ChannelOperation<TPixel, T>(ReadOnlySpan<TPixel> rowA, ReadOnlySpan<TPixel> rowB, Span<TPixel> targetRow, ImageMathMode mode)
        where TPixel : unmanaged, IL1Pixel<T>
        where T : unmanaged, INumber<T>
    {
        int length = rowA.Length;
        switch (mode)
        {
            case ImageMathMode.Add:
                {
                    for (int x = 0; x < length; x++)
                    {
                        targetRow[x].Value = ClampAdd(rowA[x], rowB[x]);
                    }
                    break;
                }
            case ImageMathMode.Subtract:
                {
                    for (int x = 0; x < length; x++)
                    {
                        targetRow[x].Value = ClampSubtract(rowA[x], rowB[x]);
                    }
                    break;
                }
            case ImageMathMode.Multiply:
                {
                    for (int x = 0; x < length; x++)
                    {
                        targetRow[x].Value = ClampMultiply(rowA[x], rowB[x]);
                    }
                    break;
                }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static T ClampAdd<T>(IL1Pixel<T> p1, IL1Pixel<T> p2) where T : unmanaged, INumber<T>
    {
        return Numerics.ClampAdd(p1.Value, p2.Value, p1.Min, p1.Max);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static T ClampSubtract<T>(IL1Pixel<T> p1, IL1Pixel<T> p2) where T : unmanaged, INumber<T>
    {
        return Numerics.ClampSubtract(p1.Value, p2.Value, p1.Min, p1.Max);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static T ClampMultiply<T>(IL1Pixel<T> p1, IL1Pixel<T> p2) where T : unmanaged, INumber<T>
    {
        return Numerics.ClampMultiply(p1.Value, p2.Value, p1.Min, p1.Max);
    }
}
