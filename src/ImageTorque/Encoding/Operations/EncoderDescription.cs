using ImageTorque.Operations;

namespace ImageTorque.Encoding;

public record EncoderDescription : OperationDescription
{
    public EncoderDescription() : base() { }
    public EncoderDescription(Type inputType, Type outputType, Delegate operation)
        : base(inputType, outputType, operation)
    {
    }
}
