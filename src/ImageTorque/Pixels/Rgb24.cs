using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

[StructLayout(LayoutKind.Explicit, Size = 3)]
public record struct Rgb24 : IL3Pixel<byte>
{
    [FieldOffset(0)]
    public byte Red;

    [FieldOffset(1)]
    public byte Green;

    [FieldOffset(2)]
    public byte Blue;

    public byte R
    {
        get { return Red; }
        set { Red = value; }
    }

    public byte G
    {
        get { return Green; }
        set { Green = value; }
    }

    public byte B
    {
        get { return Blue; }
        set { Blue = value; }
    }

    public PixelType PixelType => PixelType.Rgb24;

    public Rgb24(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rgb24(ValueTuple<byte, byte, byte> value)
    {
        return new Rgb24(value.Item1, value.Item2, value.Item3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb ToRgb()
    {
        return new Rgb(Convert.ToSingle(Red), Convert.ToSingle(Green), Convert.ToSingle(Blue));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb48 ToRgb48()
    {
        return new Rgb48(Convert.ToUInt16(Red), Convert.ToUInt16(Green), Convert.ToUInt16(Blue));
    }
}
