using System.Buffers;

namespace ImageTorque.Memory;

/// <summary>
/// Allocates pooled memory.
/// </summary>
/// <typeparam name="T">Must be a unmanaged type.</typeparam>
public sealed class OptimizedMemoryPool<T> : MemoryPool<T> where T : unmanaged
{
    private bool _isDisposed = false;
    private readonly int _memoryBlockSize;
    private readonly UnmanagedMemoryBlock<T>[] _cache;
    private int _cacheIndex = 0;

    /// <inheritdoc />
    public static new OptimizedMemoryPool<T> Shared { get; } = new OptimizedMemoryPool<T>();

    /// <inheritdoc />
    public override int MaxBufferSize => int.MaxValue;

    public OptimizedMemoryPool(int memoryBlockSize = 4096, int cacheCapacity = 5)
    {
        _cache = new UnmanagedMemoryBlock<T>[cacheCapacity];
        _memoryBlockSize = memoryBlockSize;
    }

    /// <inheritdoc />
    public override unsafe IMemoryOwner<T> Rent(int minBufferSize = -1)
    {
        if (minBufferSize == 0)
        {
            return MemoryPool<T>.Shared.Rent(minBufferSize);
        }

        if (minBufferSize * sizeof(T) > _memoryBlockSize)
        {
            return new UnmanagedMemoryOwner<T>(this, new UnmanagedMemoryBlock<T>(minBufferSize), minBufferSize);
        }

        UnmanagedMemoryBlock<T>[] cache = _cache;
        UnmanagedMemoryBlock<T> memoryBlock;
        if (_cacheIndex >= cache.Length)
        {
            return new UnmanagedMemoryOwner<T>(this, new UnmanagedMemoryBlock<T>(minBufferSize), minBufferSize);
        }

        lock (cache)
        {
            if (_cacheIndex >= cache.Length)
            {
                return new UnmanagedMemoryOwner<T>(this, new UnmanagedMemoryBlock<T>(minBufferSize), minBufferSize);
            }

            memoryBlock = cache[_cacheIndex];
            cache[_cacheIndex++] = null!;
        }

        if (memoryBlock == null)
        {
            memoryBlock = new UnmanagedMemoryBlock<T>(minBufferSize);
        }

        return new UnmanagedMemoryOwner<T>(this, memoryBlock, minBufferSize);
    }

    /// <summary>
    /// Return memory owner to pool.
    /// </summary>
    /// <param name="memoryBlock">The memory block to return.</param>
    internal void Return(UnmanagedMemoryBlock<T> memoryBlock)
    {
        lock (_cache)
        {
            if (_cacheIndex == 0 || memoryBlock.AllocatedSize != _memoryBlockSize)
            {
                memoryBlock.Dispose();
                return;
            }

            _cache[--_cacheIndex] = memoryBlock;
        }
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
