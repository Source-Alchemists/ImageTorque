using System.Buffers.Binary;

namespace ImageTorque.Formats.Png;

internal readonly struct PngHeader(
    int width, int height, byte bitDepth, PngColorType colorType,
    byte compressionMethod, byte filterMethod, PngInterlaceMode interlaceMethod)
{
    public const int Size = 13;

    public int Width { get; } = width;

    public int Height { get; } = height;

    public byte BitDepth { get; } = bitDepth;

    public PngColorType ColorType { get; } = colorType;

    public byte CompressionMethod { get; } = compressionMethod;

    public byte FilterMethod { get; } = filterMethod;

    public PngInterlaceMode InterlaceMethod { get; } = interlaceMethod;

    public void Validate()
    {
        if (!PngConstants.ColorTypes.TryGetValue(ColorType, out byte[]? supportedBitDepths))
        {
            throw new NotSupportedException($"Unsupported color type '{ColorType}'.");
        }

        if (supportedBitDepths.AsSpan().IndexOf(BitDepth) == -1)
        {
            throw new NotSupportedException($"Unsupported bit depth '{BitDepth}'.");
        }

        if (FilterMethod != 0)
        {
            throw new NotSupportedException($"Invalid filter method '{FilterMethod}'.");
        }

        if (InterlaceMethod is not PngInterlaceMode.None and not PngInterlaceMode.Adam7)
        {
            throw new NotSupportedException($"Invalid interlace method '{InterlaceMethod}'.");
        }
    }

    // public void WriteTo(Span<byte> buffer)
    // {
    //     BinaryPrimitives.WriteInt32BigEndian(buffer[..4], Width);
    //     BinaryPrimitives.WriteInt32BigEndian(buffer.Slice(4, 4), Height);

    //     buffer[8] = BitDepth;
    //     buffer[9] = (byte)ColorType;
    //     buffer[10] = CompressionMethod;
    //     buffer[11] = FilterMethod;
    //     buffer[12] = (byte)InterlaceMethod;
    // }

    public static PngHeader Parse(ReadOnlySpan<byte> data)
        => new(
            width: BinaryPrimitives.ReadInt32BigEndian(data[..4]),
            height: BinaryPrimitives.ReadInt32BigEndian(data.Slice(4, 4)),
            bitDepth: data[8],
            colorType: (PngColorType)data[9],
            compressionMethod: data[10],
            filterMethod: data[11],
            interlaceMethod: (PngInterlaceMode)data[12]);
}
