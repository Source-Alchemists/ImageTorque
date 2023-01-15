using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace ImageTorque;

internal static class Numerics
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static T Clamp<T>(T value, T min, T max) where T : INumber<T>
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

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static T ClampAdd<T>(T a, T b, T min, T max) where T : unmanaged, INumber<T>
    {
        float fMin = float.CreateChecked(min);
        float fMax = float.CreateChecked(max);
        float fA = float.CreateChecked(a);
        float fB = float.CreateChecked(b);

        float fT = Clamp(fA + fB, fMin, fMax);

        return T.CreateChecked(fT);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static T ClampSubtract<T>(T a, T b, T min, T max) where T : unmanaged, INumber<T>
    {
        float fMin = float.CreateChecked(min);
        float fMax = float.CreateChecked(max);
        float fA = float.CreateChecked(a);
        float fB = float.CreateChecked(b);

        float fT = Clamp(fA - fB, fMin, fMax);

        return T.CreateChecked(fT);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static T ClampMultiply<T>(T a, T b, T min, T max) where T : unmanaged, INumber<T>
    {
        float fMin = float.CreateChecked(min);
        float fMax = float.CreateChecked(max);
        float fA = float.CreateChecked(a);
        float fB = float.CreateChecked(b);

        float fT = Clamp(fA * fB, fMin, fMax);

        return T.CreateChecked(fT);
    }
}
