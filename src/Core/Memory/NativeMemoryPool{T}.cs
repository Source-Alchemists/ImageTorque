using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Memory;

/// <summary>
/// Allocates native unmanagmed memory.
/// </summary>
/// <remarks>It's not a yet a memory pool, it just allocates the memory and frees it later.</remarks>
/// <typeparam name="T">Must be a unmanaged type.</typeparam>
public sealed unsafe class NativeMemoryPool<T> : MemoryPool<T> where T : unmanaged
{
    private bool _isDisposed = false;

    /// <inheritdoc />
    public static new NativeMemoryPool<T> Shared { get; } = new NativeMemoryPool<T>();

    /// <inheritdoc />
    public override int MaxBufferSize => int.MaxValue;

    /// <inheritdoc />
    public override IMemoryOwner<T> Rent(int minBufferSize = -1)
    {
        int memorySize = minBufferSize * Unsafe.SizeOf<T>();
        void* pointer = NativeMemory.AllocZeroed((uint)memorySize);
        var block = new NativeMemoryBlock { Pointer = pointer, Size = memorySize, MaxSize = memorySize };

        return new NativeMemoryManager<T>(this, block);
    }

    /// <summary>
    /// Return memory owner to pool.
    /// </summary>
    /// <param name="memoryOwner">The memory owner to return.</param>
    internal void Return(NativeMemoryManager<T> memoryOwner)
    {
        NativeMemory.Free(memoryOwner._memoryBlock.Pointer);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            _isDisposed = true;
        }
    }
}
