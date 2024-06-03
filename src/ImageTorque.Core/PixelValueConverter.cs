using System.Runtime.CompilerServices;

namespace ImageTorque;

public static class PixelValueConverter
{
    /// <summary>
    /// Converts a byte value to a single-precision floating-point number.
    /// </summary>
    /// <param name="value">The byte value to convert.</param>
    /// <returns>The converted single-precision floating-point number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float ToSingle(byte value)
    {
        return value / 255f;
    }

    /// <summary>
    /// Converts a ushort value to a single-precision floating-point number.
    /// </summary>
    /// <param name="value">The ushort value to convert.</param>
    /// <returns>The converted single-precision floating-point number.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float ToSingle(ushort value)
    {
        return value / 65535f;
    }

    /// <summary>
    /// Converts a float value to a byte value.
    /// </summary>
    /// <param name="value">The float value to convert.</param>
    /// <returns>The byte value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte ToByte(float value)
    {
        return (byte)(value * 255f);
    }

    /// <summary>
    /// Converts a ushort value to a byte value.
    /// </summary>
    /// <param name="value">The ushort value to convert.</param>
    /// <returns>The converted byte value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte ToByte(ushort value)
    {
        return (byte)(((value * 255) + 32895) >> 16);
    }

    /// <summary>
    /// Converts a floating-point value to an unsigned 16-bit integer.
    /// </summary>
    /// <param name="value">The floating-point value to convert.</param>
    /// <returns>The converted unsigned 16-bit integer value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ushort ToUInt16(float value)
    {
        return (ushort)(value * 65535f);
    }

    /// <summary>
    /// Converts a byte value to a ushort value.
    /// </summary>
    /// <param name="value">The byte value to convert.</param>
    /// <returns>The converted ushort value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ushort ToUInt16(byte value)
    {
        return (ushort)(value * 257);
    }
}
