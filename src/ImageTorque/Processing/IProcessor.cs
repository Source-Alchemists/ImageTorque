namespace ImageTorque.Processing;

public interface IProcessor<in TParameters, out TResult>
    where TParameters : ProcessorParameters, new()
{
    TResult Execute(TParameters parameters);
}
