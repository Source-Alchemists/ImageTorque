using ImageTorque.Buffers;

namespace ImageTorque.Processing;

internal record PixelBufferConvertParameters : ProcessorParameters
{
    public IReadOnlyPixelBuffer Input { get; init; } = null!;
    public Type OutputType { get; init; } = null!;
}
