using ImageTorque.Buffers;

namespace ImageTorque.Processing;

internal record ResizerParameters : ProcessorParameters
{
    public override ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public IReadOnlyPixelBuffer? Input { get; init; }

    public int Width { get; init; } = 100;

    public int Height { get; init; } = 100;

    public ResizeMode ResizeMode { get; init; } = ResizeMode.Bilinear;
}
