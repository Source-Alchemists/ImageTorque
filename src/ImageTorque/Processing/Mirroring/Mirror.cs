using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal sealed class Mirror : IProcessor<MirrorParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(MirrorParameters parameters)
    {
        Type inputType = parameters.Input!.GetType();

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<LS>))
        {
            return MirrorMono(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L8>))
        {
            return MirrorMono8(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L16>))
        {
            return MirrorMono16(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            return MirrorRgb(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return MirrorRgb24(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return MirrorRgb48(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<LS>))
        {
            return MirrorRgbFFF(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L8>))
        {
            return MirrorRgb888(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L16>))
        {
            return MirrorRgb161616(parameters);
        }

        throw new NotSupportedException($"The input type is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<LS> MirrorMono(MirrorParameters parameters) => MirrorPacked((ReadOnlyPackedPixelBuffer<LS>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<L8> MirrorMono8(MirrorParameters parameters) => MirrorPacked((ReadOnlyPackedPixelBuffer<L8>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<L16> MirrorMono16(MirrorParameters parameters) => MirrorPacked((ReadOnlyPackedPixelBuffer<L16>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<Rgb> MirrorRgb(MirrorParameters parameters) => MirrorPacked((ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<Rgb24> MirrorRgb24(MirrorParameters parameters) => MirrorPacked((ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<Rgb48> MirrorRgb48(MirrorParameters parameters) => MirrorPacked((ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PlanarPixelBuffer<LS> MirrorRgbFFF(MirrorParameters parameters) => MirrorPlanar((ReadOnlyPlanarPixelBuffer<LS>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PlanarPixelBuffer<L8> MirrorRgb888(MirrorParameters parameters) => MirrorPlanar((ReadOnlyPlanarPixelBuffer<L8>)parameters.Input!, parameters);
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PlanarPixelBuffer<L16> MirrorRgb161616(MirrorParameters parameters) => MirrorPlanar((ReadOnlyPlanarPixelBuffer<L16>)parameters.Input!, parameters);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PixelBuffer<TPixel> MirrorPacked<TPixel>(ReadOnlyPackedPixelBuffer<TPixel> sourcePixelBuffer, MirrorParameters parameters)
        where TPixel : unmanaged, IPixel
    {
        MirrorMode mode = parameters.MirrorMode;
        var targetPixelBuffer = new PixelBuffer<TPixel>(sourcePixelBuffer.Width, sourcePixelBuffer.Height);

        switch (mode)
        {
            case MirrorMode.Horizontal:
                _ = Parallel.For(0, sourcePixelBuffer.Height, parameters.ParallelOptions, rowIndex =>
                {
                    ReadOnlySpan<TPixel> sourceRow = sourcePixelBuffer.GetRow(rowIndex);
                    Span<TPixel> targetRow = targetPixelBuffer.GetRow(rowIndex);
                    int z = targetRow.Length - 1;
                    for (int x = 0; x < sourceRow.Length; x++)
                    {
                        targetRow[z--] = sourceRow[x];
                    }
                });
                break;
            case MirrorMode.Vertical:
                int lastColumnIndex = sourcePixelBuffer.Height - 1;
                Parallel.For(0, sourcePixelBuffer.Height, parameters.ParallelOptions, rowIndex =>
                {
                    sourcePixelBuffer.GetRow(rowIndex).CopyTo(targetPixelBuffer.GetRow(lastColumnIndex - rowIndex));
                });
                break;
            case MirrorMode.VerticalHorizontal:
                Parallel.For(0, sourcePixelBuffer.Height, parameters.ParallelOptions, rowIndex =>
                {
                    int stride = rowIndex * sourcePixelBuffer.Width;
                    int tPos = (targetPixelBuffer.Height - rowIndex) * targetPixelBuffer.Width - 1;
                    for (int x = 0; x < targetPixelBuffer.Width; x++)
                    {
                        targetPixelBuffer.Pixels[tPos - x] = sourcePixelBuffer.Pixels[stride + x];
                    }
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static PlanarPixelBuffer<TPixel> MirrorPlanar<TPixel>(ReadOnlyPlanarPixelBuffer<TPixel> sourcePixelBuffer, MirrorParameters parameters)
        where TPixel : unmanaged, IPixel
    {
        MirrorMode mode = parameters.MirrorMode;
        var targetPixelBuffer = new PlanarPixelBuffer<TPixel>(sourcePixelBuffer.Width, sourcePixelBuffer.Height);

        switch (mode)
        {
            case MirrorMode.Horizontal:
                for (int channelIndex = 0; channelIndex < sourcePixelBuffer.NumberOfChannels; channelIndex++)
                {
                    _ = Parallel.For(0, sourcePixelBuffer.Height, parameters.ParallelOptions, rowIndex =>
                    {
                        ReadOnlySpan<TPixel> sourceRow = sourcePixelBuffer.GetRow(channelIndex, rowIndex);
                        Span<TPixel> targetRow = targetPixelBuffer.GetRow(channelIndex, rowIndex);
                        int z = targetRow.Length - 1;
                        for (int x = 0; x < sourceRow.Length; x++)
                        {
                            targetRow[z--] = sourceRow[x];
                        }
                    });
                }
                break;
            case MirrorMode.Vertical:
                for (int channelIndex = 0; channelIndex < sourcePixelBuffer.NumberOfChannels; channelIndex++)
                {
                    int lastColumnIndex = sourcePixelBuffer.Height - 1;
                    Parallel.For(0, sourcePixelBuffer.Height, rowIndex =>
                    {
                        sourcePixelBuffer.GetRow(channelIndex, rowIndex).CopyTo(targetPixelBuffer.GetRow(channelIndex, lastColumnIndex - rowIndex));
                    });
                }
                break;
            case MirrorMode.VerticalHorizontal:
                for (int channelIndex = 0; channelIndex < sourcePixelBuffer.NumberOfChannels; channelIndex++)
                {
                    Parallel.For(0, sourcePixelBuffer.Height, rowIndex =>
                    {
                        int stride = rowIndex * sourcePixelBuffer.Width;
                        int tPos = (targetPixelBuffer.Height - rowIndex) * targetPixelBuffer.Width - 1;
                        for (int x = 0; x < targetPixelBuffer.Width; x++)
                        {
                            targetPixelBuffer.GetChannel(channelIndex)[tPos - x] = sourcePixelBuffer.GetChannel(channelIndex)[stride + x];
                        }
                    });
                }
                break;
        }
        return targetPixelBuffer;
    }
}
