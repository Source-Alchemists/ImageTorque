namespace ImageTorque.Processing;

/// <summary>
/// Represents a processor.
/// </summary>
public interface IProcessor<in TParameters, out TResult>
    where TParameters : ProcessorParameters, new()
{
    /// <summary>
    /// Executes the processor.
    /// </summary>
    /// <param name="parameters">The processor parameters.</param>
    /// <returns>The processor result.</returns>
    TResult Execute(TParameters parameters);
}
