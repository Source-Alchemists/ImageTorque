using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public class MonoConvertOperation : PixelBufferConverter
{
    public MonoConvertOperation()
    {
        AddOperation<ReadOnlyPackedPixelBuffer<Mono>, PackedPixelBuffer<Mono8>>(MonoToMono8);
        AddOperation<ReadOnlyPackedPixelBuffer<Mono>, PackedPixelBuffer<Mono16>>(MonoToMono16);
        AddOperation<ReadOnlyPackedPixelBuffer<Mono8>, PackedPixelBuffer<Mono>>(Mono8ToMono);
        AddOperation<ReadOnlyPackedPixelBuffer<Mono8>, PackedPixelBuffer<Mono16>>(Mono8ToMono16);
        AddOperation<ReadOnlyPackedPixelBuffer<Mono16>, PackedPixelBuffer<Mono>>(Mono16ToMono);
        AddOperation<ReadOnlyPackedPixelBuffer<Mono16>, PackedPixelBuffer<Mono8>>(Mono16ToMono8);
    }

    private PackedPixelBuffer<Mono16> MonoToMono16(ConvertParameters parameters)
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

    private PackedPixelBuffer<Mono8> MonoToMono8(ConvertParameters parameters)
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

    private PackedPixelBuffer<Mono> Mono8ToMono(ConvertParameters parameters)
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

    private PackedPixelBuffer<Mono16> Mono8ToMono16(ConvertParameters parameters)
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

    private PackedPixelBuffer<Mono> Mono16ToMono(ConvertParameters parameters)
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

    private PackedPixelBuffer<Mono8> Mono16ToMono8(ConvertParameters parameters)
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
