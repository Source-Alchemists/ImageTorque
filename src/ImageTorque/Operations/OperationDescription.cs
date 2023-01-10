namespace ImageTorque.Operations;

public record OperationDescription
{
    public Type? InputType { get; init; }
    public Type? OutputType { get; init; }
    public Delegate? Operation { get; init; }

    public OperationDescription() {}

    public OperationDescription(Type inputType, Type outputType, Delegate operation)
    {
        InputType = inputType;
        OutputType = outputType;
        Operation = operation;
    }
}
