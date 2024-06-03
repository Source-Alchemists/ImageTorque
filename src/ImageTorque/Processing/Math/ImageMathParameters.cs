using ImageTorque.Buffers;

namespace ImageTorque.Processing;

internal record ImageMathParameters : ProcessorParameters
{
    public override ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

    public IReadOnlyPixelBuffer? InputA { get; set; }
    public IReadOnlyPixelBuffer? InputB { get; set; }

    public ImageMathMode ImageMathMode { get; init; } = ImageMathMode.Add;
}
