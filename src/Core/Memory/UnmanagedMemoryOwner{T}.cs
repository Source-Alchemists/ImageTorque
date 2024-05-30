using System.Buffers;

namespace ImageTorque.Memory;

internal sealed unsafe class UnmanagedMemoryOwner<T>(OptimizedMemoryPool<T> pool, UnmanagedMemoryBlock<T> memoryBlock, int elementLength) : MemoryManager<T> where T : unmanaged
{
    private readonly OptimizedMemoryPool<T> _pool = pool;
    private readonly int _elementLength = elementLength;
    private bool _isDisposed = false;

    internal UnmanagedMemoryBlock<T> MemoryBlock { get; } = memoryBlock;

    /// <inheritdoc/>
    public override Span<T> GetSpan()
    {
        return MemoryBlock.AsSpan().Slice(0, _elementLength);
    }

    /// <inheritdoc/>
    public override MemoryHandle Pin(int elementIndex = 0)
    {
        return new MemoryHandle(MemoryBlock.AsPointer() + elementIndex, pinnable: this);
    }

    /// <inheritdoc/>
    public override void Unpin() { }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            _pool.Return(MemoryBlock);
            _isDisposed = true;
        }
    }
}
