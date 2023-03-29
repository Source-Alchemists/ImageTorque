using ImageTorque.Buffers;

namespace ImageTorque.Processing;

internal record BinarizerParameters : ProcessorParameters
{
    public override  ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public IReadOnlyPixelBuffer? Input { get; init; }

    public float Threshold { get; init; } = 0.5f;

    public BinaryThresholdMode Mode { get; init; } = BinaryThresholdMode.Lumincance;
}
