using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct L16 : IL1Pixel<ushort>
{
    public const ushort White = 65535;

    public const ushort Black = 0;

    public ushort Value { get; set; }

    public ushort Min { get => Black; }

    public ushort Max { get => White; }

    public PixelType PixelType => PixelType.L16;

    public L16(ushort value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator L16(ushort value)
    {
        return new L16(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ushort(L16 mono)
    {
        return mono.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LS ToLF()
    {
        return new LS(Convert.ToSingle(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L8 ToL8()
    {
        return new L8(Convert.ToByte(Value));
    }
}
