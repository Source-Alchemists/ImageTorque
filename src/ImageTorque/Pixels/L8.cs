using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct L8 : IL1Pixel<byte>
{
    public const byte White = 255;

    public const byte Black = 0;

    public byte Value { get; set; }

    public byte Min { get => Black; }

    public byte Max { get => White; }

    public PixelType PixelType => PixelType.L8;

    public L8(byte value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte(L8 mono)
    {
        return mono.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator L8(byte value)
    {
        return new L8(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LF ToLF()
    {
        return new LF(Convert.ToSingle(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L16 ToL16()
    {
        return new L16(Convert.ToUInt16(Value));
    }
}
