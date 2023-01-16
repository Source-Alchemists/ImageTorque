using System.Runtime.CompilerServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal partial class PixelBufferConverter
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPackedRgb(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PixelBuffer<Rgb24>))
        {
            return RgbToRgb24(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb48>))
        {
            return RgbToRgb48(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<L16>))
        {
            return RgbToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<L8>))
        {
            return RgbToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<LS>))
        {
            return RgbToRgbFFF(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPackedRgb24(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PixelBuffer<Rgb>))
        {
            return Rgb24ToRgb(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb48>))
        {
            return Rgb24ToRgb48(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<L16>))
        {
            return Rgb24ToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<L8>))
        {
            return Rgb24ToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<LS>))
        {
            return Rgb24ToRgbFFF(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPackedRgb48(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PixelBuffer<Rgb>))
        {
            return Rgb48ToRgb(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb24>))
        {
            return Rgb48ToRgb24(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<L16>))
        {
            return Rgb48ToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<L8>))
        {
            return Rgb48ToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<LS>))
        {
            return Rgb48ToRgbFFF(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPlanarRgbFFF(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PlanarPixelBuffer<L16>))
        {
            return RgbFFFToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<L8>))
        {
            return RgbFFFToRgb888(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb>))
        {
            return RgbFFFToRgb(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb24>))
        {
            return RgbFFFToRgb24(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb48>))
        {
            return RgbFFFToRgb48(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPlanarRgb888(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PlanarPixelBuffer<L16>))
        {
            return Rgb888ToRgb161616(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<LS>))
        {
            return Rgb888ToRgbFFF(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb>))
        {
            return Rgb888ToRgb(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb24>))
        {
            return Rgb888ToRgb24(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb48>))
        {
            return Rgb888ToRgb48(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer ConvertPlanarRgb161616(PixelBufferConvertParameters parameters)
    {
        Type outputType = parameters.OutputType;

        if (outputType == typeof(PlanarPixelBuffer<L8>))
        {
            return Rgb161616ToRgb888(parameters);
        }

        if (outputType == typeof(PlanarPixelBuffer<LS>))
        {
            return Rgb161616ToRgbFFF(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb>))
        {
            return Rgb161616ToRgb(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb24>))
        {
            return Rgb161616ToRgb24(parameters);
        }

        if (outputType == typeof(PixelBuffer<Rgb48>))
        {
            return Rgb161616ToRgb48(parameters);
        }

        return null!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb24> RgbToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
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
    private static PixelBuffer<Rgb48> RgbToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
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
    private static PixelBuffer<Rgb> Rgb24ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
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
    private static PixelBuffer<Rgb48> Rgb24ToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
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
    private static PixelBuffer<Rgb> Rgb48ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
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
    private static PixelBuffer<Rgb24> Rgb48ToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
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
    private static PlanarPixelBuffer<L16> RgbToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<L16> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L16> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L16> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb pixel = row[x];
                resultRowRed[x] = PixelValueConverter.ToUInt16(pixel.Red);
                resultRowGreen[x] = PixelValueConverter.ToUInt16(pixel.Green);
                resultRowBlue[x] = PixelValueConverter.ToUInt16(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L16> Rgb24ToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<L16> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L16> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L16> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb24 pixel = row[x];
                resultRowRed[x] = PixelValueConverter.ToUInt16(pixel.Red);
                resultRowGreen[x] = PixelValueConverter.ToUInt16(pixel.Green);
                resultRowBlue[x] = PixelValueConverter.ToUInt16(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L16> Rgb48ToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<L16> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L16> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L16> resultRowBlue = resultBuffer.GetRow(2, y);
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
    private static PlanarPixelBuffer<L8> RgbToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<L8> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L8> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L8> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb pixel = row[x];
                resultRowRed[x] = PixelValueConverter.ToByte(pixel.Red);
                resultRowGreen[x] = PixelValueConverter.ToByte(pixel.Green);
                resultRowBlue[x] = PixelValueConverter.ToByte(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L8> Rgb24ToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<L8> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L8> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L8> resultRowBlue = resultBuffer.GetRow(2, y);
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
    private static PlanarPixelBuffer<L8> Rgb48ToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<L8> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L8> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L8> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb48 pixel = row[x];
                resultRowRed[x] = PixelValueConverter.ToByte(pixel.Red);
                resultRowGreen[x] = PixelValueConverter.ToByte(pixel.Green);
                resultRowBlue[x] = PixelValueConverter.ToByte(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<LS> RgbToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<LS>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb> row = inputBuffer.GetRow(y);
            Span<LS> resultRowRed = resultBuffer.GetRow(0, y);
            Span<LS> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<LS> resultRowBlue = resultBuffer.GetRow(2, y);
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
    private static PlanarPixelBuffer<LS> Rgb24ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<LS>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb24> row = inputBuffer.GetRow(y);
            Span<LS> resultRowRed = resultBuffer.GetRow(0, y);
            Span<LS> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<LS> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb24 pixel = row[x];
                resultRowRed[x] = PixelValueConverter.ToSingle(pixel.Red);
                resultRowGreen[x] = PixelValueConverter.ToSingle(pixel.Green);
                resultRowBlue[x] = PixelValueConverter.ToSingle(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<LS> Rgb48ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<LS>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<Rgb48> row = inputBuffer.GetRow(y);
            Span<LS> resultRowRed = resultBuffer.GetRow(0, y);
            Span<LS> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<LS> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                Rgb48 pixel = row[x];
                resultRowRed[x] = PixelValueConverter.ToSingle(pixel.Red);
                resultRowGreen[x] = PixelValueConverter.ToSingle(pixel.Green);
                resultRowBlue[x] = PixelValueConverter.ToSingle(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L8> Rgb161616ToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L16> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L16> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L16> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<L8> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L8> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L8> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToL8();
                resultRowGreen[x] = orgRowGreen[x].ToL8();
                resultRowBlue[x] = orgRowBlue[x].ToL8();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<LS> Rgb161616ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<LS>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L16> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L16> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L16> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<LS> resultRowRed = resultBuffer.GetRow(0, y);
            Span<LS> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<LS> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToLS();
                resultRowGreen[x] = orgRowGreen[x].ToLS();
                resultRowBlue[x] = orgRowBlue[x].ToLS();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L16> Rgb888ToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L8> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L8> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L8> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<L16> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L16> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L16> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToL16();
                resultRowGreen[x] = orgRowGreen[x].ToL16();
                resultRowBlue[x] = orgRowBlue[x].ToL16();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<LS> Rgb888ToRgbFFF(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<LS>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L8> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L8> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L8> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<LS> resultRowRed = resultBuffer.GetRow(0, y);
            Span<LS> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<LS> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToLS();
                resultRowGreen[x] = orgRowGreen[x].ToLS();
                resultRowBlue[x] = orgRowBlue[x].ToLS();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L16> RgbFFFToRgb161616(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<LS>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L16>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<LS> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<LS> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<LS> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<L16> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L16> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L16> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToL16();
                resultRowGreen[x] = orgRowGreen[x].ToL16();
                resultRowBlue[x] = orgRowBlue[x].ToL16();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PlanarPixelBuffer<L8> RgbFFFToRgb888(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<LS>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<L8>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<LS> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<LS> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<LS> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<L8> resultRowRed = resultBuffer.GetRow(0, y);
            Span<L8> resultRowGreen = resultBuffer.GetRow(1, y);
            Span<L8> resultRowBlue = resultBuffer.GetRow(2, y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToL8();
                resultRowGreen[x] = orgRowGreen[x].ToL8();
                resultRowBlue[x] = orgRowBlue[x].ToL8();
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb> Rgb161616ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L16> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L16> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L16> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(PixelValueConverter.ToSingle(orgRowRed[x]),
                                        PixelValueConverter.ToSingle(orgRowGreen[x]),
                                        PixelValueConverter.ToSingle(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb24> Rgb161616ToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L16> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L16> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L16> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb24> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(PixelValueConverter.ToByte(orgRowRed[x]),
                                        PixelValueConverter.ToByte(orgRowGreen[x]),
                                        PixelValueConverter.ToByte(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb48> Rgb161616ToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L16>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L16> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L16> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L16> orgRowBlue = inputBuffer.GetRow(2, y);

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
    private static PixelBuffer<Rgb> Rgb888ToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L8> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L8> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L8> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(PixelValueConverter.ToSingle(orgRowRed[x]),
                                        PixelValueConverter.ToSingle(orgRowGreen[x]),
                                        PixelValueConverter.ToSingle(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb24> Rgb888ToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L8> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L8> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L8> orgRowBlue = inputBuffer.GetRow(2, y);

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
    private static PixelBuffer<Rgb48> Rgb888ToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<L8>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<L8> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<L8> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<L8> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb48> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(PixelValueConverter.ToUInt16(orgRowRed[x]),
                                        PixelValueConverter.ToUInt16(orgRowGreen[x]),
                                        PixelValueConverter.ToUInt16(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb> RgbFFFToRgb(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<LS>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<LS> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<LS> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<LS> orgRowBlue = inputBuffer.GetRow(2, y);

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
    private static PixelBuffer<Rgb24> RgbFFFToRgb24(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<LS>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<LS> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<LS> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<LS> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb24> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(PixelValueConverter.ToByte(orgRowRed[x]),
                                        PixelValueConverter.ToByte(orgRowGreen[x]),
                                        PixelValueConverter.ToByte(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PixelBuffer<Rgb48> RgbFFFToRgb48(PixelBufferConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<LS>)parameters.Input;
        var resultBuffer = new PixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        _ = Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            ReadOnlySpan<LS> orgRowRed = inputBuffer.GetRow(0, y);
            ReadOnlySpan<LS> orgRowGreen = inputBuffer.GetRow(1, y);
            ReadOnlySpan<LS> orgRowBlue = inputBuffer.GetRow(2, y);

            Span<Rgb48> resultRow = resultBuffer.GetRow(y);
            for (int x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(PixelValueConverter.ToUInt16(orgRowRed[x]),
                                        PixelValueConverter.ToUInt16(orgRowGreen[x]),
                                        PixelValueConverter.ToUInt16(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }
}
