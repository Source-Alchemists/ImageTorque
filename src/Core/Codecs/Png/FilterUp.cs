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
internal static class FilterUp
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Decode(Span<byte> scanline, Span<byte> previousScanline)
    {
        if (Avx2.IsSupported)
        {
            DecodeAvx2(scanline, previousScanline);
        }
        else if (AdvSimd.IsSupported)
        {
            DecodeArm(scanline, previousScanline);
        }
        else if (Sse2.IsSupported)
        {
            DecodeSse2(scanline, previousScanline);
        }
        else
        {
            DecodeScalar(scanline, previousScanline);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(ReadOnlySpan<byte> scanline, ReadOnlySpan<byte> previousScanline, Span<byte> result, out int sum)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        ref byte resultBaseRef = ref MemoryMarshal.GetReference(result);
        sum = 0;

        resultBaseRef = (byte)FilterType.Up;

        nuint x = 0;

        if (Avx2.IsSupported)
        {
            Vector256<byte> vectorZero = Vector256<byte>.Zero;
            Vector256<int> sumAccumulator = Vector256<int>.Zero;

            while (x <= (uint)(scanline.Length - Vector256<byte>.Count))
            {
                Vector256<byte> scan = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector256<byte> above = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref prevBaseRef, x));

                Vector256<byte> res = Avx2.Subtract(scan, above);
                Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res; // +1 to skip filter type
                x += (uint)Vector256<byte>.Count;

                sumAccumulator = Avx2.Add(sumAccumulator, Avx2.SumAbsoluteDifferences(Avx2.Abs(res.AsSByte()), vectorZero).AsInt32());
            }

            sum += VectorMath.EvenReduceSum(sumAccumulator);
        }
        else if (Vector.IsHardwareAccelerated)
        {
            Vector<uint> sumAccumulator = Vector<uint>.Zero;

            while (x <= (uint)(scanline.Length - Vector<byte>.Count))
            {
                Vector<byte> scan = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector<byte> above = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref prevBaseRef, x));

                Vector<byte> res = scan - above;
                Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res; // +1 to skip filter type
                x += (uint)Vector<byte>.Count;

                NumericMath.Accumulate(ref sumAccumulator, Vector.AsVectorByte(Vector.Abs(Vector.AsVectorSByte(res))));
            }

            for (int i = 0; i < Vector<uint>.Count; i++)
            {
                sum += (int)sumAccumulator[i];
            }
        }

        while (x < (uint)scanline.Length)
        {
            byte scan = Unsafe.Add(ref scanBaseRef, x);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            ++x;
            ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
            res = (byte)(scan - above);
            sum += NumericMath.Abs(unchecked((sbyte)res));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeArm(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        int rb = scanline.Length;
        nuint offset = 1;
        const int bytesPerBatch = 16;

        while (rb >= bytesPerBatch)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector128<byte> prior = Unsafe.As<byte, Vector128<byte>>(ref scanRef);
            Vector128<byte> up = Unsafe.As<byte, Vector128<byte>>(ref Unsafe.Add(ref prevBaseRef, offset));

            Unsafe.As<byte, Vector128<byte>>(ref scanRef) = AdvSimd.Add(prior, up);

            offset += bytesPerBatch;
            rb -= bytesPerBatch;
        }

        for (nuint i = offset; i < (uint)scanline.Length; i++)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, offset);
            byte above = Unsafe.Add(ref prevBaseRef, offset);
            scan = (byte)(scan + above);
            offset++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeAvx2(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        int rb = scanline.Length;
        nuint offset = 1;
        int vector256Count = Vector256<byte>.Count;

        while (rb >= vector256Count)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector256<byte> prior = Unsafe.As<byte, Vector256<byte>>(ref scanRef);
            Vector256<byte> up = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref prevBaseRef, offset));

            Unsafe.As<byte, Vector256<byte>>(ref scanRef) = Avx2.Add(up, prior);

            offset += (uint)vector256Count;
            rb -= vector256Count;
        }

        for (nuint i = offset; i < (uint)scanline.Length; i++)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, offset);
            byte above = Unsafe.Add(ref prevBaseRef, offset);
            scan = (byte)(scan + above);
            offset++;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeScalar(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);

        for (nuint x = 1; x < (uint)scanline.Length; x++)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            scan = (byte)(scan + above);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeSse2(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        int rb = scanline.Length;
        nuint offset = 1;
        int vector128Count = Vector128<byte>.Count;

        while (rb >= vector128Count)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector128<byte> prior = Unsafe.As<byte, Vector128<byte>>(ref scanRef);
            Vector128<byte> up = Unsafe.As<byte, Vector128<byte>>(ref Unsafe.Add(ref prevBaseRef, offset));

            Unsafe.As<byte, Vector128<byte>>(ref scanRef) = Sse2.Add(up, prior);

            offset += (uint)vector128Count;
            rb -= vector128Count;
        }

        for (nuint i = offset; i < (uint)scanline.Length; i++)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, offset);
            byte above = Unsafe.Add(ref prevBaseRef, offset);
            scan = (byte)(scan + above);
            offset++;
        }
    }
}
