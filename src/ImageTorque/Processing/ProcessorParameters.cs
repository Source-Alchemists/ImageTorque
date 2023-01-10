namespace ImageTorque.Processing;

public abstract record ProcessorParameters
{
    public virtual ParallelOptions ParallelOptions { get; init; } = new ParallelOptions { MaxDegreeOfParallelism = 1 };
}
