using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace ImageTorque.Formats.Png;

/// <summary>
/// <see href="https://www.w3.org/TR/PNG-Filters.html"/>
/// </summary>
internal static class FilterPaeth
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Decode(Span<byte> scanline, Span<byte> previousScanline, int bytesPerPixel)
    {
        if (AdvSimd.Arm64.IsSupported && bytesPerPixel is 4)
        {
            DecodeArm(scanline, previousScanline);
        }
        else if (Ssse3.IsSupported && bytesPerPixel is 4)
        {
            DecodeSsse3(scanline, previousScanline);
        }
        else
        {
            DecodeScalar(scanline, previousScanline, (uint)bytesPerPixel);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeArm(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);

        Vector128<byte> b = Vector128<byte>.Zero;
        Vector128<byte> d = Vector128<byte>.Zero;

        int rb = scanline.Length;
        nuint offset = 1;
        const int bytesPerBatch = 4;
        while (rb >= bytesPerBatch)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector128<byte> c = b;
            Vector128<byte> a = d;
            b = AdvSimd.Arm64.ZipLow(
                Vector128.CreateScalar(Unsafe.As<byte, int>(ref Unsafe.Add(ref prevBaseRef, offset))).AsByte(),
                Vector128<byte>.Zero).AsByte();
            d = AdvSimd.Arm64.ZipLow(
                Vector128.CreateScalar(Unsafe.As<byte, int>(ref scanRef)).AsByte(),
                Vector128<byte>.Zero).AsByte();
            Vector128<short> pa = AdvSimd.Subtract(b.AsInt16(), c.AsInt16());
            Vector128<short> pb = AdvSimd.Subtract(a.AsInt16(), c.AsInt16());
            Vector128<short> pc = AdvSimd.Add(pa.AsInt16(), pb.AsInt16());
            pa = AdvSimd.Abs(pa.AsInt16()).AsInt16();
            pb = AdvSimd.Abs(pb.AsInt16()).AsInt16();
            pc = AdvSimd.Abs(pc.AsInt16()).AsInt16();
            Vector128<short> smallest = AdvSimd.Min(pc, AdvSimd.Min(pa, pb));
            Vector128<byte> mask = SimdHelper.BlendVariable(c, b, AdvSimd.CompareEqual(smallest, pb).AsByte());
            Vector128<byte> nearest = SimdHelper.BlendVariable(mask, a, AdvSimd.CompareEqual(smallest, pa).AsByte());
            d = AdvSimd.Add(d, nearest);
            Vector64<byte> e = AdvSimd.ExtractNarrowingSaturateUnsignedLower(d.AsInt16());
            Unsafe.As<byte, int>(ref scanRef) = Vector128.Create(e, e).AsInt32().ToScalar();
            offset += bytesPerBatch;
            rb -= bytesPerBatch;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Encode(ReadOnlySpan<byte> scanline, ReadOnlySpan<byte> previousScanline, Span<byte> result, int bytesPerPixel, out int sum)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        ref byte resultBaseRef = ref MemoryMarshal.GetReference(result);
        sum = 0;
        resultBaseRef = (byte)FilterType.Paeth;

        nuint x = 0;
        for (; x < (uint)bytesPerPixel;)
        {
            byte scan = Unsafe.Add(ref scanBaseRef, x);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            ++x;
            ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
            res = (byte)(scan - PaethPredictor(0, above, 0));
            sum += NumericMath.Abs(unchecked((sbyte)res));
        }

        if (Avx2.IsSupported)
        {
            Vector256<byte> zero = Vector256<byte>.Zero;
            Vector256<int> sumAccumulator = Vector256<int>.Zero;

            for (nuint xLeft = x - (uint)bytesPerPixel; (int)x <= scanline.Length - Vector256<byte>.Count; xLeft += (uint)Vector256<byte>.Count)
            {
                Vector256<byte> scan = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector256<byte> left = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref scanBaseRef, xLeft));
                Vector256<byte> above = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref prevBaseRef, x));
                Vector256<byte> upperLeft = Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref prevBaseRef, xLeft));

                Vector256<byte> res = Avx2.Subtract(scan, PaethPredictor(left, above, upperLeft));
                Unsafe.As<byte, Vector256<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res;
                x += (uint)Vector256<byte>.Count;

                sumAccumulator = Avx2.Add(sumAccumulator, Avx2.SumAbsoluteDifferences(Avx2.Abs(res.AsSByte()), zero).AsInt32());
            }

            sum += VectorMath.EvenReduceSum(sumAccumulator);
        }
        else if (Vector.IsHardwareAccelerated)
        {
            Vector<uint> sumAccumulator = Vector<uint>.Zero;

            for (nuint xLeft = x - (uint)bytesPerPixel; (int)x <= scanline.Length - Vector<byte>.Count; xLeft += (uint)Vector<byte>.Count)
            {
                Vector<byte> scan = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref scanBaseRef, x));
                Vector<byte> left = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref scanBaseRef, xLeft));
                Vector<byte> above = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref prevBaseRef, x));
                Vector<byte> upperLeft = Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref prevBaseRef, xLeft));

                Vector<byte> res = scan - PaethPredictor(left, above, upperLeft);
                Unsafe.As<byte, Vector<byte>>(ref Unsafe.Add(ref resultBaseRef, x + 1)) = res;
                x += (uint)Vector<byte>.Count;

                NumericMath.Accumulate(ref sumAccumulator, Vector.AsVectorByte(Vector.Abs(Vector.AsVectorSByte(res))));
            }

            for (int i = 0; i < Vector<uint>.Count; i++)
            {
                sum += (int)sumAccumulator[i];
            }
        }

        for (nuint xLeft = x - (uint)bytesPerPixel; (int)x < scanline.Length; ++xLeft)
        {
            byte scan = Unsafe.Add(ref scanBaseRef, x);
            byte left = Unsafe.Add(ref scanBaseRef, xLeft);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            byte upperLeft = Unsafe.Add(ref prevBaseRef, xLeft);
            ++x;
            ref byte res = ref Unsafe.Add(ref resultBaseRef, x);
            res = (byte)(scan - PaethPredictor(left, above, upperLeft));
            sum += NumericMath.Abs(unchecked((sbyte)res));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeSsse3(Span<byte> scanline, Span<byte> previousScanline)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);
        Vector128<byte> b = Vector128<byte>.Zero;
        Vector128<byte> d = Vector128<byte>.Zero;
        int rb = scanline.Length;
        nuint offset = 1;

        while (rb >= 4)
        {
            ref byte scanRef = ref Unsafe.Add(ref scanBaseRef, offset);
            Vector128<byte> c = b;
            Vector128<byte> a = d;
            b = Sse2.UnpackLow(
                Sse2.ConvertScalarToVector128Int32(Unsafe.As<byte, int>(ref Unsafe.Add(ref prevBaseRef, offset))).AsByte(),
                Vector128<byte>.Zero);
            d = Sse2.UnpackLow(
                Sse2.ConvertScalarToVector128Int32(Unsafe.As<byte, int>(ref scanRef)).AsByte(),
                Vector128<byte>.Zero);
            Vector128<short> pa = Sse2.Subtract(b.AsInt16(), c.AsInt16());
            Vector128<short> pb = Sse2.Subtract(a.AsInt16(), c.AsInt16());
            Vector128<short> pc = Sse2.Add(pa.AsInt16(), pb.AsInt16());
            pa = Ssse3.Abs(pa.AsInt16()).AsInt16();
            pb = Ssse3.Abs(pb.AsInt16()).AsInt16();
            pc = Ssse3.Abs(pc.AsInt16()).AsInt16();
            Vector128<short> smallest = Sse2.Min(pc, Sse2.Min(pa, pb));
            Vector128<byte> mask = SimdHelper.BlendVariable(c, b, Sse2.CompareEqual(smallest, pb).AsByte());
            Vector128<byte> nearest = SimdHelper.BlendVariable(mask, a, Sse2.CompareEqual(smallest, pa).AsByte());
            d = Sse2.Add(d, nearest);
            Unsafe.As<byte, int>(ref scanRef) = Sse2.ConvertToInt32(Sse2.PackUnsignedSaturate(d.AsInt16(), d.AsInt16()).AsInt32());
            offset += 4;
            rb -= 4;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void DecodeScalar(Span<byte> scanline, Span<byte> previousScanline, uint bytesPerPixel)
    {
        ref byte scanBaseRef = ref MemoryMarshal.GetReference(scanline);
        ref byte prevBaseRef = ref MemoryMarshal.GetReference(previousScanline);

        nuint offset = bytesPerPixel + 1;
        nuint x = 1;
        for (; x < offset; x++)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            scan = (byte)(scan + above);
        }

        for (; x < (uint)scanline.Length; x++)
        {
            ref byte scan = ref Unsafe.Add(ref scanBaseRef, x);
            byte left = Unsafe.Add(ref scanBaseRef, x - bytesPerPixel);
            byte above = Unsafe.Add(ref prevBaseRef, x);
            byte upperLeft = Unsafe.Add(ref prevBaseRef, x - bytesPerPixel);
            scan = (byte)(scan + PaethPredictor(left, above, upperLeft));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte PaethPredictor(byte left, byte above, byte upperLeft)
    {
        int p = left + above - upperLeft;
        int pa = NumericMath.Abs(p - left);
        int pb = NumericMath.Abs(p - above);
        int pc = NumericMath.Abs(p - upperLeft);

        if (pa <= pb && pa <= pc)
        {
            return left;
        }

        if (pb <= pc)
        {
            return above;
        }

        return upperLeft;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector256<byte> PaethPredictor(Vector256<byte> left, Vector256<byte> above, Vector256<byte> upleft)
    {
        Vector256<byte> zero = Vector256<byte>.Zero;
        Vector256<byte> sac = Avx2.SubtractSaturate(above, upleft);
        Vector256<byte> sbc = Avx2.SubtractSaturate(left, upleft);
        Vector256<byte> pa = Avx2.Or(Avx2.SubtractSaturate(upleft, above), sac);
        Vector256<byte> pb = Avx2.Or(Avx2.SubtractSaturate(upleft, left), sbc);
        Vector256<byte> pm = Avx2.CompareEqual(Avx2.CompareEqual(sac, zero), Avx2.CompareEqual(sbc, zero));
        Vector256<byte> pc = Avx2.Or(pm, Avx2.Or(Avx2.SubtractSaturate(pb, pa), Avx2.SubtractSaturate(pa, pb)));
        Vector256<byte> minbc = Avx2.Min(pc, pb);
        Vector256<byte> resbc = Avx2.BlendVariable(upleft, above, Avx2.CompareEqual(minbc, pb));
        return Avx2.BlendVariable(resbc, left, Avx2.CompareEqual(Avx2.Min(minbc, pa), pa));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector<byte> PaethPredictor(Vector<byte> left, Vector<byte> above, Vector<byte> upperLeft)
    {
        Vector.Widen(left, out Vector<ushort> a1, out Vector<ushort> a2);
        Vector.Widen(above, out Vector<ushort> b1, out Vector<ushort> b2);
        Vector.Widen(upperLeft, out Vector<ushort> c1, out Vector<ushort> c2);

        Vector<short> p1 = PaethPredictor(Vector.AsVectorInt16(a1), Vector.AsVectorInt16(b1), Vector.AsVectorInt16(c1));
        Vector<short> p2 = PaethPredictor(Vector.AsVectorInt16(a2), Vector.AsVectorInt16(b2), Vector.AsVectorInt16(c2));
        return Vector.AsVectorByte(Vector.Narrow(p1, p2));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector<short> PaethPredictor(Vector<short> left, Vector<short> above, Vector<short> upperLeft)
    {
        Vector<short> p = left + above - upperLeft;
        Vector<short> pa = Vector.Abs(p - left);
        Vector<short> pb = Vector.Abs(p - above);
        Vector<short> pc = Vector.Abs(p - upperLeft);

        Vector<short> pa_pb = Vector.LessThanOrEqual(pa, pb);
        Vector<short> pa_pc = Vector.LessThanOrEqual(pa, pc);
        Vector<short> pb_pc = Vector.LessThanOrEqual(pb, pc);

        return Vector.ConditionalSelect(
            condition: Vector.BitwiseAnd(pa_pb, pa_pc),
            left: left,
            right: Vector.ConditionalSelect(
                condition: pb_pc,
                left: above,
                right: upperLeft));
    }
}
