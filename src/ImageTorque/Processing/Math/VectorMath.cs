using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace ImageTorque;

internal static class VectorMath
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max) => Vector4.Min(Vector4.Max(value, min), max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int EvenReduceSum(Vector128<int> accumulator)
    {
        Vector128<int> vsum = Sse2.Add(accumulator, Sse2.Shuffle(accumulator, 0b_11_10_11_10));
        return Sse2.ConvertToInt32(vsum);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int EvenReduceSum(Vector256<int> accumulator)
    {
        Vector128<int> vsum = Sse2.Add(accumulator.GetLower(), accumulator.GetUpper());
        vsum = Sse2.Add(vsum, Sse2.Shuffle(vsum, 0b_11_10_11_10));
        return Sse2.ConvertToInt32(vsum);
    }
}
