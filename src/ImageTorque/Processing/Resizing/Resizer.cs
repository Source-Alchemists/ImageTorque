using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal sealed partial class Resizer : IProcessor<ResizerParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(ResizerParameters parameters)
    {
        Type inputType = parameters.Input!.GetType();

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<LF>))
        {
            return ResizeMono(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L8>))
        {
            return ResizeMono8(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<L16>))
        {
            return ResizeMono16(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb>))
        {
            return ResizeRgb(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb24>))
        {
            return ResizeRgb24(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Rgb48>))
        {
            return ResizeRgb48(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<LF>))
        {
            return ResizeRgbFFF(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L8>))
        {
            return ResizeRgb888(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<L16>))
        {
            return ResizeRgb161616(parameters);
        }

        throw new NotSupportedException($"Resizing of type {inputType} is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<LF> ResizeMono(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PixelBuffer<LF>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<LF>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborSingle(sourceBuffer.Pixels.AsSingle(),
                                                            targetPixelBuffer.Pixels.AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBilinearSingle(sourceBuffer.Pixels.AsSingle(),
                                                    targetPixelBuffer.Pixels.AsSingle(),
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBicubicSingle(sourceBuffer.Pixels.AsSingle(),
                                                    targetPixelBuffer.Pixels.AsSingle(),
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<L8> ResizeMono8(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PixelBuffer<L8>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<L8>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborByte(sourceBuffer.Pixels.AsByte(),
                                                            targetPixelBuffer.Pixels.AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBilinearByte(sourceBuffer.Pixels.AsByte(),
                                                    targetPixelBuffer.Pixels.AsByte(),
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBicubicByte(sourceBuffer.Pixels.AsByte(),
                                                    targetPixelBuffer.Pixels.AsByte(),
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    private static PixelBuffer<L16> ResizeMono16(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PixelBuffer<L16>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<L16>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborUInt16(sourceBuffer.Pixels.AsUInt16(),
                                                            targetPixelBuffer.Pixels.AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBilinearUInt16(sourceBuffer.Pixels.AsUInt16(),
                                                    targetPixelBuffer.Pixels.AsUInt16(),
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBicubicUInt16(sourceBuffer.Pixels.AsUInt16(),
                                                    targetPixelBuffer.Pixels.AsUInt16(),
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb> ResizeRgb(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PixelBuffer<Rgb>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborRgb(sourceBuffer.Pixels,
                                                            targetPixelBuffer.Pixels,
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBilinearRgb(sourceBuffer.Pixels,
                                                    targetPixelBuffer.Pixels,
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBicubicRgb(sourceBuffer.Pixels,
                                                    targetPixelBuffer.Pixels,
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb24> ResizeRgb24(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PixelBuffer<Rgb24>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborRgb24(sourceBuffer.Pixels,
                                                            targetPixelBuffer.Pixels,
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBilinearRgb24(sourceBuffer.Pixels,
                                                    targetPixelBuffer.Pixels,
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBicubicRgb24(sourceBuffer.Pixels,
                                                    targetPixelBuffer.Pixels,
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb48> ResizeRgb48(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PixelBuffer<Rgb48>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborRgb48(sourceBuffer.Pixels,
                                                            targetPixelBuffer.Pixels,
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBilinearRgb48(sourceBuffer.Pixels,
                                                    targetPixelBuffer.Pixels,
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBicubicRgb48(sourceBuffer.Pixels,
                                                    targetPixelBuffer.Pixels,
                                                    sourceBuffer.Width,
                                                    sourceBuffer.Height,
                                                    targetPixelBuffer.Width,
                                                    targetPixelBuffer.Height,
                                                    row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<LF> ResizeRgbFFF(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PlanarPixelBuffer<LF>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<LF>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborSingle(sourceBuffer.GetChannel(0).AsSingle(),
                                                            targetPixelBuffer.GetChannel(0).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborSingle(sourceBuffer.GetChannel(1).AsSingle(),
                                                            targetPixelBuffer.GetChannel(1).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborSingle(sourceBuffer.GetChannel(2).AsSingle(),
                                                            targetPixelBuffer.GetChannel(2).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBilinearSingle(sourceBuffer.GetChannel(0).AsSingle(),
                                                            targetPixelBuffer.GetChannel(0).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeBilinearSingle(sourceBuffer.GetChannel(1).AsSingle(),
                                                            targetPixelBuffer.GetChannel(1).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeBilinearSingle(sourceBuffer.GetChannel(2).AsSingle(),
                                                            targetPixelBuffer.GetChannel(2).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeBicubicSingle(sourceBuffer.GetChannel(0).AsSingle(),
                                                            targetPixelBuffer.GetChannel(0).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeBicubicSingle(sourceBuffer.GetChannel(1).AsSingle(),
                                                            targetPixelBuffer.GetChannel(1).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeBicubicSingle(sourceBuffer.GetChannel(2).AsSingle(),
                                                            targetPixelBuffer.GetChannel(2).AsSingle(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L8> ResizeRgb888(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PlanarPixelBuffer<L8>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(0).AsByte(),
                                                            targetPixelBuffer.GetChannel(0).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(1).AsByte(),
                                                            targetPixelBuffer.GetChannel(1).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(2).AsByte(),
                                                            targetPixelBuffer.GetChannel(2).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(0).AsByte(),
                                                            targetPixelBuffer.GetChannel(0).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(1).AsByte(),
                                                            targetPixelBuffer.GetChannel(1).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(2).AsByte(),
                                                            targetPixelBuffer.GetChannel(2).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(0).AsByte(),
                                                            targetPixelBuffer.GetChannel(0).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(1).AsByte(),
                                                            targetPixelBuffer.GetChannel(1).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborByte(sourceBuffer.GetChannel(2).AsByte(),
                                                            targetPixelBuffer.GetChannel(2).AsByte(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
        }

        return targetPixelBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L16> ResizeRgb161616(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PlanarPixelBuffer<L16>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input!;
        switch (parameters.ResizeMode)
        {
            case ResizeMode.NearestNeighbor:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(0).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(0).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(1).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(1).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(2).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(2).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bilinear:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(0).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(0).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(1).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(1).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(2).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(2).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
            case ResizeMode.Bicubic:
                Parallel.For(0, targetPixelBuffer.Height, parameters.ParallelOptions, row =>
                {
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(0).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(0).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(1).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(1).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                    ResizeNearestNeighborUInt16(sourceBuffer.GetChannel(2).AsUInt16(),
                                                            targetPixelBuffer.GetChannel(2).AsUInt16(),
                                                            sourceBuffer.Width,
                                                            sourceBuffer.Height,
                                                            targetPixelBuffer.Width,
                                                            targetPixelBuffer.Height,
                                                            row);
                });
                break;
        }

        return targetPixelBuffer;
    }
}
