using System.Runtime.CompilerServices;

namespace ImageTorque;

internal static class PixelValueConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float ToSingle(byte value)
    {
        return value / 255f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float ToSingle(ushort value)
    {
        return value / 65535f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte ToByte(float value)
    {
        return (byte)(value * 255f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte ToByte(ushort value)
    {
        return (byte)(((value * 255) + 32895) >> 16);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ushort ToUInt16(float value)
    {
        return (ushort)(value * 65535f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static ushort ToUInt16(byte value)
    {
        return (ushort)(value * 257);
    }
}
