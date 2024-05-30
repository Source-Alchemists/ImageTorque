using System.Buffers;

namespace ImageTorque.Codecs.Png;

internal readonly record struct PngChunk
{
    public PngChunk(int length, PngChunkType type, IMemoryOwner<byte> data = null!)
    {
        Length = length;
        Type = type;
        Data = data;
    }

    public int Length { get; }

    public PngChunkType Type { get; }

    public IMemoryOwner<byte> Data { get; }

}
