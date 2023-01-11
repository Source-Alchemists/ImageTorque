using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

[StructLayout(LayoutKind.Explicit, Size = 6)]
public record struct Rgb48 : IPackedPixel<Rgb48>
{
    [FieldOffset(0)]
    public ushort Red;

    [FieldOffset(2)]
    public ushort Green;

    [FieldOffset(4)]
    public ushort Blue;

    public Rgb48(ushort red, ushort green, ushort blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    public PixelInfo PixelInfo
    {
        get
        {
            return new PixelInfo(PixelType.Rgb48, 6, 3, 1, true);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rgb48(ValueTuple<ushort, ushort, ushort> value)
    {
        return new Rgb48(value.Item1, value.Item2, value.Item3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb ToRgb()
    {
        return new Rgb(Convert.ToSingle(Red), Convert.ToSingle(Green), Convert.ToSingle(Blue));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb24 ToRgb24()
    {
        return new Rgb24(Convert.ToByte(Red), Convert.ToByte(Green), Convert.ToByte(Blue));
    }
}
