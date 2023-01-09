using ImageTorque.Operations;

namespace ImageTorque.Buffers;

public record ConvertParameters : IOperationParameters
{
    public ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = 1 };
    public IReadOnlyPixelBuffer Input { get; init; } = null!;
    public Type OutputType { get; init; } = null!;
}
