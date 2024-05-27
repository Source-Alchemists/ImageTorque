using System.Buffers.Binary;

namespace ImageTorque.Formats.Png;

internal readonly struct PngPhysical(uint x, uint y, byte unitSpecifier)
{
    public const int Size = 9;

    public uint XAxisPixelsPerUnit { get; } = x;

    public uint YAxisPixelsPerUnit { get; } = y;

    public byte UnitSpecifier { get; } = unitSpecifier;

    public static PngPhysical Parse(ReadOnlySpan<byte> data)
    {
        uint hResolution = BinaryPrimitives.ReadUInt32BigEndian(data[..4]);
        uint vResolution = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(4, 4));
        byte unit = data[8];

        return new PngPhysical(hResolution, vResolution, unit);
    }
}
