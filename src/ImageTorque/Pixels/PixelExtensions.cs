using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

/// <summary>
/// Extension methods for pixel types.
/// </summary>
public static class PixelExtensions
{
    /// <summary>
    /// Converts a <see cref="Span{L8}"/> to a <see cref="Span{Byte}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> AsByte(this Span<L8> pixels) => MemoryMarshal.Cast<L8, byte>(pixels);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{L8}"/> to a <see cref="ReadOnlySpan{Byte}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsByte(this ReadOnlySpan<L8> pixels) => MemoryMarshal.Cast<L8, byte>(pixels);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{Rgb24}"/> to a <see cref="Span{Byte}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsByte(this ReadOnlySpan<Rgb24> pixels) => MemoryMarshal.Cast<Rgb24, byte>(pixels);

    /// <summary>
    /// Converts a <see cref="float"/> to a <see cref="Span{LS}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<LS> AsMono(this float[] values) => MemoryMarshal.Cast<float, LS>(values);

    /// <summary>
    /// Converts a <see cref="Span{Single}"/> to a <see cref="Span{LS}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<LS> AsMono(this Span<float> values) => MemoryMarshal.Cast<float, LS>(values);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{Single}"/> to a <see cref="ReadOnlySpan{LS}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<LS> AsMono(this ReadOnlySpan<float> values) => MemoryMarshal.Cast<float, LS>(values);

    /// <summary>
    /// Converts a <see cref="ushort"/> to a <see cref="ReadOnlySpan{L16}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<L16> AsMono16(this ReadOnlySpan<ushort> values) => MemoryMarshal.Cast<ushort, L16>(values);

    /// <summary>
    /// Converts a <see cref="ushort"/> to a <see cref="Span{L16}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L16> AsMono16(this ushort[] values) => MemoryMarshal.Cast<ushort, L16>(values);

    /// <summary>
    /// Converts a <see cref="Span{Long}"/> to a <see cref="Span{L16}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L16> AsMono16(this Span<ushort> values) => MemoryMarshal.Cast<ushort, L16>(values);

    /// <summary>
    /// Converts a <see cref="byte"/> to a <see cref="Span{L8}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L8> AsMono8(this byte[] values) => MemoryMarshal.Cast<byte, L8>(values);

    /// <summary>
    /// Converts a <see cref="Span{Byte}"/> to a <see cref="Span{L8}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L8> AsMono8(this Span<byte> values) => MemoryMarshal.Cast<byte, L8>(values);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{Byte}"/> to a <see cref="ReadOnlySpan{L8}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<L8> AsMono8(this ReadOnlySpan<byte> values) => MemoryMarshal.Cast<byte, L8>(values);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{Long}"/> to a <see cref="ReadOnlySpan{L16}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<L16> AsRgb161616(this ReadOnlySpan<ushort> values) => MemoryMarshal.Cast<ushort, L16>(values);

    /// <summary>
    /// Converts a <see cref="Span{Long}"/> to a <see cref="Span{L16}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L16> AsRgb161616(this Span<ushort> values) => MemoryMarshal.Cast<ushort, L16>(values);

    /// <summary>
    /// Converts a <see cref="ushort"/> to a <see cref="Span{L16}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L16> AsRgb161616(this ushort[] values) => MemoryMarshal.Cast<ushort, L16>(values);

    /// <summary>
    /// Converts a <see cref="byte"/> to a <see cref="Span{L8}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L8> AsRgb888(this byte[] values) => MemoryMarshal.Cast<byte, L8>(values);

    /// <summary>
    /// Converts a <see cref="Span{Byte}"/> to a <see cref="Span{L8}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L8> AsRgb888(this Span<byte> values) => MemoryMarshal.Cast<byte, L8>(values);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{Byte}"/> to a <see cref="ReadOnlySpan{L8}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<L8> AsRgb888(this ReadOnlySpan<byte> values) => MemoryMarshal.Cast<byte, L8>(values);

    /// <summary>
    /// Converts a <see cref="float"/> to a <see cref="Span{LS}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<LS> AsRgbFFF(this float[] values) => MemoryMarshal.Cast<float, LS>(values);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{Single}"/> to a <see cref="ReadOnlySpan{LS}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<LS> AsRgbFFF(this ReadOnlySpan<float> values) => MemoryMarshal.Cast<float, LS>(values);

    /// <summary>
    /// Converts a <see cref="Span{Single}"/> to a <see cref="Span{LS}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<LS> AsRgbFFF(this Span<float> values) => MemoryMarshal.Cast<float, LS>(values);

    /// <summary>
    /// Converts a <see cref="Span{LS}"/> to a <see cref="Span{Single}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<float> AsSingle(this Span<LS> pixels) => MemoryMarshal.Cast<LS, float>(pixels);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{LS}"/> to a <see cref="ReadOnlySpan{Single}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<float> AsSingle(this ReadOnlySpan<LS> pixels) => MemoryMarshal.Cast<LS, float>(pixels);

    /// <summary>
    /// Converts a <see cref="Span{L16}"/> to a <see cref="Span{Long}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<ushort> AsUInt16(this Span<L16> pixels) => MemoryMarshal.Cast<L16, ushort>(pixels);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{L16}"/> to a <see cref="ReadOnlySpan{Long}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<ushort> AsUInt16(this ReadOnlySpan<L16> pixels) => MemoryMarshal.Cast<L16, ushort>(pixels);

    /// <summary>
    /// Converts a <see cref="Span{Rgb}"/> to a <see cref="Span{Vector3}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Vector3> AsVector3(this Span<Rgb> pixels) => MemoryMarshal.Cast<Rgb, Vector3>(pixels);

    /// <summary>
    /// Converts a <see cref="ReadOnlySpan{Rgb}"/> to a <see cref="ReadOnlySpan{Vector3}"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Vector3> AsVector3(this ReadOnlySpan<Rgb> pixels) => MemoryMarshal.Cast<Rgb, Vector3>(pixels);
}
