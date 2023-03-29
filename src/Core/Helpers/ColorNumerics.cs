using System.Runtime.CompilerServices;
using ImageTorque.Pixels;

namespace ImageTorque;

internal static class ColorNumerics
{
    /// <summary>
    /// Gets the luminance from the rgb components using the formula
    /// as specified by ITU-R Recommendation BT.709.
    /// </summary>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    /// <param name="b">The blue component.</param>
    /// <returns>The <see cref="byte"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte Get8BitBT709Luminance(byte r, byte g, byte b)
        => (byte)((r * .2126F) + (g * .7152F) + (b * .0722F) + 0.5F);

    /// <summary>
    /// Gets the luminance from the rgb components using the formula as
    /// specified by ITU-R Recommendation BT.709.
    /// </summary>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    /// <param name="b">The blue component.</param>
    /// <returns>The <see cref="ushort"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Get16BitBT709Luminance(ushort r, ushort g, ushort b)
        => (ushort)((r * .2126F) + (g * .7152F) + (b * .0722F) + 0.5F);

    /// <summary>
    /// Gets the luminance from the rgb components using the formula as specified
    /// by ITU-R Recommendation BT.709.
    /// </summary>
    /// <param name="r">The red component.</param>
    /// <param name="g">The green component.</param>
    /// <param name="b">The blue component.</param>
    /// <returns>The <see cref="ushort"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort Get16BitBT709Luminance(float r, float g, float b)
        => (ushort)((r * .2126F) + (g * .7152F) + (b * .0722F) + 0.5F);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetSaturation(Rgb rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;

        float max = MathF.Max(r, MathF.Max(g, b));
        float min = MathF.Min(r, MathF.Min(g, b));
        float chroma = max - min;

        if (MathF.Abs(chroma) < float.Epsilon)
        {
            return 0F;
        }

        float l = (max + min) / 2F;

        if (l <= .5F)
        {
            return chroma / (max + min);
        }
        else
        {
            return chroma / (2F - max - min);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetSaturation(Rgb24 rgb)
    {
        float r = rgb.Red / 255F;
        float g = rgb.Green / 255F;
        float b = rgb.Blue / 255F;

        float max = MathF.Max(r, MathF.Max(g, b));
        float min = MathF.Min(r, MathF.Min(g, b));
        float chroma = max - min;

        if (MathF.Abs(chroma) < float.Epsilon)
        {
            return 0F;
        }

        float l = (max + min) / 2F;

        if (l <= .5F)
        {
            return chroma / (max + min);
        }
        else
        {
            return chroma / (2F - max - min);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetSaturation(Rgb48 rgb)
    {
        float r = rgb.Red / 65535F;
        float g = rgb.Green / 65535F;
        float b = rgb.Blue / 65535F;

        float max = MathF.Max(r, MathF.Max(g, b));
        float min = MathF.Min(r, MathF.Min(g, b));
        float chroma = max - min;

        if (MathF.Abs(chroma) < float.Epsilon)
        {
            return 0F;
        }

        float l = (max + min) / 2F;

        if (l <= .5F)
        {
            return chroma / (max + min);
        }
        else
        {
            return chroma / (2F - max - min);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMaxChroma(Rgb rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;
        const float achromatic = 0.5F;

        float cb = 0.501F + ((-0.168736F * r) - (0.331264F * g) + (0.5F * b));
        float cr = 0.501F + ((0.5F * r) - (0.418688F * g) - (0.081312F * b));

        return MathF.Max(MathF.Abs(cb - achromatic), MathF.Abs(cr - achromatic));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMaxChroma(Rgb24 rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;
        const float achromatic = 127.5F;

        float cb = 128F + ((-0.168736F * r) - (0.331264F * g) + (0.5F * b));
        float cr = 128F + ((0.5F * r) - (0.418688F * g) - (0.081312F * b));

        return MathF.Max(MathF.Abs(cb - achromatic), MathF.Abs(cr - achromatic));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMaxChroma(Rgb48 rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;
        const float achromatic = 32767.5F;

        float cb = 32768F + ((-0.168736F * r) - (0.331264F * g) + (0.5F * b));
        float cr = 32768F + ((0.5F * r) - (0.418688F * g) - (0.081312F * b));

        return MathF.Max(MathF.Abs(cb - achromatic), MathF.Abs(cr - achromatic));
    }
}
