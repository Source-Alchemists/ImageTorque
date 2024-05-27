using System.Buffers;

namespace ImageTorque.Formats.Png;

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

    public bool IsCritical(PngCrcChunkHandling handling)
        => handling switch
        {
            PngCrcChunkHandling.IgnoreNone => true,
            PngCrcChunkHandling.IgnoreNonCritical => Type is PngChunkType.Header or PngChunkType.Palette or PngChunkType.Data,
            PngCrcChunkHandling.IgnoreData => Type is PngChunkType.Header or PngChunkType.Palette,
            _ => false,
        };
}
