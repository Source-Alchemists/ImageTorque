using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// <see href="https://www.w3.org/TR/PNG-Filters.html"/>
/// </summary>
internal static class FilterSub
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Decode(Span<byte> scanline, int bytesPerPixel)
    {

        if (AdvSimd.IsSupported && bytesPerPixel is 4)
        {
            DecodeArm(scanline);
        }
        else if (Sse2.IsSupported && bytesPerPixel is 4)
        {
            DecodeSse2(scanline);
        }
        else
        {
            DecodeScalar(scanline, (uint)bytesPerPixel);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(ReadOnlySpan<byte> scanline, ReadOnlySpan<byte> result, int bytesPerPixel, out int sum)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte resultBaseRef = ref MemoryMarshal.GetReference(result);
        sum = 0;
        resultBaseRef = (byte)FilterType.Sub;

        nuint x = 0;
        while(x < (uint)bytesPerPixel)
        {
            byte scan = Unsafe.Add(ref scanBaseRef, x);
            ++x;
            ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
            res = scan;
            sum += NumericMath.Abs(unchecked((sbyte)res));
        }

        if (Avx2.IsSupported)
        {
            Vector256<byte> vectorZero = Vector256<byte>.Zero;
            Vector256<int> sumAccumulator = Vector256<int>.Zero;
            uint vector256Count = (uint)Vector256<byte>.Count;

            for (nuint xLeft = x - (uint)bytesPerPixel; x <= (uint)(scanline.Length - vector256Count); xLeft += vector256Count)
            {
                Vector256<byte> scan = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector256<byte> prev = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref scanBaseRef, xLeft));
                Vector256<byte> res = Avx2.Subtract(scan, prev);
                Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res;
                x += vector256Count;

                sumAccumulator = Avx2.Add(sumAccumulator, Avx2.SumAbsoluteDifferences(Avx2.Abs(res.AsSByte()), vectorZero).AsInt32());
            }

            sum += VectorMath.EvenReduceSum(sumAccumulator);
        }
        else if (Vector.IsHardwareAccelerated)
        {
            Vector<uint> sumAccumulator = Vector<uint>.Zero;
            uint vectorCount = (uint)Vector<byte>.Count;

            for (nuint xLeft = x - (uint)bytesPerPixel; x <= (uint)(scanline.Length - vectorCount); xLeft += vectorCount)
            {
                Vector<byte> scan = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector<byte> prev = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref scanBaseRef, xLeft));

                Vector<byte> res = scan - prev;
                Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res;
                x += (uint)Vector<byte>.Count;

                NumericMath.Accumulate(ref sumAccumulator, Vector.AsVectorByte(Vector.Abs(Vector.AsVectorSByte(res))));
            }

            for (int i = 0; i < Vector<uint>.Count; i++)
            {
                sum += (int)sumAccumulator[i];
            }
        }

        for (nuint xLeft = x - (uint)bytesPerPixel; x < (uint)scanline.Length; ++xLeft)
        {
            byte scan = Unsafe.Add(ref scanBaseRef, x);
            byte prev = Unsafe.Add(ref scanBaseRef, xLeft);
            ++x;
            ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
            res = (byte)(scan - prev);
            sum += NumericMath.Abs(unchecked((sbyte)res));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeArm(Span<byte> scanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        Vector64<byte> d = Vector64<byte>.Zero;
        int rb = scanline.Length;
        nuint offset = 1;
        const int bytesPerBatch = 4;

        while (rb >= bytesPerBatch)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector64<byte> a = d;
            d = Vector64.CreateScalar(Unsafe.As<byte, int>(ref scanRef)).AsByte();
            d = AdvSimd.Add(d, a);
            Unsafe.As<byte, int>(ref scanRef) = d.AsInt32().ToScalar();
            rb -= bytesPerBatch;
            offset += bytesPerBatch;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeScalar(Span<byte> scanline, nuint bytesPerPixel)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        nuint x = bytesPerPixel + 1;
        Unsafe.Add(ref scanBaseRef, x);

        for (; x < (uint)scanline.Length; ++x)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
            byte prev = Unsafe.Add(ref scanBaseRef, x - bytesPerPixel);
            scan = (byte)(scan + prev);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeSse2(Span<byte> scanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        Vector128<byte> d = Vector128<byte>.Zero;
        int rb = scanline.Length;
        nuint offset = 1;

        while (rb >= 4)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector128<byte> a = d;
            d = Sse2.ConvertScalarToVector128Int32(Unsafe.As<byte, int>(ref scanRef)).AsByte();
            d = Sse2.Add(d, a);
            Unsafe.As<byte, int>(ref scanRef) = Sse2.ConvertToInt32(d.AsInt32());
            rb -= 4;
            offset += 4;
        }
    }
}
