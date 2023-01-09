using ImageTorque.Operations;

namespace ImageTorque.Buffers;

public class PixelBufferConverter : Operation<ConvertDescription, ConvertParameters, IPixelBuffer>
{
    public override IPixelBuffer Execute(ConvertParameters parameters)
    {
        var description = Descriptions.Where(o => o.GetType() == typeof(ConvertDescription)
                                            && o.InputType == parameters.Input.GetType()
                                            && o.OutputType == parameters.OutputType).FirstOrDefault();
        if (description == null)
            throw new InvalidOperationException($"No converter found for {parameters.Input.GetType()} to {parameters.OutputType}.");

        return (IPixelBuffer)description.Operation!.DynamicInvoke(parameters)!;
    }
}
