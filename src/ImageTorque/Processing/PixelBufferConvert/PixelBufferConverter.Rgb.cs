using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

public partial class PixelBufferConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPackedRgb(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PackedPixelBuffer<Rgb24>))
        {
            return RgbToRgb24(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb48>))
        {
            return RgbToRgb48(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<Rgb161616>))
        {
            return RgbToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<Rgb888>))
        {
            return RgbToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<RgbFFF>))
        {
            return RgbToRgbFFF(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPackedRgb24(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PackedPixelBuffer<Rgb>))
        {
            return Rgb24ToRgb(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb48>))
        {
            return Rgb24ToRgb48(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<Rgb161616>))
        {
            return Rgb24ToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<Rgb888>))
        {
            return Rgb24ToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<RgbFFF>))
        {
            return Rgb24ToRgbFFF(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPackedRgb48(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PackedPixelBuffer<Rgb>))
        {
            return Rgb48ToRgb(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb24>))
        {
            return Rgb48ToRgb24(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<Rgb161616>))
        {
            return Rgb48ToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<Rgb888>))
        {
            return Rgb48ToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<RgbFFF>))
        {
            return Rgb48ToRgbFFF(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPlanarRgbFFF(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PlanarPixelBuffer<Rgb161616>))
        {
            return RgbFFFToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<Rgb888>))
        {
            return RgbFFFToRgb888(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb>))
        {
            return RgbFFFToRgb(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb24>))
        {
            return RgbFFFToRgb24(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb48>))
        {
            return RgbFFFToRgb48(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPlanarRgb888(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PlanarPixelBuffer<Rgb161616>))
        {
            return Rgb888ToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<RgbFFF>))
        {
            return Rgb888ToRgbFFF(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb>))
        {
            return Rgb888ToRgb(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb24>))
        {
            return Rgb888ToRgb24(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb48>))
        {
            return Rgb888ToRgb48(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPlanarRgb161616(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PlanarPixelBuffer<Rgb888>))
        {
            return Rgb161616ToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<RgbFFF>))
        {
            return Rgb161616ToRgbFFF(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb>))
        {
            return Rgb161616ToRgb(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb24>))
        {
            return Rgb161616ToRgb24(parameters);
        }

        if (outputType == typeof(PackedPixelBuffer<Rgb48>))
        {
            return Rgb161616ToRgb48(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb24> RgbToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<Rgb24> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb24();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb48> RgbToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<Rgb48> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb48();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb> Rgb24ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<Rgb> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb48> Rgb24ToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<Rgb48> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb48();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb> Rgb48ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<Rgb> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb24> Rgb48ToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<Rgb24> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb24();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb161616> RgbToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<Rgb161616> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb161616> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb161616> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToUInt16(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToUInt16(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToUInt16(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb161616> Rgb24ToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<Rgb161616> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb161616> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb161616> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb24 pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToUInt16(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToUInt16(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToUInt16(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb161616> Rgb48ToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<Rgb161616> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb161616> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb161616> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb48 pixel = row[x];
                resultRowRed[x] = pixel.Red;
                resultRowGreen[x] = pixel.Green;
                resultRowBlue[x] = pixel.Blue;
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb888> RgbToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<Rgb888> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb888> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb888> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToByte(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToByte(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToByte(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb888> Rgb24ToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<Rgb888> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb888> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb888> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb24 pixel = row[x];
                resultRowRed[x] = pixel.Red;
                resultRowGreen[x] = pixel.Green;
                resultRowBlue[x] = pixel.Blue;
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb888> Rgb48ToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<Rgb888> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb888> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb888> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb48 pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToByte(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToByte(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToByte(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<RgbFFF> RgbToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<RgbFFF> resultRowRed = resultBuffer.GetRow(0, y);
            Span<RgbFFF> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<RgbFFF> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb pixel = row[x];
                resultRowRed[x] = pixel.Red;
                resultRowGreen[x] = pixel.Green;
                resultRowBlue[x] = pixel.Blue;
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<RgbFFF> Rgb24ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<RgbFFF> resultRowRed = resultBuffer.GetRow(0, y);
            Span<RgbFFF> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<RgbFFF> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb24 pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToSingle(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToSingle(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToSingle(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<RgbFFF> Rgb48ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<RgbFFF> resultRowRed = resultBuffer.GetRow(0, y);
            Span<RgbFFF> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<RgbFFF> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb48 pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToSingle(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToSingle(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToSingle(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb888> Rgb161616ToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb161616> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb161616> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb161616> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb888> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb888> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb888> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb888();
                resultRowGreen[x] = orgRowGreen[x].ToRgb888();
                resultRowBlue[x] = orgRowBlue[x].ToRgb888();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<RgbFFF> Rgb161616ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb161616> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb161616> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb161616> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<RgbFFF> resultRowRed = resultBuffer.GetRow(0, y);
            Span<RgbFFF> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<RgbFFF> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgbFFF();
                resultRowGreen[x] = orgRowGreen[x].ToRgbFFF();
                resultRowBlue[x] = orgRowBlue[x].ToRgbFFF();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb161616> Rgb888ToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb888> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb888> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb888> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb161616> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb161616> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb161616> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb161616();
                resultRowGreen[x] = orgRowGreen[x].ToRgb161616();
                resultRowBlue[x] = orgRowBlue[x].ToRgb161616();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<RgbFFF> Rgb888ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb888> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb888> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb888> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<RgbFFF> resultRowRed = resultBuffer.GetRow(0, y);
            Span<RgbFFF> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<RgbFFF> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgbFFF();
                resultRowGreen[x] = orgRowGreen[x].ToRgbFFF();
                resultRowBlue[x] = orgRowBlue[x].ToRgbFFF();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb161616> RgbFFFToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<RgbFFF> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<RgbFFF> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<RgbFFF> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb161616> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb161616> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb161616> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb161616();
                resultRowGreen[x] = orgRowGreen[x].ToRgb161616();
                resultRowBlue[x] = orgRowBlue[x].ToRgb161616();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<Rgb888> RgbFFFToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<RgbFFF> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<RgbFFF> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<RgbFFF> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb888> resultRowRed = resultBuffer.GetRow(0, y);
            Span<Rgb888> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<Rgb888> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb888();
                resultRowGreen[x] = orgRowGreen[x].ToRgb888();
                resultRowBlue[x] = orgRowBlue[x].ToRgb888();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb> Rgb161616ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb161616> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb161616> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb161616> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(Pixels.Convert.ToSingle(orgRowRed[x]),
                                        Pixels.Convert.ToSingle(orgRowGreen[x]),
                                        Pixels.Convert.ToSingle(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb24> Rgb161616ToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb161616> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb161616> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb161616> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb24> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(Pixels.Convert.ToByte(orgRowRed[x]),
                                        Pixels.Convert.ToByte(orgRowGreen[x]),
                                        Pixels.Convert.ToByte(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb48> Rgb161616ToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb161616> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb161616> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb161616> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb48> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(orgRowRed[x],
                                        orgRowGreen[x],
                                        orgRowBlue[x]);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb> Rgb888ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb888> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb888> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb888> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(Pixels.Convert.ToSingle(orgRowRed[x]),
                                        Pixels.Convert.ToSingle(orgRowGreen[x]),
                                        Pixels.Convert.ToSingle(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb24> Rgb888ToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb888> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb888> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb888> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb24> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(orgRowRed[x],
                                        orgRowGreen[x],
                                        orgRowBlue[x]);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb48> Rgb888ToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb888> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<Rgb888> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<Rgb888> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb48> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(Pixels.Convert.ToUInt16(orgRowRed[x]),
                                        Pixels.Convert.ToUInt16(orgRowGreen[x]),
                                        Pixels.Convert.ToUInt16(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb> RgbFFFToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<RgbFFF> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<RgbFFF> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<RgbFFF> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(orgRowRed[x],
                                        orgRowGreen[x],
                                        orgRowBlue[x]);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb24> RgbFFFToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<RgbFFF> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<RgbFFF> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<RgbFFF> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb24> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(Pixels.Convert.ToByte(orgRowRed[x]),
                                        Pixels.Convert.ToByte(orgRowGreen[x]),
                                        Pixels.Convert.ToByte(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PackedPixelBuffer<Rgb48> RgbFFFToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<RgbFFF> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<RgbFFF> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<RgbFFF> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb48> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(Pixels.Convert.ToUInt16(orgRowRed[x]),
                                        Pixels.Convert.ToUInt16(orgRowGreen[x]),
                                        Pixels.Convert.ToUInt16(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }
}
