using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

/// <summary>
/// Represents a single-precision grayscale pixel.
/// </summary>
public record struct LS : IL1Pixel<float>
{
    /// <summary>
    /// Gets the white value.
    /// </summary>
    public const float White = 1f;

    /// <summary>
    /// Gets the black value.
    /// </summary>
    public const float Black = 0f;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public float Min { get => Black; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public float Max { get => White; }

    /// <summary>
    /// Gets the pixel type.
    /// </summary>
    public PixelType PixelType => PixelType.LF;

    /// <summary>
    /// Initializes a new instance of the <see cref="LS"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    public LS(float value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LS"/> struct.
    /// </summary>
    /// <param name="mono">The mono.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator float(LS mono)
    {
        return mono.Value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LS"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator LS(float value)
    {
        return new LS(value);
    }

    /// <summary>
    /// Converts to <see cref="L8"/>.
    /// </summary>
    /// <returns>The <see cref="L8"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public L8 ToL8()
    {
        return new L8(PixelValueConverter.ToByte(Value));
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
