using System.Reflection;

namespace ImageTorque.Operations;

public abstract class Operation
{
    private static readonly Dictionary<Type, Operation> _loadedOperations = new();

    public static IEnumerable<Operation> LoadedOperations => _loadedOperations.Values;

    internal static DescriptionCollection Descriptions { get; } = new();

    static Operation()
    {
        LoadFrom(typeof(Operation).Assembly);
    }

    public static TOperation Load<TOperation>()
        where TOperation : Operation
    {
        var operationType = typeof(TOperation);
        if (_loadedOperations.TryGetValue(operationType, out var operation))
        {
            return (TOperation)operation;
        }

        var operationInstance = Activator.CreateInstance<TOperation>();
        _loadedOperations.Add(operationType, operationInstance);
        return operationInstance;
    }

    public static IEnumerable<Operation> LoadFrom(params Assembly[] assemblies)
    {
        return LoadFromImplementation(assemblies);
    }

    public static IEnumerable<Operation> LoadFrom(IEnumerable<Assembly> assemblies)
    {
        return LoadFromImplementation(assemblies);
    }

    private static IEnumerable<Operation> LoadFromImplementation(IEnumerable<Assembly> assemblies)
    {
        var et = assemblies.SelectMany(a => a.ExportedTypes);
        var operationTypes = et.Where(t => t.IsClass
                                        && !t.IsAbstract
                                         && t.GetConstructor(Type.EmptyTypes) != null
                                         && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOperation<,,>)));

        var operations = new List<Operation>();
        foreach (var operationType in operationTypes)
        {
            if (_loadedOperations.TryGetValue(operationType, out var knownOperation))
            {
                operations.Add(knownOperation);
                continue;
            }

            var operation = (Operation)Activator.CreateInstance(operationType)!;
            _loadedOperations.Add(operationType, operation);
            operations.Add(operation);
        }

        return operations;
    }
}

public abstract class Operation<TDescription, TParameters, TResult> : Operation, IOperation<TDescription, TParameters, TResult>
    where TDescription : OperationDescription, new()
    where TParameters : class, IOperationParameters, new()
    where TResult : class
{
    public abstract TResult Execute(TParameters parameters);

    protected void AddOperation<TInput, TOutput>(Func<TParameters, TOutput> delegateFunc)
    {
        var description = new TDescription
        {
            InputType = typeof(TInput),
            OutputType = typeof(TOutput),
            Operation = delegateFunc
        };
        Descriptions.Add(description);
    }
}