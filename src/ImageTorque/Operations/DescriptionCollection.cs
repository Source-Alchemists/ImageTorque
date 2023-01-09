using System.Collections;
using System.Collections.Concurrent;

namespace ImageTorque.Operations;

internal class DescriptionCollection : IEnumerable<OperationDescription>
{
    private readonly ConcurrentBag<OperationDescription> _descriptions = new();

    public void Add<TDescription>(TDescription description)
        where TDescription : OperationDescription
    {
        if (!_descriptions.Contains(description))
        {
            _descriptions.Add(description);
        }
    }

    public TDescription Get<TDescription>(Type inputType)
        where TDescription : OperationDescription
    {
        var description = _descriptions.OfType<TDescription>().First(i => i.InputType == inputType);
        return description;
    }

    public TDescription Get<TDescription>(IEnumerable<Type> inputTypes)
        where TDescription : OperationDescription
    {
        var descriptions = _descriptions.OfType<TDescription>();
        var description = descriptions.First(i => inputTypes.Contains(i.InputType));
        return description;
    }

    public IEnumerable<TDescription> Get<TDescription>()
        where TDescription : OperationDescription
    {
        return _descriptions.OfType<TDescription>();
    }

    public IEnumerator<OperationDescription> GetEnumerator()
    {
        return ((IEnumerable<OperationDescription>)_descriptions).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_descriptions).GetEnumerator();
    }
}