using ImageTorque.Operations;

namespace ImageTorque.Decoding;

public record DecoderParameters : IOperationParameters
{
    public ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = 1 };

    public Stream? Input { get; set; }

    public Type OutputType { get; init; } = null!;
}
