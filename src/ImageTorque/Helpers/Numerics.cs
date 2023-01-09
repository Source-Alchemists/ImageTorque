using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace ImageTorque;

internal static class Numerics
{
    private const int ShuffleAlphaControl = 0b_11_11_11_11;

    public const int BlendAlphaControl = 0b_10_00_10_00;
    public const float Epsilon = 0.001F;
    public static readonly float EpsilonSquared = Epsilon * Epsilon;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GreatestCommonDivisor(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LeastCommonMultiple(int a, int b) => a / GreatestCommonDivisor(a, b) * b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Modulo2(int x) => x & 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Modulo4(int x) => x & 3;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Modulo8(int x) => x & 7;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint Modulo8(nint x) => x & 7;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ModuloP2(int x, int m) => x & (m - 1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Abs(int x)
    {
        int y = x >> 31;
        return (x ^ y) - y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Pow2(float x) => x * x;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Pow3(float x) => x * x * x;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Gaussian(float x, float sigma)
    {
        const float numerator = 1.0f;
        float denominator = MathF.Sqrt(2 * MathF.PI) * sigma;

        float exponentNumerator = -x * x;
        float exponentDenominator = 2 * Pow2(sigma);

        float left = numerator / denominator;
        float right = MathF.Exp(exponentNumerator / exponentDenominator);

        return left * right;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SinC(float f)
    {
        if (MathF.Abs(f) > Epsilon)
        {
            f *= MathF.PI;
            float result = MathF.Sin(f) / f;
            return MathF.Abs(result) < Epsilon ? 0F : result;
        }

        return 1F;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Clamp(byte value, byte min, byte max)
    {
        if (value > max)
        {
            return max;
        }

        if (value < min)
        {
            return min;
        }

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Clamp(uint value, uint min, uint max)
    {
        if (value > max)
        {
            return max;
        }

        if (value < min)
        {
            return min;
        }

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Clamp(int value, int min, int max)
    {
        if (value > max)
        {
            return max;
        }

        if (value < min)
        {
            return min;
        }

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Clamp(float value, float min, float max)
    {
        if (value > max)
        {
            return max;
        }

        if (value < min)
        {
            return min;
        }

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Clamp(double value, double min, double max)
    {
        if (value > max)
        {
            return max;
        }

        if (value < min)
        {
            return min;
        }

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4 Clamp(Vector4 value, Vector4 min, Vector4 max) => Vector4.Min(Vector4.Max(value, min), max);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(Span<byte> span, byte min, byte max)
    {
        Span<byte> remainder = span[ClampReduce(span, min, max)..];

        if (remainder.Length > 0)
        {
            ref byte remainderStart = ref MemoryMarshal.GetReference(remainder);
            ref byte remainderEnd = ref Unsafe.Add(ref remainderStart, remainder.Length);

            while (Unsafe.IsAddressLessThan(ref remainderStart, ref remainderEnd))
            {
                remainderStart = Clamp(remainderStart, min, max);

                remainderStart = ref Unsafe.Add(ref remainderStart, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(Span<uint> span, uint min, uint max)
    {
        Span<uint> remainder = span[ClampReduce(span, min, max)..];

        if (remainder.Length > 0)
        {
            ref uint remainderStart = ref MemoryMarshal.GetReference(remainder);
            ref uint remainderEnd = ref Unsafe.Add(ref remainderStart, remainder.Length);

            while (Unsafe.IsAddressLessThan(ref remainderStart, ref remainderEnd))
            {
                remainderStart = Clamp(remainderStart, min, max);

                remainderStart = ref Unsafe.Add(ref remainderStart, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(Span<int> span, int min, int max)
    {
        Span<int> remainder = span[ClampReduce(span, min, max)..];

        if (remainder.Length > 0)
        {
            ref int remainderStart = ref MemoryMarshal.GetReference(remainder);
            ref int remainderEnd = ref Unsafe.Add(ref remainderStart, remainder.Length);

            while (Unsafe.IsAddressLessThan(ref remainderStart, ref remainderEnd))
            {
                remainderStart = Clamp(remainderStart, min, max);

                remainderStart = ref Unsafe.Add(ref remainderStart, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(Span<float> span, float min, float max)
    {
        Span<float> remainder = span[ClampReduce(span, min, max)..];

        if (remainder.Length > 0)
        {
            ref float remainderStart = ref MemoryMarshal.GetReference(remainder);
            ref float remainderEnd = ref Unsafe.Add(ref remainderStart, remainder.Length);

            while (Unsafe.IsAddressLessThan(ref remainderStart, ref remainderEnd))
            {
                remainderStart = Clamp(remainderStart, min, max);

                remainderStart = ref Unsafe.Add(ref remainderStart, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Clamp(Span<double> span, double min, double max)
    {
        Span<double> remainder = span[ClampReduce(span, min, max)..];

        if (remainder.Length > 0)
        {
            ref double remainderStart = ref MemoryMarshal.GetReference(remainder);
            ref double remainderEnd = ref Unsafe.Add(ref remainderStart, remainder.Length);

            while (Unsafe.IsAddressLessThan(ref remainderStart, ref remainderEnd))
            {
                remainderStart = Clamp(remainderStart, min, max);

                remainderStart = ref Unsafe.Add(ref remainderStart, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int ClampReduce<T>(Span<T> span, T min, T max)
        where T : unmanaged
    {
        if (Vector.IsHardwareAccelerated && span.Length >= Vector<T>.Count)
        {
            int remainder = ModuloP2(span.Length, Vector<T>.Count);
            int adjustedCount = span.Length - remainder;

            if (adjustedCount > 0)
            {
                ClampImpl(span[..adjustedCount], min, max);
            }

            return adjustedCount;
        }

        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ClampImpl<T>(Span<T> span, T min, T max)
        where T : unmanaged
    {
        ref T sRef = ref MemoryMarshal.GetReference(span);
        var vmin = new Vector<T>(min);
        var vmax = new Vector<T>(max);

        int n = span.Length / Vector<T>.Count;
        int m = Modulo4(n);
        int u = n - m;

        ref Vector<T> vs0 = ref Unsafe.As<T, Vector<T>>(ref MemoryMarshal.GetReference(span));
        ref Vector<T> vs1 = ref Unsafe.Add(ref vs0, 1);
        ref Vector<T> vs2 = ref Unsafe.Add(ref vs0, 2);
        ref Vector<T> vs3 = ref Unsafe.Add(ref vs0, 3);
        ref Vector<T> vsEnd = ref Unsafe.Add(ref vs0, u);

        while (Unsafe.IsAddressLessThan(ref vs0, ref vsEnd))
        {
            vs0 = Vector.Min(Vector.Max(vmin, vs0), vmax);
            vs1 = Vector.Min(Vector.Max(vmin, vs1), vmax);
            vs2 = Vector.Min(Vector.Max(vmin, vs2), vmax);
            vs3 = Vector.Min(Vector.Max(vmin, vs3), vmax);

            vs0 = ref Unsafe.Add(ref vs0, 4);
            vs1 = ref Unsafe.Add(ref vs1, 4);
            vs2 = ref Unsafe.Add(ref vs2, 4);
            vs3 = ref Unsafe.Add(ref vs3, 4);
        }

        if (m > 0)
        {
            vs0 = ref vsEnd;
            vsEnd = ref Unsafe.Add(ref vsEnd, m);

            while (Unsafe.IsAddressLessThan(ref vs0, ref vsEnd))
            {
                vs0 = Vector.Min(Vector.Max(vmin, vs0), vmax);

                vs0 = ref Unsafe.Add(ref vs0, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Premultiply(ref Vector4 source)
    {
        float w = source.W;
        source *= w;
        source.W = w;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UnPremultiply(ref Vector4 source)
    {
        float w = source.W;
        source /= w;
        source.W = w;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Premultiply(Span<Vector4> vectors)
    {
        if (Avx2.IsSupported && vectors.Length >= 2)
        {
            ref Vector256<float> vectorsBase = ref Unsafe.As<Vector4, Vector256<float>>(ref MemoryMarshal.GetReference(vectors));
            ref Vector256<float> vectorsLast = ref Unsafe.Add(ref vectorsBase, (IntPtr)((uint)vectors.Length / 2u));

            while (Unsafe.IsAddressLessThan(ref vectorsBase, ref vectorsLast))
            {
                Vector256<float> source = vectorsBase;
                Vector256<float> multiply = Avx.Shuffle(source, source, ShuffleAlphaControl);
                vectorsBase = Avx.Blend(Avx.Multiply(source, multiply), source, BlendAlphaControl);
                vectorsBase = ref Unsafe.Add(ref vectorsBase, 1);
            }

            if (Modulo2(vectors.Length) != 0)
            {
                Premultiply(ref MemoryMarshal.GetReference(vectors[^1..]));
            }
        }
        else
        {
            ref Vector4 vectorsStart = ref MemoryMarshal.GetReference(vectors);
            ref Vector4 vectorsEnd = ref Unsafe.Add(ref vectorsStart, vectors.Length);

            while (Unsafe.IsAddressLessThan(ref vectorsStart, ref vectorsEnd))
            {
                Premultiply(ref vectorsStart);

                vectorsStart = ref Unsafe.Add(ref vectorsStart, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void UnPremultiply(Span<Vector4> vectors)
    {
        if (Avx2.IsSupported && vectors.Length >= 2)
        {
            ref Vector256<float> vectorsBase = ref Unsafe.As<Vector4, Vector256<float>>(ref MemoryMarshal.GetReference(vectors));
            ref Vector256<float> vectorsLast = ref Unsafe.Add(ref vectorsBase, (IntPtr)((uint)vectors.Length / 2u));

            while (Unsafe.IsAddressLessThan(ref vectorsBase, ref vectorsLast))
            {
                Vector256<float> source = vectorsBase;
                Vector256<float> multiply = Avx.Shuffle(source, source, ShuffleAlphaControl);
                vectorsBase = Avx.Blend(Avx.Divide(source, multiply), source, BlendAlphaControl);
                vectorsBase = ref Unsafe.Add(ref vectorsBase, 1);
            }

            if (Modulo2(vectors.Length) != 0)
            {
                UnPremultiply(ref MemoryMarshal.GetReference(vectors[^1..]));
            }
        }
        else
        {
            ref Vector4 vectorsStart = ref MemoryMarshal.GetReference(vectors);
            ref Vector4 vectorsEnd = ref Unsafe.Add(ref vectorsStart, vectors.Length);

            while (Unsafe.IsAddressLessThan(ref vectorsStart, ref vectorsEnd))
            {
                UnPremultiply(ref vectorsStart);

                vectorsStart = ref Unsafe.Add(ref vectorsStart, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void CubePowOnXYZ(Span<Vector4> vectors)
    {
        ref Vector4 baseRef = ref MemoryMarshal.GetReference(vectors);
        ref Vector4 endRef = ref Unsafe.Add(ref baseRef, vectors.Length);

        while (Unsafe.IsAddressLessThan(ref baseRef, ref endRef))
        {
            Vector4 v = baseRef;
            float a = v.W;
            v = v * v * v;
            v.W = a;

            baseRef = v;
            baseRef = ref Unsafe.Add(ref baseRef, 1);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void CubeRootOnXYZ(Span<Vector4> vectors)
    {
        if (Sse41.IsSupported)
        {
            ref Vector128<float> vectors128Ref = ref Unsafe.As<Vector4, Vector128<float>>(ref MemoryMarshal.GetReference(vectors));
            ref Vector128<float> vectors128End = ref Unsafe.Add(ref vectors128Ref, vectors.Length);

            var v128_341 = Vector128.Create(341);
            Vector128<int> v128_negativeZero = Vector128.Create(-0.0f).AsInt32();
            Vector128<int> v128_one = Vector128.Create(1.0f).AsInt32();

            var v128_13rd = Vector128.Create(1 / 3f);
            var v128_23rds = Vector128.Create(2 / 3f);

            while (Unsafe.IsAddressLessThan(ref vectors128Ref, ref vectors128End))
            {
                Vector128<float> vecx = vectors128Ref;
                Vector128<int> veax = vecx.AsInt32();
                veax = Sse2.AndNot(v128_negativeZero, veax);
                veax = Sse2.Subtract(veax, v128_one);
                veax = Sse2.ShiftRightArithmetic(veax, 10);
                veax = Sse41.MultiplyLow(veax, v128_341);
                veax = Sse2.Add(veax, v128_one);
                veax = Sse2.AndNot(v128_negativeZero, veax);
                veax = Sse2.Or(veax, Sse2.And(vecx.AsInt32(), v128_negativeZero));

                Vector128<float> y4 = veax.AsSingle();

                if (Fma.IsSupported)
                {
                    y4 = Fma.MultiplyAdd(v128_23rds, y4, Sse.Multiply(v128_13rd, Sse.Divide(vecx, Sse.Multiply(y4, y4))));
                    y4 = Fma.MultiplyAdd(v128_23rds, y4, Sse.Multiply(v128_13rd, Sse.Divide(vecx, Sse.Multiply(y4, y4))));
                }
                else
                {
                    y4 = Sse.Add(Sse.Multiply(v128_23rds, y4), Sse.Multiply(v128_13rd, Sse.Divide(vecx, Sse.Multiply(y4, y4))));
                    y4 = Sse.Add(Sse.Multiply(v128_23rds, y4), Sse.Multiply(v128_13rd, Sse.Divide(vecx, Sse.Multiply(y4, y4))));
                }

                y4 = Sse41.Insert(y4, vecx, 0xF0);

                vectors128Ref = y4;
                vectors128Ref = ref Unsafe.Add(ref vectors128Ref, 1);
            }
        }
        else
        {
            ref Vector4 vectorsRef = ref MemoryMarshal.GetReference(vectors);
            ref Vector4 vectorsEnd = ref Unsafe.Add(ref vectorsRef, vectors.Length);

            while (Unsafe.IsAddressLessThan(ref vectorsRef, ref vectorsEnd))
            {
                Vector4 v = vectorsRef;

                double
                    x64 = v.X,
                    y64 = v.Y,
                    z64 = v.Z;
                float a = v.W;

                ulong
                    xl = *(ulong*)&x64,
                    yl = *(ulong*)&y64,
                    zl = *(ulong*)&z64;

                xl = 0x2a9f8a7be393b600 + (xl / 3);
                yl = 0x2a9f8a7be393b600 + (yl / 3);
                zl = 0x2a9f8a7be393b600 + (zl / 3);

                Vector4 y4;
                y4.X = (float)*(double*)&xl;
                y4.Y = (float)*(double*)&yl;
                y4.Z = (float)*(double*)&zl;
                y4.W = 0;

                y4 = (2 / 3f * y4) + (1 / 3f * (v / (y4 * y4)));
                y4 = (2 / 3f * y4) + (1 / 3f * (v / (y4 * y4)));
                y4.W = a;

                vectorsRef = y4;
                vectorsRef = ref Unsafe.Add(ref vectorsRef, 1);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector256<float> Lerp(
        in Vector256<float> value1,
        in Vector256<float> value2,
        in Vector256<float> amount)
    {
        Vector256<float> diff = Avx.Subtract(value2, value1);
        if (Fma.IsSupported)
        {
            return Fma.MultiplyAdd(diff, amount, value1);
        }
        else
        {
            return Avx.Add(Avx.Multiply(diff, amount), value1);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Lerp(float value1, float value2, float amount) => ((value2 - value1) * amount) + value1;

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
    public static int ReduceSum(Vector128<int> accumulator)
    {
        Vector128<int> vsum = Sse2.Add(accumulator, Sse2.Shuffle(accumulator, 0b_11_11_01_01));
        vsum = Sse2.Add(vsum, Sse2.Shuffle(vsum, 0b_11_10_11_10));
        return Sse2.ConvertToInt32(vsum);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReduceSum(Vector256<int> accumulator)
    {
        Vector128<int> vsum = Sse2.Add(accumulator.GetLower(), accumulator.GetUpper());
        vsum = Sse2.Add(vsum, Sse2.Shuffle(vsum, 0b_11_11_01_01));
        vsum = Sse2.Add(vsum, Sse2.Shuffle(vsum, 0b_11_10_11_10));
        return Sse2.ConvertToInt32(vsum);
    }

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

    public static uint DivideCeil(uint value, uint divisor) => (value + divisor - 1) / divisor;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOutOfRange(int value, int min, int max) => (uint)(value - min) > (uint)(max - min);
}
