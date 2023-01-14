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

            int length = rowA.Length;
            switch (parameters.ImageMathMode)
            {
                case ImageMathMode.Add:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].Value = rowA[x].Value + rowB[x].Value;
                        }
                        break;
                    }
                case ImageMathMode.Subtract:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].Value = rowA[x].Value - rowB[x].Value;
                        }
                        break;
                    }
                case ImageMathMode.Multiply:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].Value = rowA[x].Value * rowB[x].Value;
                        }
                        break;
                    }
                case ImageMathMode.Divide:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].Value = rowA[x].Value / rowB[x].Value;
                        }
                        break;
                    }
            }
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
            switch (parameters.ImageMathMode)
            {
                case ImageMathMode.Add:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].R = rowA[x].R + rowB[x].R;
                            targetRow[x].G = rowA[x].G + rowB[x].G;
                            targetRow[x].B = rowA[x].B + rowB[x].B;
                        }
                        break;
                    }
                case ImageMathMode.Subtract:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].R = rowA[x].R - rowB[x].R;
                            targetRow[x].G = rowA[x].G - rowB[x].G;
                            targetRow[x].B = rowA[x].B - rowB[x].B;
                        }
                        break;
                    }
                case ImageMathMode.Multiply:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].R = rowA[x].R * rowB[x].R;
                            targetRow[x].G = rowA[x].G * rowB[x].G;
                            targetRow[x].B = rowA[x].B * rowB[x].B;
                        }
                        break;
                    }
                case ImageMathMode.Divide:
                    {
                        for (int x = 0; x < length; x++)
                        {
                            targetRow[x].R = rowA[x].R / rowB[x].R;
                            targetRow[x].G = rowA[x].G / rowB[x].G;
                            targetRow[x].B = rowA[x].B / rowB[x].B;
                        }
                        break;
                    }
            }
        });

        return targetBuffer;
    }

    // [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    // private static IPixelBuffer PlanarL3Operation<TPixel, T>(ReadOnlyPlanarPixelBuffer<TPixel> sourceA, ReadOnlyPlanarPixelBuffer<TPixel> sourceB, ImageMathParameters parameters)
    //     where TPixel : unmanaged, IPackedL3Pixel<T>
    //     where T : unmanaged, INumber<T>
    // {
    // }
}
