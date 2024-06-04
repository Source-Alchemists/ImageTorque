using ImageTorque.Buffers;

namespace ImageTorque.Processing;

internal record EncoderParameters : ProcessorParameters
{
    public override ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public IReadOnlyPixelBuffer? Input { get; set; }

    public Stream? Stream { get; init; }

    public string EncoderType { get; init; } = "png";

    public int Quality { get; init; } = 80;
}
