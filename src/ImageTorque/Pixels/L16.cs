using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

/// <summary>
/// Represents a 16-bit grayscale pixel.
/// </summary>
public record struct L16 : IL1Pixel<ushort>
{
    /// <summary>
    /// Gets the white value.
    /// </summary>
    public const ushort White = 65535;

    /// <summary>
    /// Gets the black value.
    /// </summary>
    public const ushort Black = 0;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public ushort Value { get; set; }

    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public ushort Min { get => Black; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public ushort Max { get => White; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    public PixelType PixelType => PixelType.L16;

    /// <summary>
    /// Initializes a new instance of the <see cref="L16"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public L16(ushort value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="L16"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator L16(ushort value)
    {
        return new L16(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="L16"/> struct.
    /// </summary>
    /// <param name="mono">The mono value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator ushort(L16 mono)
    {
        return mono.Value;
    }

    /// <summary>
    /// Converts the pixel to a <see cref="LS"/> pixel.
    /// </summary>
    /// <returns>The <see cref="LS"/> pixel.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LS ToLS()
    {
        return new LS(PixelValueConverter.ToSingle(Value));
    }

    /// <summary>
    /// Converts the pixel to a <see cref="L8"/> pixel.
    /// </summary>
    /// <returns>The <see cref="L8"/> pixel.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L8 ToL8()
    {
        return new L8(PixelValueConverter.ToByte(Value));
    }
}
