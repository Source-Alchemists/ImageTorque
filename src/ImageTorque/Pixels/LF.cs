using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct LF : IL1Pixel<float>
{
    public const float White = 1f;

    public const float Black = 0f;

    public float Value { get; set; }

    public LF(float value)
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
    public static implicit operator float(LF mono)
    {
        return mono.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator LF(float value)
    {
        return new LF(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L8 ToMono8()
    {
        return new L8(Convert.ToByte(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L16 ToMono16()
    {
        return new L16(Convert.ToUInt16(Value));
    }
}
