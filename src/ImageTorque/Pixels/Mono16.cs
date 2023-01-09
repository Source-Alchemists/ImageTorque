using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct Mono16 : IPackedPixel<Mono16>
{
    public const ushort White = 65535;
    
    public const ushort Black = 0;

    public ushort Value;

    public Mono16(ushort value)
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
    public static implicit operator Mono16(ushort value)
    {
        return new Mono16(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ushort(Mono16 mono)
    {
        return mono.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mono ToMono()
    {
        return new Mono(Convert.ToSingle(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mono8 ToMono8()
    {
        return new Mono8(Convert.ToByte(Value));
    }
}