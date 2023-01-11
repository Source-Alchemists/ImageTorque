using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal sealed partial class Resizer : IProcessor<ResizerParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(ResizerParameters parameters)
    {
        Type inputType = parameters.Input!.GetType();

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Mono>))
        {
            return ResizeMono(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Mono8>))
        {
            return ResizeMono8(parameters);
        }

        if (inputType == typeof(ReadOnlyPackedPixelBuffer<Mono16>))
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

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<RgbFFF>))
        {
            return ResizeRgbFFF(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<Rgb888>))
        {
            return ResizeRgb888(parameters);
        }

        if (inputType == typeof(ReadOnlyPlanarPixelBuffer<Rgb161616>))
        {
            return ResizeRgb161616(parameters);
        }

        throw new NotSupportedException($"Resizing of type {inputType} is not supported.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Mono> ResizeMono(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PackedPixelBuffer<Mono>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Mono>)parameters.Input!;
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
    private static PackedPixelBuffer<Mono8> ResizeMono8(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PackedPixelBuffer<Mono8>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Mono8>)parameters.Input!;
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

    private static PackedPixelBuffer<Mono16> ResizeMono16(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PackedPixelBuffer<Mono16>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPackedPixelBuffer<Mono16>)parameters.Input!;
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
    private static PackedPixelBuffer<Rgb> ResizeRgb(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PackedPixelBuffer<Rgb>(parameters.Width, parameters.Height);
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
    private static PackedPixelBuffer<Rgb24> ResizeRgb24(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PackedPixelBuffer<Rgb24>(parameters.Width, parameters.Height);
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
    private static PackedPixelBuffer<Rgb48> ResizeRgb48(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PackedPixelBuffer<Rgb48>(parameters.Width, parameters.Height);
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
    private static PlanarPixelBuffer<RgbFFF> ResizeRgbFFF(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PlanarPixelBuffer<RgbFFF>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input!;
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
    private static PlanarPixelBuffer<Rgb888> ResizeRgb888(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PlanarPixelBuffer<Rgb888>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input!;
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
    private static PlanarPixelBuffer<Rgb161616> ResizeRgb161616(ResizerParameters parameters)
    {
        var targetPixelBuffer = new PlanarPixelBuffer<Rgb161616>(parameters.Width, parameters.Height);
        var sourceBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input!;
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
