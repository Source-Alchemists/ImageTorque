using System.Runtime.CompilerServices;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal partial class Resizer
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static float CubicHermite(float A, float B, float C, float D, float t)
    {
        float a = (-A / 2.0f) + (3.0f * B / 2.0f) - (3.0f * C / 2.0f) + (D / 2.0f);
        float b = A - (5.0f * B / 2.0f) + (2.0f * C) - (D / 2.0f);
        float c = (-A / 2.0f) + (C / 2.0f);
        float d = B;

        float t2 = t * t;
        return a * t * t2 + b * t2 + c * t + d;
    }

    /// <summary>
    /// Clamps the pixel index to within the <paramref name="src"/> image, "solves" the edge problem by considering the closest border pixels in the same direction
    /// when none are available at the actual desired (<paramref name="x"/>,<paramref name="y"/>) index.
    /// </summary>
    /// <param name="src">Source image</param>
    /// <param name="originalWidth">Width of the <paramref name="src"/> image.</param>
    /// <param name="originalHeight">Height of the <paramref name="src"/> image.</param>
    /// <param name="x">Desired <paramref name="x"/> position to be clamped to within the image.</param>
    /// <param name="y">Desired <paramref name="y"/> position to be clamped to within the image.</param>
    /// <returns>Clamped pixel's byte value as float.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetPixelClamped_Byte(ReadOnlySpan<byte> src, int originalWidth, int originalHeight, int x, int y)
    {
        int cx = Math.Clamp(x, 0, originalWidth - 1);
        int cy = Math.Clamp(y, 0, originalHeight - 1);
        return src[(cy * originalWidth) + cx]; // We need to use float, else the GPU would produce wrong results
    }

    /// <summary>
    /// Clamps the pixel index to within the <paramref name="src"/> image, "solves" the edge problem by considering the closest border pixels in the same direction
    /// when none are available at the actual desired (<paramref name="x"/>,<paramref name="y"/>) index.
    /// </summary>
    /// <param name="src">Source image</param>
    /// <param name="originalWidth">Width of the <paramref name="src"/> image.</param>
    /// <param name="originalHeight">Height of the <paramref name="src"/> image.</param>
    /// <param name="x">Desired <paramref name="x"/> position to be clamped to within the image.</param>
    /// <param name="y">Desired <paramref name="y"/> position to be clamped to within the image.</param>
    /// <returns>Clamped pixel float value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetPixelClamped_Float(ReadOnlySpan<float> src, int originalWidth, int originalHeight, int x, int y)
    {
        int cx = Math.Clamp(x, 0, originalWidth - 1);
        int cy = Math.Clamp(y, 0, originalHeight - 1);
        return src[(cy * originalWidth) + cx]; // We need to use float, else the GPU would produce wrong results
    }

    /// <summary>
    /// Clamps the pixel index to within the <paramref name="src"/> image, "solves" the edge problem by considering the closest border pixels in the same direction
    /// when none are available at the actual desired (<paramref name="x"/>,<paramref name="y"/>) index.
    /// </summary>
    /// <param name="src">Source image</param>
    /// <param name="originalWidth">Width of the <paramref name="src"/> image.</param>
    /// <param name="originalHeight">Height of the <paramref name="src"/> image.</param>
    /// <param name="x">Desired <paramref name="x"/> position to be clamped to within the image.</param>
    /// <param name="y">Desired <paramref name="y"/> position to be clamped to within the image.</param>
    /// <returns>Rgb pixel containing the clamped pixel's float values.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Rgb GetPixelClamped_Rgb(ReadOnlySpan<Rgb> src, int originalWidth, int originalHeight, int x, int y)
    {
        int cx = Math.Clamp(x, 0, originalWidth - 1);
        int cy = Math.Clamp(y, 0, originalHeight - 1);
        Rgb outPixel = src[(cy * originalWidth) + cx];
        return new Rgb(outPixel.Red, outPixel.Green, outPixel.Blue); // We need to use float, else the GPU would produce wrong
    }

    /// <summary>
    /// Clamps the pixel index to within the <paramref name="src"/> image, "solves" the edge problem by considering the closest border pixels in the same direction
    /// when none are available at the actual desired (<paramref name="x"/>,<paramref name="y"/>) index.
    /// </summary>
    /// <param name="src">Source image</param>
    /// <param name="originalWidth">Width of the <paramref name="src"/> image.</param>
    /// <param name="originalHeight">Height of the <paramref name="src"/> image.</param>
    /// <param name="x">Desired <paramref name="x"/> position to be clamped to within the image.</param>
    /// <param name="y">Desired <paramref name="y"/> position to be clamped to within the image.</param>
    /// <returns>Rgb pixel containing the clamped pixel's byte values as float.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Rgb GetPixelClamped_Rgb24(ReadOnlySpan<Rgb24> src, int originalWidth, int originalHeight, int x, int y)
    {
        int cx = Math.Clamp(x, 0, originalWidth - 1);
        int cy = Math.Clamp(y, 0, originalHeight - 1);
        Rgb24 outPixel = src[(cy * originalWidth) + cx];
        return new Rgb(outPixel.Red, outPixel.Green, outPixel.Blue); // We need to use float, else the GPU would produce wrong
    }

    /// <summary>
    /// Clamps the pixel index to within the <paramref name="src"/> image, "solves" the edge problem by considering the closest border pixels in the same direction
    /// when none are available at the actual desired (<paramref name="x"/>,<paramref name="y"/>) index.
    /// </summary>
    /// <param name="src">Source image</param>
    /// <param name="originalWidth">Width of the <paramref name="src"/> image.</param>
    /// <param name="originalHeight">Height of the <paramref name="src"/> image.</param>
    /// <param name="x">Desired <paramref name="x"/> position to be clamped to within the image.</param>
    /// <param name="y">Desired <paramref name="y"/> position to be clamped to within the image.</param>
    /// <returns>Rgb pixel containing the clamped pixel's ushort values as float.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Rgb GetPixelClamped_Rgb48(ReadOnlySpan<Rgb48> src, int originalWidth, int originalHeight, int x, int y)
    {
        int cx = Math.Clamp(x, 0, originalWidth - 1);
        int cy = Math.Clamp(y, 0, originalHeight - 1);
        Rgb48 outPixel = src[(cy * originalWidth) + cx];
        return new Rgb(outPixel.Red, outPixel.Green, outPixel.Blue); // We need to use float, else the GPU would produce wrong
    }

    /// <summary>
    /// Clamps the pixel index to within the <paramref name="src"/> image, "solves" the edge problem by considering the closest border pixels in the same direction
    /// when none are available at the actual desired (<paramref name="x"/>,<paramref name="y"/>) index.
    /// </summary>
    /// <param name="src">Source image</param>
    /// <param name="originalWidth">Width of the <paramref name="src"/> image.</param>
    /// <param name="originalHeight">Height of the <paramref name="src"/> image.</param>
    /// <param name="x">Desired <paramref name="x"/> position to be clamped to within the image.</param>
    /// <param name="y">Desired <paramref name="y"/> position to be clamped to within the image.</param>
    /// <returns>Clamped pixel's ushort value as float.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float GetPixelClamped_Ushort(ReadOnlySpan<ushort> src, int originalWidth, int originalHeight, int x, int y)
    {
        int cx = Math.Clamp(x, 0, originalWidth - 1);
        int cy = Math.Clamp(y, 0, originalHeight - 1);
        return src[(cy * originalWidth) + cx]; // We need to use float, else the GPU would produce wrong results
    }

    /// <summary>
    /// Binary Interpolation, can be achieved by interpolating between a result of
    /// 2 interpolated same-axis points with a perpendicular axis point.
    /// In this example the horizontal axis point interpolations are calculated first
    /// and then combined with one final linear interpolation in the vertical axis.
    ///
    /// quadriliteral with the bilinear interpolation points
    ///
    /// c00 --- c10
    /// |        |
    /// |        |
    /// c01 --- c11
    ///
    /// </summary>
    /// <param name="c00">"top-left" point value</param>
    /// <param name="c10">"top-right" point value</param>
    /// <param name="c01">"bottom-left" point value</param>
    /// <param name="c11">"bottom-right" point value</param>
    /// <param name="tx">horizontal relative position on the c(i)j - c(i-1)j axis</param>
    /// <param name="ty">vertical relative position on the c(i)j - c(i-1)j axis.</param>
    /// <returns>Bilinear interpolated value for the pixel located at X(result) = X(c00)+tx, Y(result)=X(c00)+ty.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Blerp(float c00, float c10, float c01, float c11, float tx, float ty) => Lerp(
              Lerp(c00, c10, tx) // interpolation of c00 and c10, "top row" point A value
            , Lerp(c01, c11, tx) // interpolation of c01 & c11, "bottom row" point B value
            , ty); // relative position on the vertical axis, interpolating between point A & B values for the final point pixel value

    /// <summary>
    /// Linear interpolation, imprecise when t=1.
    /// See https://en.wikipedia.org/wiki/Linear_interpolation
    /// </summary>
    /// <param name="s">1st point value </param>
    /// <param name="e">2nd point value</param>
    /// <param name="t">relative position between [0.0-1.0) along the axis between the 2 points.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float Lerp(float s, float e, float t) => s + ((e - s) * t);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBicubicByte(ReadOnlySpan<byte> byteSrc, Span<byte> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bicubic_interpolation

            int destX = xDest;
            int destY = yDest;

            float v = destY / (float)(destinationHeight - 1);
            float u = destX / (float)(destinationWidth - 1);
            float x2 = (u * originalWidth) - 0.5f;
            int xint = (int)x2;
            float xfract = x2 - MathF.Floor(x2);

            float y2 = (v * originalHeight) - 0.5f;
            int yint = (int)y2;
            float yfract = y2 - MathF.Floor(y2);

            // 1st row
            float p00 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint - 1, yint - 1);
            float p01 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 0, yint - 1);
            float p02 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 1, yint - 1);
            float p03 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 2, yint - 1);

            // 2nd row
            float p10 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint - 1, yint + 0);
            float p11 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 0, yint + 0);
            float p12 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 1, yint + 0);
            float p13 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 2, yint + 0);

            // 3rd row
            float p20 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint - 1, yint + 1);
            float p21 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 0, yint + 1);
            float p22 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 1, yint + 1);
            float p23 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 2, yint + 1);

            // 4th row
            float p30 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint - 1, yint + 2);
            float p31 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 0, yint + 2);
            float p32 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 1, yint + 2);
            float p33 = GetPixelClamped_Byte(byteSrc, originalWidth, originalHeight, xint + 2, yint + 2);

            float col0 = CubicHermite(p00, p10, p20, p30, yfract);
            float col1 = CubicHermite(p01, p11, p21, p31, yfract);
            float col2 = CubicHermite(p02, p12, p22, p32, yfract);
            float col3 = CubicHermite(p03, p13, p23, p33, yfract);
            float value = CubicHermite(col0, col1, col2, col3, xfract);
            value = Math.Clamp(value, 0.0f, 255.0f);
            int dstIndex = Math.Clamp((destY * destinationWidth) + destX, 0, byteDest.Length - 1);
            byteDest[dstIndex] = (byte)value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBicubicRgb(ReadOnlySpan<Rgb> rgbSrc, Span<Rgb> rgbDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bicubic_interpolation

            int destX = xDest;
            int destY = yDest;

            float v = destY / (float)(destinationHeight - 1);
            float u = destX / (float)(destinationWidth - 1);
            float x2 = (u * originalWidth) - 0.5f;
            int xint = (int)x2;
            float xfract = x2 - MathF.Floor(x2);

            float y2 = (v * originalHeight) - 0.5f;
            int yint = (int)y2;
            float yfract = y2 - MathF.Floor(y2);

            // 1st row
            Rgb p00 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint - 1, yint - 1);
            Rgb p01 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 0, yint - 1);
            Rgb p02 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 1, yint - 1);
            Rgb p03 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 2, yint - 1);

            // 2nd row
            Rgb p10 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint - 1, yint + 0);
            Rgb p11 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 0, yint + 0);
            Rgb p12 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 1, yint + 0);
            Rgb p13 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 2, yint + 0);

            // 3rd row
            Rgb p20 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint - 1, yint + 1);
            Rgb p21 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 0, yint + 1);
            Rgb p22 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 1, yint + 1);
            Rgb p23 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 2, yint + 1);

            // 4th row
            Rgb p30 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint - 1, yint + 2);
            Rgb p31 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 0, yint + 2);
            Rgb p32 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 1, yint + 2);
            Rgb p33 = GetPixelClamped_Rgb(rgbSrc, originalWidth, originalHeight, xint + 2, yint + 2);

            // Red channel
            float col0R = CubicHermite(p00.Red, p10.Red, p20.Red, p30.Red, yfract);
            float col1R = CubicHermite(p01.Red, p11.Red, p21.Red, p31.Red, yfract);
            float col2R = CubicHermite(p02.Red, p12.Red, p22.Red, p32.Red, yfract);
            float col3R = CubicHermite(p03.Red, p13.Red, p23.Red, p33.Red, yfract);
            float valueR = CubicHermite(col0R, col1R, col2R, col3R, xfract);
            valueR = Math.Clamp(valueR, 0.0f, 1.0f);
            // Green channel
            float col0G = CubicHermite(p00.Green, p10.Green, p20.Green, p30.Green, yfract);
            float col1G = CubicHermite(p01.Green, p11.Green, p21.Green, p31.Green, yfract);
            float col2G = CubicHermite(p02.Green, p12.Green, p22.Green, p32.Green, yfract);
            float col3G = CubicHermite(p03.Green, p13.Green, p23.Green, p33.Green, yfract);
            float valueG = CubicHermite(col0G, col1G, col2G, col3G, xfract);
            valueG = Math.Clamp(valueG, 0.0f, 1.0f);
            // Blue channel
            float col0B = CubicHermite(p00.Blue, p10.Blue, p20.Blue, p30.Blue, yfract);
            float col1B = CubicHermite(p01.Blue, p11.Blue, p21.Blue, p31.Blue, yfract);
            float col2B = CubicHermite(p02.Blue, p12.Blue, p22.Blue, p32.Blue, yfract);
            float col3B = CubicHermite(p03.Blue, p13.Blue, p23.Blue, p33.Blue, yfract);
            float valueB = CubicHermite(col0B, col1B, col2B, col3B, xfract);
            valueB = Math.Clamp(valueB, 0.0f, 1.0f);
            // combine
            rgbDest[(destY * destinationWidth) + destX] = new Rgb(valueR, valueG, valueB);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBicubicRgb24(ReadOnlySpan<Rgb24> rgb24Src, Span<Rgb24> rgb24Dest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bicubic_interpolation

            int destX = xDest;
            int destY = yDest;

            float v = destY / (float)destinationHeight;
            float u = destX / (float)destinationWidth;
            float x2 = (u * originalWidth) - 0.5f;
            int xint = (int)x2;
            float xfract = x2 - MathF.Floor(x2);

            float y2 = (v * originalHeight) - 0.5f;
            int yint = (int)y2;
            float yfract = y2 - MathF.Floor(y2);

            // 1st row
            Rgb p00 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint - 1, yint - 1);
            Rgb p01 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 0, yint - 1);
            Rgb p02 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 1, yint - 1);
            Rgb p03 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 2, yint - 1);

            // 2nd row
            Rgb p10 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint - 1, yint + 0);
            Rgb p11 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 0, yint + 0);
            Rgb p12 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 1, yint + 0);
            Rgb p13 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 2, yint + 0);

            // 3rd row
            Rgb p20 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint - 1, yint + 1);
            Rgb p21 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 0, yint + 1);
            Rgb p22 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 1, yint + 1);
            Rgb p23 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 2, yint + 1);

            // 4th row
            Rgb p30 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint - 1, yint + 2);
            Rgb p31 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 0, yint + 2);
            Rgb p32 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 1, yint + 2);
            Rgb p33 = GetPixelClamped_Rgb24(rgb24Src, originalWidth, originalHeight, xint + 2, yint + 2);

            // Red channel
            float col0R = CubicHermite(p00.Red, p10.Red, p20.Red, p30.Red, yfract);
            float col1R = CubicHermite(p01.Red, p11.Red, p21.Red, p31.Red, yfract);
            float col2R = CubicHermite(p02.Red, p12.Red, p22.Red, p32.Red, yfract);
            float col3R = CubicHermite(p03.Red, p13.Red, p23.Red, p33.Red, yfract);
            float valueR = CubicHermite(col0R, col1R, col2R, col3R, xfract);
            valueR = Math.Clamp(valueR, 0.0f, 255.0f);
            // Green channel
            float col0G = CubicHermite(p00.Green, p10.Green, p20.Green, p30.Green, yfract);
            float col1G = CubicHermite(p01.Green, p11.Green, p21.Green, p31.Green, yfract);
            float col2G = CubicHermite(p02.Green, p12.Green, p22.Green, p32.Green, yfract);
            float col3G = CubicHermite(p03.Green, p13.Green, p23.Green, p33.Green, yfract);
            float valueG = CubicHermite(col0G, col1G, col2G, col3G, xfract);
            valueG = Math.Clamp(valueG, 0.0f, 255.0f);
            // Blue channel
            float col0B = CubicHermite(p00.Blue, p10.Blue, p20.Blue, p30.Blue, yfract);
            float col1B = CubicHermite(p01.Blue, p11.Blue, p21.Blue, p31.Blue, yfract);
            float col2B = CubicHermite(p02.Blue, p12.Blue, p22.Blue, p32.Blue, yfract);
            float col3B = CubicHermite(p03.Blue, p13.Blue, p23.Blue, p33.Blue, yfract);
            float valueB = CubicHermite(col0B, col1B, col2B, col3B, xfract);
            valueB = Math.Clamp(valueB, 0.0f, 255.0f);
            // combine
            rgb24Dest[(destY * destinationWidth) + destX] = new Rgb24((byte)valueR, (byte)valueG, (byte)valueB);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBicubicRgb48(ReadOnlySpan<Rgb48> rgb48Src, Span<Rgb48> rgb48Dest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bicubic_interpolation

            int destX = xDest;
            int destY = yDest;

            float v = destY / (float)(destinationHeight - 1);
            float u = destX / (float)(destinationWidth - 1);
            float x2 = (u * originalWidth) - 0.5f;
            int xint = (int)x2;
            float xfract = x2 - MathF.Floor(x2);

            float y2 = (v * originalHeight) - 0.5f;
            int yint = (int)y2;
            float yfract = y2 - MathF.Floor(y2);

            // 1st row
            Rgb p00 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint - 1, yint - 1);
            Rgb p01 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 0, yint - 1);
            Rgb p02 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 1, yint - 1);
            Rgb p03 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 2, yint - 1);

            // 2nd row
            Rgb p10 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint - 1, yint + 0);
            Rgb p11 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 0, yint + 0);
            Rgb p12 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 1, yint + 0);
            Rgb p13 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 2, yint + 0);

            // 3rd row
            Rgb p20 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint - 1, yint + 1);
            Rgb p21 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 0, yint + 1);
            Rgb p22 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 1, yint + 1);
            Rgb p23 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 2, yint + 1);

            // 4th row
            Rgb p30 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint - 1, yint + 2);
            Rgb p31 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 0, yint + 2);
            Rgb p32 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 1, yint + 2);
            Rgb p33 = GetPixelClamped_Rgb48(rgb48Src, originalWidth, originalHeight, xint + 2, yint + 2);

            // Red channel
            float col0R = CubicHermite(p00.Red, p10.Red, p20.Red, p30.Red, yfract);
            float col1R = CubicHermite(p01.Red, p11.Red, p21.Red, p31.Red, yfract);
            float col2R = CubicHermite(p02.Red, p12.Red, p22.Red, p32.Red, yfract);
            float col3R = CubicHermite(p03.Red, p13.Red, p23.Red, p33.Red, yfract);
            float valueR = CubicHermite(col0R, col1R, col2R, col3R, xfract);
            valueR = Math.Clamp(valueR, 0.0f, 65535.0f);
            // Green channel
            float col0G = CubicHermite(p00.Green, p10.Green, p20.Green, p30.Green, yfract);
            float col1G = CubicHermite(p01.Green, p11.Green, p21.Green, p31.Green, yfract);
            float col2G = CubicHermite(p02.Green, p12.Green, p22.Green, p32.Green, yfract);
            float col3G = CubicHermite(p03.Green, p13.Green, p23.Green, p33.Green, yfract);
            float valueG = CubicHermite(col0G, col1G, col2G, col3G, xfract);
            valueG = Math.Clamp(valueG, 0.0f, 65535.0f);
            // Blue channel
            float col0B = CubicHermite(p00.Blue, p10.Blue, p20.Blue, p30.Blue, yfract);
            float col1B = CubicHermite(p01.Blue, p11.Blue, p21.Blue, p31.Blue, yfract);
            float col2B = CubicHermite(p02.Blue, p12.Blue, p22.Blue, p32.Blue, yfract);
            float col3B = CubicHermite(p03.Blue, p13.Blue, p23.Blue, p33.Blue, yfract);
            float valueB = CubicHermite(col0B, col1B, col2B, col3B, xfract);
            valueB = Math.Clamp(valueB, 0.0f, 65535.0f);
            // combine
            rgb48Dest[(destY * destinationWidth) + destX] = new Rgb48((ushort)valueR, (ushort)valueG, (ushort)valueB);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBicubicSingle(ReadOnlySpan<float> byteSrc, Span<float> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bicubic_interpolation

            int destX = xDest;
            int destY = yDest;

            float v = destY / (float)(destinationHeight - 1);
            float u = destX / (float)(destinationWidth - 1);
            float x2 = (u * originalWidth) - 0.5f;
            int xint = (int)x2;
            float xfract = x2 - MathF.Floor(x2);

            float y2 = (v * originalHeight) - 0.5f;
            int yint = (int)y2;
            float yfract = y2 - MathF.Floor(y2);

            // 1st row
            float p00 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint - 1, yint - 1);
            float p01 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 0, yint - 1);
            float p02 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 1, yint - 1);
            float p03 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 2, yint - 1);

            // 2nd row
            float p10 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint - 1, yint + 0);
            float p11 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 0, yint + 0);
            float p12 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 1, yint + 0);
            float p13 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 2, yint + 0);

            // 3rd row
            float p20 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint - 1, yint + 1);
            float p21 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 0, yint + 1);
            float p22 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 1, yint + 1);
            float p23 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 2, yint + 1);

            // 4th row
            float p30 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint - 1, yint + 2);
            float p31 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 0, yint + 2);
            float p32 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 1, yint + 2);
            float p33 = GetPixelClamped_Float(byteSrc, originalWidth, originalHeight, xint + 2, yint + 2);

            float col0 = CubicHermite(p00, p10, p20, p30, yfract);
            float col1 = CubicHermite(p01, p11, p21, p31, yfract);
            float col2 = CubicHermite(p02, p12, p22, p32, yfract);
            float col3 = CubicHermite(p03, p13, p23, p33, yfract);
            float value = CubicHermite(col0, col1, col2, col3, xfract);
            value = Math.Clamp(value, 0.0f, 1.0f);
            int dstIndex = Math.Clamp((destY * destinationWidth) + destX, 0, byteDest.Length - 1);
            byteDest[dstIndex] = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBicubicUInt16(ReadOnlySpan<ushort> byteSrc, Span<ushort> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bicubic_interpolation

            int destX = xDest;
            int destY = yDest;

            float v = destY / (float)(destinationHeight - 1);
            float u = destX / (float)(destinationWidth - 1);
            float x2 = (u * originalWidth) - 0.5f;
            int xint = (int)x2;
            float xfract = x2 - MathF.Floor(x2);

            float y2 = (v * originalHeight) - 0.5f;
            int yint = (int)y2;
            float yfract = y2 - MathF.Floor(y2);

            // 1st row
            float p00 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint - 1, yint - 1);
            float p01 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 0, yint - 1);
            float p02 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 1, yint - 1);
            float p03 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 2, yint - 1);

            // 2nd row
            float p10 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint - 1, yint + 0);
            float p11 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 0, yint + 0);
            float p12 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 1, yint + 0);
            float p13 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 2, yint + 0);

            // 3rd row
            float p20 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint - 1, yint + 1);
            float p21 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 0, yint + 1);
            float p22 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 1, yint + 1);
            float p23 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 2, yint + 1);

            // 4th row
            float p30 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint - 1, yint + 2);
            float p31 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 0, yint + 2);
            float p32 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 1, yint + 2);
            float p33 = GetPixelClamped_Ushort(byteSrc, originalWidth, originalHeight, xint + 2, yint + 2);

            float col0 = CubicHermite(p00, p10, p20, p30, yfract);
            float col1 = CubicHermite(p01, p11, p21, p31, yfract);
            float col2 = CubicHermite(p02, p12, p22, p32, yfract);
            float col3 = CubicHermite(p03, p13, p23, p33, yfract);
            float value = CubicHermite(col0, col1, col2, col3, xfract);
            value = Math.Clamp(value, 0.0f, 65535.0f);
            int dstIndex = Math.Clamp((destY * destinationWidth) + destX, 0, byteDest.Length - 1);
            byteDest[dstIndex] = (ushort)value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBilinearByte(ReadOnlySpan<byte> byteSrc, Span<byte> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // See https://en.wikipedia.org/wiki/Bilinear_interpolation

            int destX = xDest;
            int destY = yDest;

            float gx = ((float)xDest) / destinationWidth * (originalWidth - 1);
            float gy = ((float)yDest) / destinationHeight * (originalHeight - 1);
            int gxi0 = (int)Math.Floor(gx);
            int gyi0 = (int)Math.Floor(gy);
            int gxi1 = Math.Clamp(gxi0 + 1, 0, originalWidth - 1);
            int gyi1 = Math.Clamp(gyi0 + 1, 0, originalHeight - 1);
            int srcIndex = (gyi0 * originalWidth) + gxi0;
            int srcIndexX1 = (gyi0 * originalWidth) + gxi1;
            int srcIndexY1 = (gyi1 * originalWidth) + gxi0;
            int srcIndexXY1 = (gyi1 * originalWidth) + gxi1;
            float c00 = byteSrc[srcIndex];
            float c10 = byteSrc[srcIndexX1];
            float c01 = byteSrc[srcIndexY1];
            float c11 = byteSrc[srcIndexXY1];

            byte value = (byte)Blerp(c00, c10, c01, c11, gx - gxi0, gy - gyi0);
            int dstIndex = (destY * destinationWidth) + destX;
            byteDest[dstIndex] = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBilinearRgb(ReadOnlySpan<Rgb> byteSrc, Span<Rgb> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bilinear_interpolation

            int destX = xDest;
            int destY = yDest;

            float gx = ((float)destX) / destinationWidth * (originalWidth - 1);
            float gy = ((float)destY) / destinationHeight * (originalHeight - 1);
            int gxi0 = (int)Math.Floor(gx);
            int gyi0 = (int)Math.Floor(gy);
            int gxi1 = Math.Clamp(gxi0 + 1, 0, originalWidth - 1);
            int gyi1 = Math.Clamp(gyi0 + 1, 0, originalHeight - 1);
            int srcIndex = (gyi0 * originalWidth) + gxi0;
            int srcIndexX1 = (gyi0 * originalWidth) + gxi1;
            int srcIndexY1 = (gyi1 * originalWidth) + gxi0;
            int srcIndexXY1 = (gyi1 * originalWidth) + gxi1;
            float c00R = byteSrc[srcIndex].Red;
            float c00G = byteSrc[srcIndex].Green;
            float c00B = byteSrc[srcIndex].Blue;
            float c10R = byteSrc[srcIndexX1].Red;
            float c10G = byteSrc[srcIndexX1].Green;
            float c10B = byteSrc[srcIndexX1].Blue;
            float c01R = byteSrc[srcIndexY1].Red;
            float c01G = byteSrc[srcIndexY1].Green;
            float c01B = byteSrc[srcIndexY1].Blue;
            float c11R = byteSrc[srcIndexXY1].Red;
            float c11G = byteSrc[srcIndexXY1].Green;
            float c11B = byteSrc[srcIndexXY1].Blue;

            float valueR = Blerp(c00R, c10R, c01R, c11R, gx - gxi0, gy - gyi0);
            float valueG = Blerp(c00G, c10G, c01G, c11G, gx - gxi0, gy - gyi0);
            float valueB = Blerp(c00B, c10B, c01B, c11B, gx - gxi0, gy - gyi0);
            int dstIndex = (destY * destinationWidth) + destX;
            byteDest[dstIndex] = new Rgb(valueR, valueG, valueB);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBilinearRgb24(ReadOnlySpan<Rgb24> byteSrc, Span<Rgb24> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bilinear_interpolation

            int destX = xDest;
            int destY = yDest;

            float gx = ((float)destX) / destinationWidth * (originalWidth - 1);
            float gy = ((float)destY) / destinationHeight * (originalHeight - 1);
            int gxi0 = (int)Math.Floor(gx);
            int gyi0 = (int)Math.Floor(gy);
            int gxi1 = Math.Clamp(gxi0 + 1, 0, originalWidth - 1);
            int gyi1 = Math.Clamp(gyi0 + 1, 0, originalHeight - 1);
            int srcIndex = (gyi0 * originalWidth) + gxi0;
            int srcIndexX1 = (gyi0 * originalWidth) + gxi1;
            int srcIndexY1 = (gyi1 * originalWidth) + gxi0;
            int srcIndexXY1 = (gyi1 * originalWidth) + gxi1;
            float c00R = byteSrc[srcIndex].Red;
            float c00G = byteSrc[srcIndex].Green;
            float c00B = byteSrc[srcIndex].Blue;
            float c10R = byteSrc[srcIndexX1].Red;
            float c10G = byteSrc[srcIndexX1].Green;
            float c10B = byteSrc[srcIndexX1].Blue;
            float c01R = byteSrc[srcIndexY1].Red;
            float c01G = byteSrc[srcIndexY1].Green;
            float c01B = byteSrc[srcIndexY1].Blue;
            float c11R = byteSrc[srcIndexXY1].Red;
            float c11G = byteSrc[srcIndexXY1].Green;
            float c11B = byteSrc[srcIndexXY1].Blue;

            byte valueR = (byte)Blerp(c00R, c10R, c01R, c11R, gx - gxi0, gy - gyi0);
            byte valueG = (byte)Blerp(c00G, c10G, c01G, c11G, gx - gxi0, gy - gyi0);
            byte valueB = (byte)Blerp(c00B, c10B, c01B, c11B, gx - gxi0, gy - gyi0);
            int dstIndex = (destY * destinationWidth) + destX;
            byteDest[dstIndex] = new Rgb24(valueR, valueG, valueB);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBilinearRgb48(ReadOnlySpan<Rgb48> byteSrc, Span<Rgb48> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bilinear_interpolation

            int destX = xDest;
            int destY = yDest;

            float gx = ((float)destX) / destinationWidth * (originalWidth - 1);
            float gy = ((float)destY) / destinationHeight * (originalHeight - 1);
            int gxi0 = (int)Math.Floor(gx);
            int gyi0 = (int)Math.Floor(gy);
            int gxi1 = Math.Clamp(gxi0 + 1, 0, originalWidth - 1);
            int gyi1 = Math.Clamp(gyi0 + 1, 0, originalHeight - 1);
            int srcIndex = (gyi0 * originalWidth) + gxi0;
            int srcIndexX1 = (gyi0 * originalWidth) + gxi1;
            int srcIndexY1 = (gyi1 * originalWidth) + gxi0;
            int srcIndexXY1 = (gyi1 * originalWidth) + gxi1;
            float c00R = byteSrc[srcIndex].Red;
            float c00G = byteSrc[srcIndex].Green;
            float c00B = byteSrc[srcIndex].Blue;
            float c10R = byteSrc[srcIndexX1].Red;
            float c10G = byteSrc[srcIndexX1].Green;
            float c10B = byteSrc[srcIndexX1].Blue;
            float c01R = byteSrc[srcIndexY1].Red;
            float c01G = byteSrc[srcIndexY1].Green;
            float c01B = byteSrc[srcIndexY1].Blue;
            float c11R = byteSrc[srcIndexXY1].Red;
            float c11G = byteSrc[srcIndexXY1].Green;
            float c11B = byteSrc[srcIndexXY1].Blue;

            ushort valueR = (ushort)Blerp(c00R, c10R, c01R, c11R, gx - gxi0, gy - gyi0);
            ushort valueG = (ushort)Blerp(c00G, c10G, c01G, c11G, gx - gxi0, gy - gyi0);
            ushort valueB = (ushort)Blerp(c00B, c10B, c01B, c11B, gx - gxi0, gy - gyi0);
            int dstIndex = (destY * destinationWidth) + destX;
            byteDest[dstIndex] = new Rgb48(valueR, valueG, valueB);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBilinearSingle(ReadOnlySpan<float> byteSrc, Span<float> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bilinear_interpolation

            int destX = xDest;
            int destY = yDest;

            float gx = ((float)xDest) / destinationWidth * (originalWidth - 1);
            float gy = ((float)yDest) / destinationHeight * (originalHeight - 1);
            int gxi0 = (int)Math.Floor(gx);
            int gyi0 = (int)Math.Floor(gy);
            int gxi1 = Math.Clamp(gxi0 + 1, 0, originalWidth - 1);
            int gyi1 = Math.Clamp(gyi0 + 1, 0, originalHeight - 1);
            int srcIndex = (gyi0 * originalWidth) + gxi0;
            int srcIndexX1 = (gyi0 * originalWidth) + gxi1;
            int srcIndexY1 = (gyi1 * originalWidth) + gxi0;
            int srcIndexXY1 = (gyi1 * originalWidth) + gxi1;
            float c00 = byteSrc[srcIndex];
            float c10 = byteSrc[srcIndexX1];
            float c01 = byteSrc[srcIndexY1];
            float c11 = byteSrc[srcIndexXY1];

            float value = (float)Blerp(c00, c10, c01, c11, gx - gxi0, gy - gyi0);
            int dstIndex = (destY * destinationWidth) + destX;
            byteDest[dstIndex] = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeBilinearUInt16(ReadOnlySpan<ushort> byteSrc, Span<ushort> byteDest, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            // see https://en.wikipedia.org/wiki/Bilinear_interpolation

            int destX = xDest;
            int destY = yDest;

            float gx = ((float)xDest) / destinationWidth * (originalWidth - 1);
            float gy = ((float)yDest) / destinationHeight * (originalHeight - 1);
            int gxi0 = (int)Math.Floor(gx);
            int gyi0 = (int)Math.Floor(gy);
            int gxi1 = Math.Clamp(gxi0 + 1, 0, originalWidth - 1);
            int gyi1 = Math.Clamp(gyi0 + 1, 0, originalHeight - 1);
            int srcIndex = (gyi0 * originalWidth) + gxi0;
            int srcIndexX1 = (gyi0 * originalWidth) + gxi1;
            int srcIndexY1 = (gyi1 * originalWidth) + gxi0;
            int srcIndexXY1 = (gyi1 * originalWidth) + gxi1;
            float c00 = byteSrc[srcIndex];
            float c10 = byteSrc[srcIndexX1];
            float c01 = byteSrc[srcIndexY1];
            float c11 = byteSrc[srcIndexXY1];

            ushort value = (ushort)Blerp(c00, c10, c01, c11, gx - gxi0, gy - gyi0);
            int dstIndex = (destY * destinationWidth) + destX;
            byteDest[dstIndex] = value;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeNearestNeighborByte(ReadOnlySpan<byte> source, Span<byte> destination, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            int destX = xDest;
            int destY = yDest;

            int gxi = (int)((float)destX / destinationWidth * originalWidth);
            int gyi = (int)((float)destY / destinationHeight * originalHeight);

            int srcIndex = (gyi * originalWidth) + gxi;
            int dstIndex = (destY * destinationWidth) + destX;
            destination[dstIndex] = source[srcIndex];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeNearestNeighborRgb(ReadOnlySpan<Rgb> source, Span<Rgb> destination, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            int destX = xDest;
            int destY = yDest;

            int gxi = (int)((float)destX / destinationWidth * originalWidth);
            int gyi = (int)((float)destY / destinationHeight * originalHeight);

            int srcIndex = (gyi * originalWidth) + gxi;
            int dstIndex = (destY * destinationWidth) + destX;
            destination[dstIndex] = source[srcIndex];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeNearestNeighborRgb24(ReadOnlySpan<Rgb24> source, Span<Rgb24> destination, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            int destX = xDest;
            int destY = yDest;

            int gxi = (int)((float)destX / destinationWidth * originalWidth);
            int gyi = (int)((float)destY / destinationHeight * originalHeight);

            int srcIndex = (gyi * originalWidth) + gxi;
            int dstIndex = (destY * destinationWidth) + destX;
            destination[dstIndex] = source[srcIndex];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeNearestNeighborRgb48(ReadOnlySpan<Rgb48> source, Span<Rgb48> destination, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            int destX = xDest;
            int destY = yDest;

            int gxi = (int)((float)destX / destinationWidth * originalWidth);
            int gyi = (int)((float)destY / destinationHeight * originalHeight);

            int srcIndex = (gyi * originalWidth) + gxi;
            int dstIndex = (destY * destinationWidth) + destX;
            destination[dstIndex] = source[srcIndex];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeNearestNeighborSingle(ReadOnlySpan<float> source, Span<float> destination, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            int destX = xDest;
            int destY = yDest;

            int gxi = (int)((float)destX / destinationWidth * originalWidth);
            int gyi = (int)((float)destY / destinationHeight * originalHeight);

            int srcIndex = (gyi * originalWidth) + gxi;
            int dstIndex = (destY * destinationWidth) + destX;
            destination[dstIndex] = source[srcIndex];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void ResizeNearestNeighborUInt16(ReadOnlySpan<ushort> source, Span<ushort> destination, int originalWidth, int originalHeight, int destinationWidth, int destinationHeight, int yDest)
    {
        for (int xDest = 0; xDest < destinationWidth; xDest++)
        {
            int destX = xDest;
            int destY = yDest;

            int gxi = (int)((float)destX / destinationWidth * originalWidth);
            int gyi = (int)((float)destY / destinationHeight * originalHeight);

            int srcIndex = (gyi * originalWidth) + gxi;
            int dstIndex = (destY * destinationWidth) + destX;
            destination[dstIndex] = source[srcIndex];
        }
    }
}
