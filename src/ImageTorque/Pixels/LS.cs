using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct LS : IL1Pixel<float>
{
    public const float White = 1f;

    public const float Black = 0f;

    public float Value { get; set; }

    public float Min { get => Black; }

    public float Max { get => White; }

    public PixelType PixelType => PixelType.LF;

    public LS(float value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(LS mono)
    {
        return mono.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator LS(float value)
    {
        return new LS(value);
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
