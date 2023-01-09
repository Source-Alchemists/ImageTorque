using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct RgbFFF : IPlanarPixel<RgbFFF>
{
    public float Value;

    public RgbFFF(float value)
    {
        Value = value;
    }

    public PixelInfo PixelInfo
    {
        get
        {
            return new PixelInfo(PixelType.RgbFFF, 12, 1, 3, true);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(RgbFFF value)
    {
        return value.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator RgbFFF(float value)
    {
        return new RgbFFF(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb888 ToRgb888()
    {
        return new Rgb888(Convert.ToByte(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb161616 ToRgb161616()
    {
        return new Rgb161616(Convert.ToUInt16(Value));
    }
}