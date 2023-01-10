using ImageTorque.Buffers;

namespace ImageTorque.Processing;

public record CropParameters : ProcessorParameters
{
    public override ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public IReadOnlyPixelBuffer? Input { get; init; }

    public Rectangle Rectangle { get; init; }
}
