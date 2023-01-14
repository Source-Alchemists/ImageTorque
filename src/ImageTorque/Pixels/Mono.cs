using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct Mono : IPackedL1Pixel<float>
{
    public const float White = 1f;

    public const float Black = 0f;

    public float Value { get; set; }

    public Mono(float value)
    {
        Value = value;
    }

    public PixelInfo PixelInfo
    {
        get
        {
            return new PixelInfo(PixelType.Mono, 4, 1, 1, false);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(Mono mono)
    {
        return mono.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Mono(float value)
    {
        return new Mono(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mono8 ToMono8()
    {
        return new Mono8(Convert.ToByte(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mono16 ToMono16()
    {
        return new Mono16(Convert.ToUInt16(Value));
    }
}
