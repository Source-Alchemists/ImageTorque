using ImageTorque.Operations;

namespace ImageTorque.Decoding;

public record DecoderDescription : OperationDescription
{
    public DecoderDescription() : base() { }
    public DecoderDescription(Type inputType, Type outputType, Delegate operation)
        : base(inputType, outputType, operation)
    {
    }
}
