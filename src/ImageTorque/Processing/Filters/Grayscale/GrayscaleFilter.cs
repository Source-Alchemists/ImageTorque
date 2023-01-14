using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal class GrayscaleFilter : IProcessor<GrayscaleFilterParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(GrayscaleFilterParameters parameters)
    {
        Type inputType = parameters.Input.GetType();

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            return RgbToMono(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return Rgb24ToMono8(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return Rgb48ToMono16(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<RgbFFF>))
        {
            return RgbFFFToMono(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<Rgb888>))
        {
            return Rgb888ToMono8(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<Rgb161616>))
        {
            return Rgb161616ToMono16(parameters);
        }

        throw new NotSupportedException("The specified input type is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<LF> RgbToMono(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var targetPixelBuffer = new PackedPixelBuffer<LF>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<Rgb> sourceRow = sourcePixelBuffer.GetRow(row);
            Span<LF> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                Rgb sourcePixel = sourceRow[column];
                targetRow[column] = ToGrayscale(sourcePixel.Red, sourcePixel.Green, sourcePixel.Blue);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<L8> Rgb24ToMono8(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var targetPixelBuffer = new PackedPixelBuffer<L8>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<Rgb24> sourceRow = sourcePixelBuffer.GetRow(row);
            Span<L8> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                Rgb24 sourcePixel = sourceRow[column];
                targetRow[column] = ToGrayscale(sourcePixel.Red, sourcePixel.Green, sourcePixel.Blue);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<L16> Rgb48ToMono16(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var targetPixelBuffer = new PackedPixelBuffer<L16>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<Rgb48> sourceRow = sourcePixelBuffer.GetRow(row);
            Span<L16> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                Rgb48 sourcePixel = sourceRow[column];
                targetRow[column] = ToGrayscale(sourcePixel.Red, sourcePixel.Green, sourcePixel.Blue);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<LF> RgbFFFToMono(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var targetPixelBuffer = new PackedPixelBuffer<LF>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<RgbFFF> sourceRowRed = sourcePixelBuffer.GetRow(0, row);
            ReadOnlySpan<RgbFFF> sourceRowGreen = sourcePixelBuffer.GetRow(1, row);
            ReadOnlySpan<RgbFFF> sourceRowBlue = sourcePixelBuffer.GetRow(2, row);
            Span<LF> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                targetRow[column] = ToGrayscale(sourceRowRed[column], sourceRowGreen[column], sourceRowBlue[column]);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<L8> Rgb888ToMono8(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var targetPixelBuffer = new PackedPixelBuffer<L8>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<Rgb888> sourceRowRed = sourcePixelBuffer.GetRow(0, row);
            ReadOnlySpan<Rgb888> sourceRowGreen = sourcePixelBuffer.GetRow(1, row);
            ReadOnlySpan<Rgb888> sourceRowBlue = sourcePixelBuffer.GetRow(2, row);
            Span<L8> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                targetRow[column] = ToGrayscale(sourceRowRed[column], sourceRowGreen[column], sourceRowBlue[column]);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<L16> Rgb161616ToMono16(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var targetPixelBuffer = new PackedPixelBuffer<L16>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<Rgb161616> sourceRowRed = sourcePixelBuffer.GetRow(0, row);
            ReadOnlySpan<Rgb161616> sourceRowGreen = sourcePixelBuffer.GetRow(1, row);
            ReadOnlySpan<Rgb161616> sourceRowBlue = sourcePixelBuffer.GetRow(2, row);
            Span<L16> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                targetRow[column] = ToGrayscale(sourceRowRed[column], sourceRowGreen[column], sourceRowBlue[column]);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static float ToGrayscale(float red, float green, float blue)
                            => (red * .299f) + (green * .587f) + (blue * .114f);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static ushort ToGrayscale(ushort red, ushort green, ushort blue)
                            => (ushort)((red * 299 + green * 587 + blue * 114) / 1000);

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static byte ToGrayscale(byte red, byte green, byte blue)
                            => (byte)((red * 299 + green * 587 + blue * 114) / 1000);
}
