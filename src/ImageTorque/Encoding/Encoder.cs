using ImageTorque.Operations;

namespace ImageTorque.Encoding;

public class Encoder : Operation<EncoderDescription, EncoderParameters, object>
{
    public override object Execute(EncoderParameters parameters)
    {
        OperationDescription? description = Descriptions.Where(o => o.GetType() == typeof(EncoderDescription)
                                            && o.InputType == parameters.Input!.GetType()).FirstOrDefault();
        if (description == null)
            throw new InvalidOperationException($"No encoder found for {parameters.Input!.GetType()}.");

        description.Operation!.DynamicInvoke(parameters);
        return null!;
    }
}
