using ImageTorque.Buffers;

namespace ImageTorque.Processing;

public record GrayscaleFilterParameters : ProcessorParameters
{
    public override  ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public IReadOnlyPixelBuffer Input { get; init; } = null!;

    public Type OutputType { get; init; } = null!;
}
