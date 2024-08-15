/*
 * Copyright 2024 Source Alchemists
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// <see href="https://www.w3.org/TR/PNG-Filters.html"/>
/// </summary>
internal static class FilterAverage
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Decode(Span<byte> scanline, Span<byte> previousScanline, int bytesPerPixel)
    {
        if (AdvSimd.IsSupported && bytesPerPixel is 4)
        {
            DecodeArm(scanline, previousScanline);
        }
        else if (Sse2.IsSupported && bytesPerPixel is 4)
        {
            DecodeSse2(scanline, previousScanline);
        }
        else
        {
            DecodeScalar(scanline, previousScanline, (uint)bytesPerPixel);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(ReadOnlySpan<byte> scanline, ReadOnlySpan<byte> previousScanline, Span<byte> result, uint bytesPerPixel, out int sum)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        ref byte resultBaseRef = ref MemoryMarshal.GetReference(result);
        sum = 0;
        resultBaseRef = (byte)FilterType.Average;

        nuint x = 0;
        while (x < bytesPerPixel)
        {
            byte scan = Unsafe.Add(ref scanBaseRef, x);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            ++x;
            ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
            res = (byte)(scan - (above >> 1));
            sum += NumericMath.Abs(unchecked((sbyte)res));
        }

        if (Avx2.IsSupported)
        {
            Vector256<byte> zero = Vector256<byte>.Zero;
            Vector256<int> sumAccumulator = Vector256<int>.Zero;
            Vector256<byte> allBitsSet = Avx2.CompareEqual(sumAccumulator, sumAccumulator).AsByte();
            uint vectorCount = (uint)Vector256<byte>.Count;

            for (nuint xLeft = x - bytesPerPixel; x <= (uint)(scanline.Length - vectorCount); xLeft += vectorCount)
            {
                Vector256<byte> scan = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector256<byte> left = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref scanBaseRef, xLeft));
                Vector256<byte> above = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref prevBaseRef, x));

                Vector256<byte> avg = Avx2.Xor(Avx2.Average(Avx2.Xor(left, allBitsSet), Avx2.Xor(above, allBitsSet)), allBitsSet);
                Vector256<byte> res = Avx2.Subtract(scan, avg);

                Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res;
                x += vectorCount;

                sumAccumulator = Avx2.Add(sumAccumulator, Avx2.SumAbsoluteDifferences(Avx2.Abs(res.AsSByte()), zero).AsInt32());
            }

            sum += VectorMath.EvenReduceSum(sumAccumulator);
        }
        else if (Sse2.IsSupported)
        {
            Vector128<byte> zero = Vector128<byte>.Zero;
            Vector128<int> sumAccumulator = Vector128<int>.Zero;
            Vector128<byte> allBitsSet = Sse2.CompareEqual(sumAccumulator, sumAccumulator).AsByte();
            uint vectorCount = (uint)Vector128<byte>.Count;

            for (nuint xLeft = x - bytesPerPixel; x <= (uint)(scanline.Length - vectorCount); xLeft += vectorCount)
            {
                Vector128<byte> scan = Unsafe.As<byte, Vector128<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector128<byte> left = Unsafe.As<byte, Vector128<byte>>(ref Unsafe.Add(ref scanBaseRef, xLeft));
                Vector128<byte> above = Unsafe.As<byte, Vector128<byte>>(ref Unsafe.Add(ref prevBaseRef, x));
                Vector128<byte> avg = Sse2.Xor(Sse2.Average(Sse2.Xor(left, allBitsSet), Sse2.Xor(above, allBitsSet)), allBitsSet);
                Vector128<byte> res = Sse2.Subtract(scan, avg);

                Unsafe.As<byte, Vector128<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res;
                x += vectorCount;

                Vector128<byte> absRes;
                if (Ssse3.IsSupported)
                {
                    absRes = Ssse3.Abs(res.AsSByte());
                }
                else
                {
                    Vector128<sbyte> mask = Sse2.CompareGreaterThan(zero.AsSByte(), res.AsSByte());
                    absRes = Sse2.Xor(Sse2.Add(res.AsSByte(), mask), mask).AsByte();
                }

                sumAccumulator = Sse2.Add(sumAccumulator, Sse2.SumAbsoluteDifferences(absRes, zero).AsInt32());
            }

            sum += VectorMath.EvenReduceSum(sumAccumulator);
        }

        #pragma warning disable S1994 // This loop's stop condition tests 'scanline' and 'x' but the incrementer updates 'xLeft'.
        for (nuint xLeft = x - bytesPerPixel; x < (uint)scanline.Length; ++xLeft)
        #pragma warning restore S1994 // This loop's stop condition tests 'scanline' and 'x' but the incrementer updates 'xLeft'.
        {
            byte scan = Unsafe.Add(ref scanBaseRef, x);
            byte left = Unsafe.Add(ref scanBaseRef, xLeft);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            ++x;
            ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
            res = (byte)(scan - NumericMath.Average(left, above));
            sum += NumericMath.Abs(unchecked((sbyte)res));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeArm(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        Vector64<byte> zero = Vector64<byte>.Zero;
        const int bytesPerBatch = 4;
        nuint offset = 1;
        int rb = scanline.Length;

        while (rb >= bytesPerBatch)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector64<byte> a = zero;
            Vector64<byte> b = Vector64.CreateScalar(Unsafe.As<byte, int>(ref Unsafe.Add(ref prevBaseRef, offset))).AsByte();
            zero = Vector64.CreateScalar(Unsafe.As<byte, int>(ref scanRef)).AsByte();
            Vector64<byte> avg = AdvSimd.FusedAddHalving(a, b);
            zero = AdvSimd.Add(zero, avg);
            Unsafe.As<byte, int>(ref scanRef) = zero.AsInt32().ToScalar();
            rb -= bytesPerBatch;
            offset += bytesPerBatch;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeSse2(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        Vector128<byte> zero = Vector128<byte>.Zero;
        Vector128<byte> ones = Vector128.Create((byte)1);
        nuint offset = 1;
        int rb = scanline.Length;

        while (rb >= 4)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector128<byte> a = zero;
            Vector128<byte> b = Sse2.ConvertScalarToVector128Int32(Unsafe.As<byte, int>(ref Unsafe.Add(ref prevBaseRef, offset))).AsByte();
            zero = Sse2.ConvertScalarToVector128Int32(Unsafe.As<byte, int>(ref scanRef)).AsByte();
            Vector128<byte> avg = Sse2.Average(a, b);
            Vector128<byte> xor = Sse2.Xor(a, b);
            Vector128<byte> and = Sse2.And(xor, ones);
            avg = Sse2.Subtract(avg, and);
            zero = Sse2.Add(zero, avg);
            Unsafe.As<byte, int>(ref scanRef) = Sse2.ConvertToInt32(zero.AsInt32());
            offset += 4;
            rb -= 4;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeScalar(Span<byte> scanline, Span<byte> previousScanline, uint bytesPerPixel)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);

        nuint x = 1;
        for (; x <= bytesPerPixel; ++x)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            scan = (byte)(scan + (above >> 1));
        }

        for (; x < (uint)scanline.Length; ++x)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
            byte left = Unsafe.Add(ref scanBaseRef, x - bytesPerPixel);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            scan = (byte)(scan + NumericMath.Average(left, above));
        }
    }
}
