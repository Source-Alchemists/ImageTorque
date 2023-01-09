using ImageTorque.Operations;

namespace ImageTorque.Buffers;

public record ConvertDescription : OperationDescription
{
    public ConvertDescription() : base() { }
    public ConvertDescription(Type inputType, Type outputType, Delegate operation)
        : base(inputType, outputType, operation)
    {
    }
}
