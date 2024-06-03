using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

/// <summary>
/// A single-precision floating point RGB pixel.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 12)]
public record struct Rgb : IL3Pixel<float>
{
    /// <summary>
    /// Gets the red component.
    /// </summary>
    [FieldOffset(0)]
    public float Red;

    /// <summary>
    /// Gets the green component.
    /// </summary>
    [FieldOffset(4)]
    public float Green;

    /// <summary>
    /// Gets the blue component.
    /// </summary>
    [FieldOffset(8)]
    public float Blue;

    /// <summary>
    /// Gets or sets the red component.
    /// </summary>
    public float R
    {
        get { return Red; }
        set { Red = value; }
    }

    /// <summary>
    /// Gets or sets the green component.
    /// </summary>
    public float G
    {
        get { return Green; }
        set { Green = value; }
    }

    /// <summary>
    /// Gets or sets the blue component.
    /// </summary>
    public float B
    {
        get { return Blue; }
        set { Blue = value; }
    }

    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public float Min { get => 0f; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public float Max { get => 1f; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    public PixelType PixelType => PixelType.Rgb;

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgb"/> struct.
    /// </summary>
    /// <param name="red">The red component.</param>
    /// <param name="green">The green component.</param>
    /// <param name="blue">The blue component.</param>
    public Rgb(float red, float green, float blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Rgb"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Rgb(ValueTuple<float, float, float> value)
    {
        return new Rgb(value.Item1, value.Item2, value.Item3);
    }

    /// <summary>
    /// Converts the pixel to a <see cref="Rgb24"/> pixel.
    /// </summary>
    /// <returns>The <see cref="Rgb24"/> pixel.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rgb24 ToRgb24()
    {
        return new Rgb24(PixelValueConverter.ToByte(Red), PixelValueConverter.ToByte(Green), PixelValueConverter.ToByte(Blue));
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
