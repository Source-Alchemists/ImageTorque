using System.Buffers;
using System.Runtime.CompilerServices;
namespace ImageTorque.Memory;

internal sealed unsafe class NativeMemoryManager<T>(NativeMemoryPool<T> pool, NativeMemoryBlock memoryBlock) : MemoryManager<T> where T : unmanaged
{
    internal NativeMemoryBlock _memoryBlock = memoryBlock;
    private readonly NativeMemoryPool<T> _pool = pool;
    private bool _isDisposed = false;

    /// <inheritdoc/>
    public override Span<T> GetSpan()
    {
        return new Span<T>(_memoryBlock.Pointer, _memoryBlock.Size / Unsafe.SizeOf<T>());
    }

    /// <inheritdoc/>
    public override MemoryHandle Pin(int elementIndex = 0)
    {
        return new MemoryHandle(((T*)_memoryBlock.Pointer) + elementIndex, pinnable: this);
    }

    /// <inheritdoc/>
    public override void Unpin() {}

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            _pool.Return(this);
            _isDisposed = true;
        }
    }
}
