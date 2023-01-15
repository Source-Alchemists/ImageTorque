using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct LF : IL1Pixel<float>
{
    public const float White = 1f;

    public const float Black = 0f;

    public float Value { get; set; }

    public float Min { get => Black; }

    public float Max { get => White; }

    public PixelType PixelType => PixelType.LF;

    public LF(float value)
    {
        Value = value;
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
    public L8 ToL8()
    {
        return new L8(Convert.ToByte(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L16 ToL16()
    {
        return new L16(Convert.ToUInt16(Value));
    }
}
