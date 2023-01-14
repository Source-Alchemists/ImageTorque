using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal partial class Binarizer
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void BinarizeMono(ReadOnlySpan<LF> sourceRow, Span<LF> targetRow, ReadOnlyPackedPixelBuffer<LF> sourceBuffer, float threshold, BinaryThresholdMode mode)
    {
        int width = sourceBuffer.Width;
        float realThreshold = threshold;
        switch (mode)
        {
            case BinaryThresholdMode.Lumincance:
            case BinaryThresholdMode.Saturation:
            case BinaryThresholdMode.MaxChroma:
                for (int x = 0; x < width; x++)
                {
                    targetRow[x] = sourceRow[x] >= realThreshold ? LF.White : LF.Black;
                }
                break;

        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void BinarizeMono8(ReadOnlySpan<L8> sourceRow, Span<L8> targetRow, ReadOnlyPackedPixelBuffer<L8> sourceBuffer, float threshold, BinaryThresholdMode mode)
    {
        int width = sourceBuffer.Width;
        float realThreshold = threshold * L8.White;
        switch (mode)
        {
            case BinaryThresholdMode.Lumincance:
            case BinaryThresholdMode.Saturation:
            case BinaryThresholdMode.MaxChroma:
                for (int x = 0; x < width; x++)
                {
                    targetRow[x] = sourceRow[x] >= realThreshold ? L8.White : L8.Black;
                }
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void BinarizeMono16(ReadOnlySpan<L16> sourceRow, Span<L8> targetRow, ReadOnlyPackedPixelBuffer<L16> sourceBuffer, float threshold, BinaryThresholdMode mode)
    {
        int width = sourceBuffer.Width;
        float realThreshold = threshold * L8.White;
        switch (mode)
        {
            case BinaryThresholdMode.Lumincance:
            case BinaryThresholdMode.Saturation:
            case BinaryThresholdMode.MaxChroma:
                for (int x = 0; x < width; x++)
                {
                    targetRow[x] = sourceRow[x] >= realThreshold ? L8.White : L8.Black;
                }
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void BinarizeRgb(ReadOnlySpan<Rgb> sourceRow, Span<LF> targetRow, ReadOnlyPackedPixelBuffer<Rgb> sourceBuffer, float threshold, BinaryThresholdMode mode)
    {
        int width = sourceBuffer.Width;
        float realThreshold = threshold;
        switch (mode)
        {
            case BinaryThresholdMode.Lumincance:
                for (int x = 0; x < width; x++)
                {
                    Rgb pixel = sourceRow[x];
                    ushort luminance = ColorNumerics.Get16BitBT709Luminance(pixel.Red, pixel.Green, pixel.Blue);
                    targetRow[x] = luminance >= realThreshold ? L8.White : L8.Black;
                }
                break;
            case BinaryThresholdMode.Saturation:
                for (int x = 0; x < width; x++)
                {
                    float saturation = ColorNumerics.GetSaturation(sourceRow[x]);
                    ref LF color = ref targetRow[x];
                    color = saturation >= threshold ? L8.White : L8.Black;
                }
                break;
            case BinaryThresholdMode.MaxChroma:
                float ct = realThreshold / 2f;
                for (int x = 0; x < width; x++)
                {
                    float maxChroma = ColorNumerics.GetMaxChroma(sourceRow[x]);
                    ref LF color = ref targetRow[x];
                    color = maxChroma >= ct ? L8.White : L8.Black;
                }
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void BinarizeRgb24(ReadOnlySpan<Rgb24> sourceRow, Span<L8> targetRow, ReadOnlyPackedPixelBuffer<Rgb24> sourceBuffer, float threshold, BinaryThresholdMode mode)
    {
        int width = sourceBuffer.Width;
        float realThreshold = threshold * L8.White;
        switch (mode)
        {
            case BinaryThresholdMode.Lumincance:
                for (int x = 0; x < width; x++)
                {
                    Rgb24 pixel = sourceRow[x];
                    byte luminance = ColorNumerics.Get8BitBT709Luminance(pixel.Red, pixel.Green, pixel.Blue);
                    targetRow[x] = luminance >= realThreshold ? L8.White : L8.Black;
                }
                break;
            case BinaryThresholdMode.Saturation:
                for (int x = 0; x < width; x++)
                {
                    float saturation = ColorNumerics.GetSaturation(sourceRow[x]);
                    ref L8 color = ref targetRow[x];
                    color = saturation >= threshold ? L8.White : L8.Black;
                }
                break;
            case BinaryThresholdMode.MaxChroma:
                float ct = realThreshold / 2f;
                for (int x = 0; x < width; x++)
                {
                    float maxChroma = ColorNumerics.GetMaxChroma(sourceRow[x]);
                    ref L8 color = ref targetRow[x];
                    color = maxChroma >= ct ? L8.White : L8.Black;
                }
                break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static void BinarizeRgb48(ReadOnlySpan<Rgb48> sourceRow, Span<L8> targetRow, ReadOnlyPackedPixelBuffer<Rgb48> sourceBuffer, float threshold, BinaryThresholdMode mode)
    {
        int width = sourceBuffer.Width;
        float realThreshold = threshold * L8.White;
        switch (mode)
        {
            case BinaryThresholdMode.Lumincance:
                for (int x = 0; x < width; x++)
                {
                    Rgb48 pixel = sourceRow[x];
                    ushort luminance = ColorNumerics.Get16BitBT709Luminance(pixel.Red, pixel.Green, pixel.Blue);
                    targetRow[x] = luminance >= realThreshold ? L8.White : L8.Black;
                }
                break;
            case BinaryThresholdMode.Saturation:
                for (int x = 0; x < width; x++)
                {
                    float saturation = ColorNumerics.GetSaturation(sourceRow[x]);
                    ref L8 color = ref targetRow[x];
                    color = saturation >= threshold ? L8.White : L8.Black;
                }
                break;
            case BinaryThresholdMode.MaxChroma:
                float ct = realThreshold / 2f;
                for (int x = 0; x < width; x++)
                {
                    float maxChroma = ColorNumerics.GetMaxChroma(sourceRow[x]);
                    ref L8 color = ref targetRow[x];
                    color = maxChroma >= ct ? L8.White : L8.Black;
                }
                break;
        }
    }
}
