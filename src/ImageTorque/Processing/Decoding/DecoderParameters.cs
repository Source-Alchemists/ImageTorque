namespace ImageTorque.Processing;

internal record DecoderParameters : ProcessorParameters
{
    public Stream? Input { get; init; }

    public Type OutputType { get; init; } = null!;
}
