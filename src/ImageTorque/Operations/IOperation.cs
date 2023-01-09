namespace ImageTorque.Operations;

public interface IOperation<out TDescription, in TParameters, out TResult>
    where TDescription : OperationDescription, new()
    where TParameters : class, IOperationParameters, new()
    where TResult : class
{
    TResult Execute(TParameters parameter);
}