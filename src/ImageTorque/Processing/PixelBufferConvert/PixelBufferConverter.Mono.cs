using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal partial class PixelBufferConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertMono(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PixelBuffer<L8>))
        {
            return MonoToMono8(parameters);
        }

        if (outputType == typeof(PixelBuffer<L16>))
        {
            return MonoToMono16(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertMono8(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PixelBuffer<LS>))
        {
            return Mono8ToMono(parameters);
        }

        if (outputType == typeof(PixelBuffer<L16>))
        {
            return Mono8ToMono16(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertMono16(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PixelBuffer<LS>))
        {
            return Mono16ToMono(parameters);
        }

        if (outputType == typeof(PixelBuffer<L8>))
        {
            return Mono16ToMono8(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L16> MonoToMono16(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<LS>)parameters.Input;
        var resultBuffer = new PixelBuffer<L16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<LS> row = inputBuffer.GetRow(y);
            Span<L16> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToL16();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L8> MonoToMono8(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<LS>)parameters.Input;
        var resultBuffer = new PixelBuffer<L8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<LS> row = inputBuffer.GetRow(y);
            Span<L8> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToL8();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<LS> Mono8ToMono(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<L8>)parameters.Input;
        var resultBuffer = new PixelBuffer<LS>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L8> row = inputBuffer.GetRow(y);
            Span<LS> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToLF();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L16> Mono8ToMono16(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<L8>)parameters.Input;
        var resultBuffer = new PixelBuffer<L16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L8> row = inputBuffer.GetRow(y);
            Span<L16> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToL16();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<LS> Mono16ToMono(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<L16>)parameters.Input;
        var resultBuffer = new PixelBuffer<LS>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L16> row = inputBuffer.GetRow(y);
            Span<LS> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToLF();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L8> Mono16ToMono8(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<L16>)parameters.Input;
        var resultBuffer = new PixelBuffer<L8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L16> row = inputBuffer.GetRow(y);
            Span<L8> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToL8();
            }
        });
        return resultBuffer;
    }
}
