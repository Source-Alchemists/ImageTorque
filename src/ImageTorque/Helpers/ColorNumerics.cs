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

    /// <summary>
    /// Gets the saturation from the rgb component
    /// </summary>
    /// <param name="rgb">The rgb component.</param>
    /// <returns>The saturation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetSaturation(Rgb rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;
        return GetSaturation(r, g, b);
    }

    /// <summary>
    /// Gets the saturation from the rgb component
    /// </summary>
    /// <param name="rgb">The rgb component.</param>
    /// <returns>The saturation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetSaturation(Rgb24 rgb)
    {
        float r = rgb.Red / 255F;
        float g = rgb.Green / 255F;
        float b = rgb.Blue / 255F;
        return GetSaturation(r, g, b);
    }

    /// <summary>
    /// Gets the saturation from the rgb component
    /// </summary>
    /// <param name="rgb">The rgb component.</param>
    /// <returns>The saturation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetSaturation(Rgb48 rgb)
    {
        float r = rgb.Red / 65535F;
        float g = rgb.Green / 65535F;
        float b = rgb.Blue / 65535F;
        return GetSaturation(r, g, b);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetSaturation(float r, float g, float b)
    {
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

    /// <summary>
    /// Gets the max chroma from the rgb component.
    /// </summary>
    /// <param name="rgb">The rgb component.</param>
    /// <returns>The max chroma.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMaxChroma(Rgb rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;
        const float achromatic = 0.5F;
        const float chromatic = 0.501F;
        return GetMaxChroma(r, g, b, achromatic, chromatic);
    }

    /// <summary>
    /// Gets the max chroma from the rgb component.
    /// </summary>
    /// <param name="rgb">The rgb component.</param>
    /// <returns>The max chroma.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMaxChroma(Rgb24 rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;
        const float achromatic = 127.5F;
        const float chromatic = 128F;
        return GetMaxChroma(r, g, b, achromatic, chromatic);
    }

    /// <summary>
    /// Gets the max chroma from the rgb component.
    /// </summary>
    /// <param name="rgb">The rgb component.</param>
    /// <returns>The max chroma.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float GetMaxChroma(Rgb48 rgb)
    {
        float r = rgb.Red;
        float g = rgb.Green;
        float b = rgb.Blue;
        const float achromatic = 32767.5F;
        const float chromatic = 32768F;
        return GetMaxChroma(r, g, b, achromatic, chromatic);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetMaxChroma(float r, float g, float b, float achromatic, float chromatic)
    {
        float cb = chromatic + ((-0.168736F * r) - (0.331264F * g) + (0.5F * b));
        float cr = chromatic + ((0.5F * r) - (0.418688F * g) - (0.081312F * b));

        return MathF.Max(MathF.Abs(cb - achromatic), MathF.Abs(cr - achromatic));
    }
}
