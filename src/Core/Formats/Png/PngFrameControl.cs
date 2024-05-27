using System.Buffers.Binary;

namespace ImageTorque.Formats.Png;

internal readonly struct FrameControl(
    uint sequenceNumber,
    uint width,
    uint height,
    uint xOffset,
    uint yOffset,
    ushort delayNumerator,
    ushort delayDenominator)
{
    public const int Size = 26;

    public FrameControl(uint width, uint height) : this(0, width, height, 0, 0, 0, 0)
    {
    }

    public uint SequenceNumber { get; } = sequenceNumber;

    public uint Width { get; } = width;

    public uint Height { get; } = height;

    public uint XOffset { get; } = xOffset;

    public uint YOffset { get; } = yOffset;

    public uint XMax => XOffset + Width;

    public uint YMax => YOffset + Height;

    public ushort DelayNumerator { get; } = delayNumerator;

    public ushort DelayDenominator { get; } = delayDenominator;

    public Rectangle Bounds => new((int)XOffset, (int)YOffset, (int)Width, (int)Height);

    public void Validate(PngHeader header)
    {
        if (Width == 0 || Height == 0)
        {
            throw new InvalidDataException("Excpected dimension > 0");
        }

        if (XMax > header.Width || YMax > header.Height)
        {
            throw new InvalidDataException("Offset and dimension out of range");
        }
    }

    public void WriteTo(Span<byte> buffer)
    {
        BinaryPrimitives.WriteUInt32BigEndian(buffer[..4], SequenceNumber);
        BinaryPrimitives.WriteUInt32BigEndian(buffer[4..8], Width);
        BinaryPrimitives.WriteUInt32BigEndian(buffer[8..12], Height);
        BinaryPrimitives.WriteUInt32BigEndian(buffer[12..16], XOffset);
        BinaryPrimitives.WriteUInt32BigEndian(buffer[16..20], YOffset);
        BinaryPrimitives.WriteUInt16BigEndian(buffer[20..22], DelayNumerator);
        BinaryPrimitives.WriteUInt16BigEndian(buffer[22..24], DelayDenominator);
    }

    public static FrameControl Parse(ReadOnlySpan<byte> data)
        => new(
            sequenceNumber: BinaryPrimitives.ReadUInt32BigEndian(data[..4]),
            width: BinaryPrimitives.ReadUInt32BigEndian(data[4..8]),
            height: BinaryPrimitives.ReadUInt32BigEndian(data[8..12]),
            xOffset: BinaryPrimitives.ReadUInt32BigEndian(data[12..16]),
            yOffset: BinaryPrimitives.ReadUInt32BigEndian(data[16..20]),
            delayNumerator: BinaryPrimitives.ReadUInt16BigEndian(data[20..22]),
            delayDenominator: BinaryPrimitives.ReadUInt16BigEndian(data[22..24]));
}
