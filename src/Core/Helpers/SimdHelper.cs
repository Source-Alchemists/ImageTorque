using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace ImageTorque;

internal static partial class SimdHelper
{
    public static bool HasVector8 { get; } = Vector.IsHardwareAccelerated && Vector<float>.Count == 8 && Vector<int>.Count == 8;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector4 PseudoRound(this Vector4 v)
    {
        Vector4 sign = VectorMath.Clamp(v, new Vector4(-1), new Vector4(1));
        return v + (sign * 0.5f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Vector<float> FastRound(this Vector<float> v)
    {
        if (Avx2.IsSupported)
        {
            ref Vector256<float> v256 = ref Unsafe.As<Vector<float>, Vector256<float>>(ref v);
            Vector256<float> vRound = Avx.RoundToNearestInteger(v256);
            return Unsafe.As<Vector256<float>, Vector<float>>(ref vRound);
        }
        else
        {
            var magic0 = new Vector<int>(int.MinValue);
            var sgn0 = Vector.AsVectorSingle(magic0);
            var and0 = Vector.BitwiseAnd(sgn0, v);
            var or0 = Vector.BitwiseOr(and0, new Vector<float>(8388608.0f));
            var add0 = Vector.Add(v, or0);
            return Vector.Subtract(add0, or0);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector128<byte> BlendVariable(Vector128<byte> left, Vector128<byte> right, Vector128<byte> mask)
    {
        if (Sse41.IsSupported)
        {
            return Sse41.BlendVariable(left, right, mask);
        }
        else if (Sse2.IsSupported)
        {
            return Sse2.Or(Sse2.And(right, mask), Sse2.AndNot(mask, left));
        }

        Vector128<short> signedMask = AdvSimd.ShiftRightArithmetic(mask.AsInt16(), 7);
        return AdvSimd.BitwiseSelect(signedMask, right.AsInt16(), left.AsInt16()).AsByte();
    }
}
