using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

[StructLayout(LayoutKind.Explicit, Size = 6)]
public record struct Rgb48 : IL3Pixel<ushort>
{
    [FieldOffset(0)]
    public ushort Red;

    [FieldOffset(2)]
    public ushort Green;

    [FieldOffset(4)]
    public ushort Blue;

    public ushort R
    {
        get { return Red; }
        set { Red = value; }
    }

    public ushort G
    {
        get { return Green; }
        set { Green = value; }
    }

    public ushort B
    {
        get { return Blue; }
        set { Blue = value; }
    }

    public PixelType PixelType => PixelType.Rgb48;

    public Rgb48(ushort red, ushort green, ushort blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
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
