namespace ImageTorque.Operations;

public interface IOperation<out TDescription, in TParameters, out TResult>
    where TDescription : OperationDescription, new()
    where TParameters : class, IOperationParameters, new()
{
    TResult Execute(TParameters parameters);
}
