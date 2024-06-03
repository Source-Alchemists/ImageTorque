using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Memory;

internal sealed unsafe class UnmanagedMemoryBlock<T> : IDisposable where T : unmanaged
{
    private readonly byte* _pointer;
    private bool _isDisposed = false;

    public int AllocatedSize { get; }


    public UnmanagedMemoryBlock(int minBufferSize)
    {
        int toAlloc = checked(minBufferSize * sizeof(T));
        AllocatedSize = toAlloc;
        _pointer = Alloc(toAlloc);
    }

    public T* AsPointer()
    {
        var span = new Span<byte>(_pointer, AllocatedSize);
        return (T*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(span));
    }

    public Span<T> AsSpan()
    {
        return new Span<T>(AsPointer(), AllocatedSize);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if(disposing && !_isDisposed)
        {
            NativeMemory.Free(_pointer);
            _isDisposed = true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte* Alloc(int size)
    {
        void* newBlock = NativeMemory.AllocZeroed((nuint)(size + sizeof(void*)));
        return (byte*)newBlock;
    }
}
