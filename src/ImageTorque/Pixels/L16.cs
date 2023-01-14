using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct L16 : IL1Pixel<ushort>
{
    public const ushort White = 65535;

    public const ushort Black = 0;

    public ushort Value { get; set; }

    public L16(ushort value)
    {
        Value = value;
    }

    public PixelInfo PixelInfo
    {
        get
        {
            return new PixelInfo(PixelType.Mono16, 2, 1, 1, false);
        }
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
    public LF ToMono()
    {
        return new LF(Convert.ToSingle(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L8 ToMono8()
    {
        return new L8(Convert.ToByte(Value));
    }
}
