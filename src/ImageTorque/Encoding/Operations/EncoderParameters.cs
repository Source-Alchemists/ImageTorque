using ImageTorque.Buffers;
using ImageTorque.Operations;

namespace ImageTorque.Encoding;

public record EncoderParameters : IOperationParameters
{
    public ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = System.Environment.ProcessorCount };

    public IReadOnlyPixelBuffer? Input { get; set; }

    public Stream? Stream { get; init; }

    public EncoderType EncoderType { get; init; } = EncoderType.Png;

    public int Quality { get; init; } = 80;
}
