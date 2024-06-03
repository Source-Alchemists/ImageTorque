using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImageTorque;

internal static class NumericMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Accumulate(ref Vector<uint> accumulator, Vector<byte> values)
    {
        Vector.Widen(values, out Vector<ushort> shortLow, out Vector<ushort> shortHigh);

        Vector.Widen(shortLow, out Vector<uint> intLow, out Vector<uint> intHigh);
        accumulator += intLow;
        accumulator += intHigh;

        Vector.Widen(shortHigh, out intLow, out intHigh);
        accumulator += intLow;
        accumulator += intHigh;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Average(byte left, byte above) => (left + above) >> 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetColorCountForBitDepth(int bitDepth) => 1 << bitDepth;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int x)
    {
        int y = x >> 31;
        return (x ^ y) - y;
    }
}
