using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal sealed partial class Binarizer : IProcessor<BinarizerParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(BinarizerParameters parameters)
    {
        Type inputType = parameters.Input!.GetType();

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Mono>))
        {
            return BinarizeMono(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Mono8>))
        {
            return BinarizeMono8(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Mono16>))
        {
            return BinarizeMono16(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            return BinarizeRgb(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return BinarizeRgb24(parameters);
        }

        if(inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return BinarizeRgb48(parameters);
        }

        throw new NotSupportedException("The specified input type is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono> BinarizeMono(BinarizerParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Mono>)parameters.Input!;
        var targetBuffer = new PackedPixelBuffer<Mono>(sourceBuffer.Width, sourceBuffer.Height);
        Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
        {
            BinarizeMono(sourceBuffer.GetRow(rowIndex), targetBuffer.GetRow(rowIndex),
                                            sourceBuffer, parameters.Threshold, parameters.Mode);
        });
        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono8> BinarizeMono8(BinarizerParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Mono8>)parameters.Input!;
        var targetBuffer = new PackedPixelBuffer<Mono8>(sourceBuffer.Width, sourceBuffer.Height);
        Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
        {
            BinarizeMono8(sourceBuffer.GetRow(rowIndex), targetBuffer.GetRow(rowIndex),
                                            sourceBuffer, parameters.Threshold, parameters.Mode);
        });
        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono8> BinarizeMono16(BinarizerParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Mono16>)parameters.Input!;
        var targetBuffer = new PackedPixelBuffer<Mono8>(sourceBuffer.Width, sourceBuffer.Height);
        Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
        {
            BinarizeMono16(sourceBuffer.GetRow(rowIndex), targetBuffer.GetRow(rowIndex),
                                            sourceBuffer, parameters.Threshold, parameters.Mode);
        });
        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono> BinarizeRgb(BinarizerParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input!;
        var targetBuffer = new PackedPixelBuffer<Mono>(sourceBuffer.Width, sourceBuffer.Height);
        Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
        {
            BinarizeRgb(sourceBuffer.GetRow(rowIndex), targetBuffer.GetRow(rowIndex),
                                            sourceBuffer, parameters.Threshold, parameters.Mode);
        });
        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono8> BinarizeRgb24(BinarizerParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input!;
        var targetBuffer = new PackedPixelBuffer<Mono8>(sourceBuffer.Width, sourceBuffer.Height);
        Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
        {
            BinarizeRgb24(sourceBuffer.GetRow(rowIndex), targetBuffer.GetRow(rowIndex),
                                            sourceBuffer, parameters.Threshold, parameters.Mode);
        });
        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono8> BinarizeRgb48(BinarizerParameters parameters)
    {
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input!;
        var targetBuffer = new PackedPixelBuffer<Mono8>(sourceBuffer.Width, sourceBuffer.Height);
        Parallel.For(0, sourceBuffer.Height, parameters.ParallelOptions, rowIndex =>
        {
            BinarizeRgb48(sourceBuffer.GetRow(rowIndex), targetBuffer.GetRow(rowIndex),
                                            sourceBuffer, parameters.Threshold, parameters.Mode);
        });
        return targetBuffer;
    }
}
