using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct Rgb888 : IPlanarPixel<Rgb888>
{
    public byte Value;

    public Rgb888(byte value)
    {
        Value = value;
    }

    public PixelInfo PixelInfo
    {
        get
        {
            return new PixelInfo(PixelType.Rgb888, 3, 1, 3, true);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte(Rgb888 value)
    {
        return value.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rgb888(byte value)
    {
        return new Rgb888(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb161616 ToRgb161616()
    {
        return new Rgb161616(Convert.ToUInt16(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public RgbFFF ToRgbFFF()
    {
        return new RgbFFF(Convert.ToSingle(Value));
    }
}