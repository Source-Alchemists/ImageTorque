using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

[StructLayout(LayoutKind.Explicit, Size = 12)]
public record struct Rgb : IPackedPixel<Rgb>
{
    [FieldOffset(0)]
    public float Red;

    [FieldOffset(4)]
    public float Green;

    [FieldOffset(8)]
    public float Blue;

    public Rgb(float red, float green, float blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public PixelInfo PixelInfo
    {
        get
        {
            return new PixelInfo(PixelType.Rgb, 12, 3, 1, true);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rgb(ValueTuple<float, float, float> value)
    {
        return new Rgb(value.Item1, value.Item2, value.Item3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb24 ToRgb24()
    {
        return new Rgb24(Convert.ToByte(Red), Convert.ToByte(Green), Convert.ToByte(Blue));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb48 ToRgb48()
    {
        return new Rgb48(Convert.ToUInt16(Red), Convert.ToUInt16(Green), Convert.ToUInt16(Blue));
    }
}