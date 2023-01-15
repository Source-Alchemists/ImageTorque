using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

/// <summary>
/// Represents a 48-bit RGB pixel.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 6)]
public record struct Rgb48 : IL3Pixel<ushort>
{
    /// <summary>
    /// Gets the red component.
    /// </summary>
    [FieldOffset(0)]
    public ushort Red;

    /// <summary>
    /// Gets the green component.
    /// </summary>
    [FieldOffset(2)]
    public ushort Green;

    /// <summary>
    /// Gets the blue component.
    /// </summary>
    [FieldOffset(4)]
    public ushort Blue;

    /// <summary>
    /// Gets or sets the red component.
    /// </summary>
    public ushort R
    {
        get { return Red; }
        set { Red = value; }
    }

    /// <summary>
    /// Gets or sets the green component.
    /// </summary>
    public ushort G
    {
        get { return Green; }
        set { Green = value; }
    }

    /// <summary>
    /// Gets or sets the blue component.
    /// </summary>
    public ushort B
    {
        get { return Blue; }
        set { Blue = value; }
    }

    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public ushort Min { get => ushort.MinValue; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public ushort Max { get => ushort.MaxValue; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    public PixelType PixelType => PixelType.Rgb48;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgb48"/> struct.
    /// </summary>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    public Rgb48(ushort red, ushort green, ushort blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgb48"/> struct.
    /// </summary>
    /// <param name="value">The value to assign to all components.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rgb48(ValueTuple<ushort, ushort, ushort> value)
    {
        return new Rgb48(value.Item1, value.Item2, value.Item3);
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
    /// Converts the pixel to a <see cref="Rgb24"/> struct.
    /// </summary>
    /// <returns>The <see cref="Rgb24"/> struct.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb24 ToRgb24()
    {
        return new Rgb24(PixelValueConverter.ToByte(Red), PixelValueConverter.ToByte(Green), PixelValueConverter.ToByte(Blue));
    }
}
