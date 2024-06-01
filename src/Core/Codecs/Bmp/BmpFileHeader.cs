using System.Runtime.InteropServices;

namespace ImageTorque.Codecs.Bmp;

/// <summary>
/// <see href="https://en.wikipedia.org/wiki/BMP_file_format"/>
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal readonly record struct BmpFileHeader
{
    public const int Size = 14;
    public short Type { get; }
    public int FileSize { get; }
    public int Reserved { get; }
    public int Offset { get; }

    public BmpFileHeader(short type, int fileSize, int reserved, int offset)
    {
        Type = type;
        FileSize = fileSize;
        Reserved = reserved;
        Offset = offset;
    }

    public static BmpFileHeader Parse(ReadOnlySpan<byte> data) => MemoryMarshal.Cast<byte, BmpFileHeader>(data)[0];
}
