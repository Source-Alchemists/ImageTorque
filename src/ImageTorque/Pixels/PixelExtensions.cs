using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Pixels;

public static class PixelExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> AsByte(this Span<L8> pixels) => MemoryMarshal.Cast<L8, byte>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsByte(this ReadOnlySpan<L8> pixels) => MemoryMarshal.Cast<L8, byte>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<byte> AsByte(this Span<Rgb888> pixels) => MemoryMarshal.Cast<Rgb888, byte>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsByte(this ReadOnlySpan<Rgb888> pixels) => MemoryMarshal.Cast<Rgb888, byte>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<byte> AsByte(this ReadOnlySpan<Rgb24> pixels) => MemoryMarshal.Cast<Rgb24, byte>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<LF> AsMono(this float[] values) => MemoryMarshal.Cast<float, LF>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<LF> AsMono(this Span<float> values) => MemoryMarshal.Cast<float, LF>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<LF> AsMono(this ReadOnlySpan<float> values) => MemoryMarshal.Cast<float, LF>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<L16> AsMono16(this ReadOnlySpan<ushort> values) => MemoryMarshal.Cast<ushort, L16>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L16> AsMono16(this ushort[] values) => MemoryMarshal.Cast<ushort, L16>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L16> AsMono16(this Span<ushort> values) => MemoryMarshal.Cast<ushort, L16>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L8> AsMono8(this byte[] values) => MemoryMarshal.Cast<byte, L8>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<L8> AsMono8(this Span<byte> values) => MemoryMarshal.Cast<byte, L8>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<L8> AsMono8(this ReadOnlySpan<byte> values) => MemoryMarshal.Cast<byte, L8>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Rgb161616> AsRgb161616(this ReadOnlySpan<ushort> values) => MemoryMarshal.Cast<ushort, Rgb161616>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Rgb161616> AsRgb161616(this Span<ushort> values) => MemoryMarshal.Cast<ushort, Rgb161616>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Rgb161616> AsRgb161616(this ushort[] values) => MemoryMarshal.Cast<ushort, Rgb161616>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Rgb888> AsRgb888(this byte[] values) => MemoryMarshal.Cast<byte, Rgb888>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Rgb888> AsRgb888(this Span<byte> values) => MemoryMarshal.Cast<byte, Rgb888>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Rgb888> AsRgb888(this ReadOnlySpan<byte> values) => MemoryMarshal.Cast<byte, Rgb888>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<RgbFFF> AsRgbFFF(this float[] values) => MemoryMarshal.Cast<float, RgbFFF>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<RgbFFF> AsRgbFFF(this ReadOnlySpan<float> values) => MemoryMarshal.Cast<float, RgbFFF>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<RgbFFF> AsRgbFFF(this Span<float> values) => MemoryMarshal.Cast<float, RgbFFF>(values);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<float> AsSingle(this Span<LF> pixels) => MemoryMarshal.Cast<LF, float>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<float> AsSingle(this Span<RgbFFF> pixels) => MemoryMarshal.Cast<RgbFFF, float>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<float> AsSingle(this ReadOnlySpan<LF> pixels) => MemoryMarshal.Cast<LF, float>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<float> AsSingle(this ReadOnlySpan<RgbFFF> pixels) => MemoryMarshal.Cast<RgbFFF, float>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<ushort> AsUInt16(this Span<Rgb161616> pixels) => MemoryMarshal.Cast<Rgb161616, ushort>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<ushort> AsUInt16(this ReadOnlySpan<Rgb161616> pixels) => MemoryMarshal.Cast<Rgb161616, ushort>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<ushort> AsUInt16(this Span<L16> pixels) => MemoryMarshal.Cast<L16, ushort>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<ushort> AsUInt16(this ReadOnlySpan<L16> pixels) => MemoryMarshal.Cast<L16, ushort>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Span<Vector3> AsVector3(this Span<Rgb> pixels) => MemoryMarshal.Cast<Rgb, Vector3>(pixels);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReadOnlySpan<Vector3> AsVector3(this ReadOnlySpan<Rgb> pixels) => MemoryMarshal.Cast<Rgb, Vector3>(pixels);
}
