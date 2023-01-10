using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

public partial class PixelBufferConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertMono(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PackedPixelBuffer<Mono8>))
        {
            return MonoToMono8(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Mono16>))
        {
            return MonoToMono16(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertMono8(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PackedPixelBuffer<Mono>))
        {
            return Mono8ToMono(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Mono16>))
        {
            return Mono8ToMono16(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertMono16(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PackedPixelBuffer<Mono>))
        {
            return Mono16ToMono(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Mono8>))
        {
            return Mono16ToMono8(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono16> MonoToMono16(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Mono>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Mono16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Mono> row = inputBuffer.GetRow(y);
            Span<Mono16> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToMono16();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono8> MonoToMono8(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Mono>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Mono8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Mono> row = inputBuffer.GetRow(y);
            Span<Mono8> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToMono8();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono> Mono8ToMono(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Mono8>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Mono>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Mono8> row = inputBuffer.GetRow(y);
            Span<Mono> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToMono();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono16> Mono8ToMono16(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Mono8>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Mono16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Mono8> row = inputBuffer.GetRow(y);
            Span<Mono16> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToMono16();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono> Mono16ToMono(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Mono16>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Mono>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Mono16> row = inputBuffer.GetRow(y);
            Span<Mono> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToMono();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono8> Mono16ToMono8(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Mono16>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Mono8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Mono16> row = inputBuffer.GetRow(y);
            Span<Mono8> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < row.Length; x++)
            {
                resultRow[x] = row[x].ToMono8();
            }
        });
        return resultBuffer;
    }
}
