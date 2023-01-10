using ImageTorque.Buffers;

namespace ImageTorque.Processing;

public record MirrorParameters : ProcessorParameters
{
    public override ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public IReadOnlyPixelBuffer? Input { get; set; }

    public MirrorMode MirrorMode { get; init; } = MirrorMode.Horizontal;
}
