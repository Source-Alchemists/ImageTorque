using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

/// <summary>
/// Represents a 8-bit grayscale pixel.
/// </summary>
public record struct L8 : IL1Pixel<byte>
{
    /// <summary>
    /// Gets the white value.
    /// </summary>
    public const byte White = byte.MaxValue;

    /// <summary>
    /// Gets the black value.
    /// </summary>
    public const byte Black = byte.MinValue;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public byte Value { get; set; }

    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public byte Min { get => Black; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public byte Max { get => White; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    public PixelType PixelType => PixelType.L8;

    /// <summary>
    /// Initializes a new instance of the <see cref="L8"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public L8(byte value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="L8"/> struct.
    /// </summary>
    /// <param name="mono">The mono.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte(L8 mono)
    {
        return mono.Value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="L8"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator L8(byte value)
    {
        return new L8(value);
    }

    /// <summary>
    /// Converts to <see cref="LS"/>.
    /// </summary>
    /// <returns>The <see cref="LS"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public LS ToLS()
    {
        return new LS(PixelValueConverter.ToSingle(Value));
    }

    /// <summary>
    /// Converts to <see cref="L16"/>.
    /// </summary>
    /// <returns>The <see cref="L16"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L16 ToL16()
    {
        return new L16(PixelValueConverter.ToUInt16(Value));
    }
}
