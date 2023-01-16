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

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<LS>))
        {
            return RgbFFFToMono(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<L8>))
        {
            return Rgb888ToMono8(parameters);
        }

        if(inputType == typeof(ReadOnlyPlanarPixelBuffer<L16>))
        {
            return Rgb161616ToMono16(parameters);
        }

        throw new NotSupportedException("The specified input type is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<LS> RgbToMono(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var targetPixelBuffer = new PixelBuffer<LS>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<Rgb> sourceRow = sourcePixelBuffer.GetRow(row);
            Span<LS> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                Rgb sourcePixel = sourceRow[column];
                targetRow[column] = ToGrayscale(sourcePixel.Red, sourcePixel.Green, sourcePixel.Blue);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L8> Rgb24ToMono8(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var targetPixelBuffer = new PixelBuffer<L8>(parameters.Input.Width, parameters.Input.Height);
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
    private static PixelBuffer<L16> Rgb48ToMono16(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var targetPixelBuffer = new PixelBuffer<L16>(parameters.Input.Width, parameters.Input.Height);
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
    private static PixelBuffer<LS> RgbFFFToMono(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPlanarPixelBuffer<LS>)parameters.Input;
        var targetPixelBuffer = new PixelBuffer<LS>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<LS> sourceRowRed = sourcePixelBuffer.GetRow(0, row);
            ReadOnlySpan<LS> sourceRowGreen = sourcePixelBuffer.GetRow(1, row);
            ReadOnlySpan<LS> sourceRowBlue = sourcePixelBuffer.GetRow(2, row);
            Span<LS> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                targetRow[column] = ToGrayscale(sourceRowRed[column], sourceRowGreen[column], sourceRowBlue[column]);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L8> Rgb888ToMono8(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input;
        var targetPixelBuffer = new PixelBuffer<L8>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<L8> sourceRowRed = sourcePixelBuffer.GetRow(0, row);
            ReadOnlySpan<L8> sourceRowGreen = sourcePixelBuffer.GetRow(1, row);
            ReadOnlySpan<L8> sourceRowBlue = sourcePixelBuffer.GetRow(2, row);
            Span<L8> targetRow = targetPixelBuffer.GetRow(row);
            for (int column = 0; column < targetPixelBuffer.Width; column++)
            {
                targetRow[column] = ToGrayscale(sourceRowRed[column], sourceRowGreen[column], sourceRowBlue[column]);
            }
        });
        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L16> Rgb161616ToMono16(GrayscaleFilterParameters parameters)
    {
        var sourcePixelBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input;
        var targetPixelBuffer = new PixelBuffer<L16>(parameters.Input.Width, parameters.Input.Height);
        _ = Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
        {
            ReadOnlySpan<L16> sourceRowRed = sourcePixelBuffer.GetRow(0, row);
            ReadOnlySpan<L16> sourceRowGreen = sourcePixelBuffer.GetRow(1, row);
            ReadOnlySpan<L16> sourceRowBlue = sourcePixelBuffer.GetRow(2, row);
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
