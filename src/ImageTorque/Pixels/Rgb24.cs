using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

/// <summary>
/// A 24-bit RGB pixel.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 3)]
public record struct Rgb24 : IL3Pixel<byte>
{
    /// <summary>
    /// Gets the red component.
    /// </summary>
    [FieldOffset(0)]
    public byte Red;

    /// <summary>
    /// Gets the green component.
    /// </summary>
    [FieldOffset(1)]
    public byte Green;

    /// <summary>
    /// Gets the blue component.
    /// </summary>
    [FieldOffset(2)]
    public byte Blue;

    /// <summary>
    /// Gets or sets the red component.
    /// </summary>
    public byte R
    {
        get { return Red; }
        set { Red = value; }
    }

    /// <summary>
    /// Gets or sets the green component.
    /// </summary>
    public byte G
    {
        get { return Green; }
        set { Green = value; }
    }

    /// <summary>
    /// Gets or sets the blue component.
    /// </summary>
    public byte B
    {
        get { return Blue; }
        set { Blue = value; }
    }

    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public byte Min { get => byte.MinValue; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public byte Max { get => byte.MaxValue; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    public PixelType PixelType => PixelType.Rgb24;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgb24"/> struct.
    /// </summary>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    public Rgb24(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgb24"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rgb24(ValueTuple<byte, byte, byte> value)
    {
        return new Rgb24(value.Item1, value.Item2, value.Item3);
    }

    /// <summary>
    /// Converts the pixel to a <see cref="Rgb"/> struct.
    /// </summary>
    /// <returns>The <see cref="Rgb"/> struct.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb ToRgb()
    {
        return new Rgb(PixelValueConverter.ToSingle(Red), PixelValueConverter.ToSingle(Green), PixelValueConverter.ToSingle(Blue));
    }

    /// <summary>
    /// Converts the pixel to a <see cref="Rgb48"/> pixel.
    /// </summary>
    /// <returns>The <see cref="Rgb48"/> pixel.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb48 ToRgb48()
    {
        return new Rgb48(PixelValueConverter.ToUInt16(Red), PixelValueConverter.ToUInt16(Green), PixelValueConverter.ToUInt16(Blue));
    }
}
