using ImageTorque.Pixels;

namespace ImageTorque.Buffers;

public class RgbConvertOperation : PixelBufferConverter
{
    public RgbConvertOperation()
    {
        // Packed to packed.
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb>, PackedPixelBuffer<Rgb24>>(RgbToRgb24);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb>, PackedPixelBuffer<Rgb48>>(RgbToRgb48);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb24>, PackedPixelBuffer<Rgb>>(Rgb24ToRgb);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb24>, PackedPixelBuffer<Rgb48>>(Rgb24ToRgb48);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb48>, PackedPixelBuffer<Rgb>>(Rgb48ToRgb);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb48>, PackedPixelBuffer<Rgb24>>(Rgb48ToRgb24);

        // Packed to planar.
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb>, PlanarPixelBuffer<Rgb161616>>(RgbToRgb161616);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb24>, PlanarPixelBuffer<Rgb161616>>(Rgb24ToRgb161616);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb48>, PlanarPixelBuffer<Rgb161616>>(Rgb48ToRgb161616);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb>, PlanarPixelBuffer<Rgb888>>(RgbToRgb888);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb24>, PlanarPixelBuffer<Rgb888>>(Rgb24ToRgb888);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb48>, PlanarPixelBuffer<Rgb888>>(Rgb48ToRgb888);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb>, PlanarPixelBuffer<RgbFFF>>(RgbToRgbFFF);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb24>, PlanarPixelBuffer<RgbFFF>>(Rgb24ToRgbFFF);
        AddOperation<ReadOnlyPackedPixelBuffer<Rgb48>, PlanarPixelBuffer<RgbFFF>>(Rgb48ToRgbFFF);

        // Planar to planar.
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb161616>, PlanarPixelBuffer<Rgb888>>(Rgb161616ToRgb888);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb161616>, PlanarPixelBuffer<RgbFFF>>(Rgb161616ToRgbFFF);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb888>, PlanarPixelBuffer<Rgb161616>>(Rgb888ToRgb161616);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb888>, PlanarPixelBuffer<RgbFFF>>(Rgb888ToRgbFFF);
        AddOperation<ReadOnlyPlanarPixelBuffer<RgbFFF>, PlanarPixelBuffer<Rgb161616>>(RgbFFFToRgb161616);
        AddOperation<ReadOnlyPlanarPixelBuffer<RgbFFF>, PlanarPixelBuffer<Rgb888>>(RgbFFFToRgb888);

        // Planar to packed.
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb161616>, PackedPixelBuffer<Rgb>>(Rgb161616ToRgb);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb161616>, PackedPixelBuffer<Rgb24>>(Rgb161616ToRgb24);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb161616>, PackedPixelBuffer<Rgb48>>(Rgb161616ToRgb48);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb888>, PackedPixelBuffer<Rgb>>(Rgb888ToRgb);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb888>, PackedPixelBuffer<Rgb24>>(Rgb888ToRgb24);
        AddOperation<ReadOnlyPlanarPixelBuffer<Rgb888>, PackedPixelBuffer<Rgb48>>(Rgb888ToRgb48);
        AddOperation<ReadOnlyPlanarPixelBuffer<RgbFFF>, PackedPixelBuffer<Rgb>>(RgbFFFToRgb);
        AddOperation<ReadOnlyPlanarPixelBuffer<RgbFFF>, PackedPixelBuffer<Rgb24>>(RgbFFFToRgb24);
        AddOperation<ReadOnlyPlanarPixelBuffer<RgbFFF>, PackedPixelBuffer<Rgb48>>(RgbFFFToRgb48);
    }

    private PackedPixelBuffer<Rgb24> RgbToRgb24(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb24();
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb48> RgbToRgb48(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb48();
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb> Rgb24ToRgb(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb();
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb48> Rgb24ToRgb48(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb48();
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb> Rgb48ToRgb(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb();
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb24> Rgb48ToRgb24(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = row[x].ToRgb24();
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb161616> RgbToRgb161616(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToUInt16(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToUInt16(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToUInt16(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb161616> Rgb24ToRgb161616(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToUInt16(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToUInt16(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToUInt16(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb161616> Rgb48ToRgb161616(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = pixel.Red;
                resultRowGreen[x] = pixel.Green;
                resultRowBlue[x] = pixel.Blue;
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb888> RgbToRgb888(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToByte(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToByte(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToByte(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb888> Rgb24ToRgb888(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = pixel.Red;
                resultRowGreen[x] = pixel.Green;
                resultRowBlue[x] = pixel.Blue;
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb888> Rgb48ToRgb888(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToByte(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToByte(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToByte(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<RgbFFF> RgbToRgbFFF(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = pixel.Red;
                resultRowGreen[x] = pixel.Green;
                resultRowBlue[x] = pixel.Blue;
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<RgbFFF> Rgb24ToRgbFFF(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb24>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToSingle(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToSingle(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToSingle(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<RgbFFF> Rgb48ToRgbFFF(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPackedPixelBuffer<Rgb48>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var row = inputBuffer.GetRow(y);
            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                var pixel = row[x];
                resultRowRed[x] = Pixels.Convert.ToSingle(pixel.Red);
                resultRowGreen[x] = Pixels.Convert.ToSingle(pixel.Green);
                resultRowBlue[x] = Pixels.Convert.ToSingle(pixel.Blue);
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb888> Rgb161616ToRgb888(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb888();
                resultRowGreen[x] = orgRowGreen[x].ToRgb888();
                resultRowBlue[x] = orgRowBlue[x].ToRgb888();
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<RgbFFF> Rgb161616ToRgbFFF(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgbFFF();
                resultRowGreen[x] = orgRowGreen[x].ToRgbFFF();
                resultRowBlue[x] = orgRowBlue[x].ToRgbFFF();
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb161616> Rgb888ToRgb161616(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb161616();
                resultRowGreen[x] = orgRowGreen[x].ToRgb161616();
                resultRowBlue[x] = orgRowBlue[x].ToRgb161616();
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<RgbFFF> Rgb888ToRgbFFF(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<RgbFFF>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgbFFF();
                resultRowGreen[x] = orgRowGreen[x].ToRgbFFF();
                resultRowBlue[x] = orgRowBlue[x].ToRgbFFF();
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb161616> RgbFFFToRgb161616(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb161616>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb161616();
                resultRowGreen[x] = orgRowGreen[x].ToRgb161616();
                resultRowBlue[x] = orgRowBlue[x].ToRgb161616();
            }
        });
        return resultBuffer;
    }

    private PlanarPixelBuffer<Rgb888> RgbFFFToRgb888(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PlanarPixelBuffer<Rgb888>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRowRed = resultBuffer.GetRow(0, y);
            var resultRowGreen = resultBuffer.GetRow(1, y);
            var resultRowBlue = resultBuffer.GetRow(2, y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRowRed[x] = orgRowRed[x].ToRgb888();
                resultRowGreen[x] = orgRowGreen[x].ToRgb888();
                resultRowBlue[x] = orgRowBlue[x].ToRgb888();
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb> Rgb161616ToRgb(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(Pixels.Convert.ToSingle(orgRowRed[x]),
                                        Pixels.Convert.ToSingle(orgRowGreen[x]),
                                        Pixels.Convert.ToSingle(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb24> Rgb161616ToRgb24(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(Pixels.Convert.ToByte(orgRowRed[x]),
                                        Pixels.Convert.ToByte(orgRowGreen[x]),
                                        Pixels.Convert.ToByte(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb48> Rgb161616ToRgb48(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb161616>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(orgRowRed[x],
                                        orgRowGreen[x],
                                        orgRowBlue[x]);
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb> Rgb888ToRgb(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(Pixels.Convert.ToSingle(orgRowRed[x]),
                                        Pixels.Convert.ToSingle(orgRowGreen[x]),
                                        Pixels.Convert.ToSingle(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb24> Rgb888ToRgb24(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(orgRowRed[x],
                                        orgRowGreen[x],
                                        orgRowBlue[x]);
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb48> Rgb888ToRgb48(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<Rgb888>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(Pixels.Convert.ToUInt16(orgRowRed[x]),
                                        Pixels.Convert.ToUInt16(orgRowGreen[x]),
                                        Pixels.Convert.ToUInt16(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb> RgbFFFToRgb(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb(orgRowRed[x],
                                        orgRowGreen[x],
                                        orgRowBlue[x]);
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb24> RgbFFFToRgb24(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb24>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb24(Pixels.Convert.ToByte(orgRowRed[x]),
                                        Pixels.Convert.ToByte(orgRowGreen[x]),
                                        Pixels.Convert.ToByte(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }

    private PackedPixelBuffer<Rgb48> RgbFFFToRgb48(ConvertParameters parameters)
    {
        var inputBuffer = (ReadOnlyPlanarPixelBuffer<RgbFFF>)parameters.Input;
        var resultBuffer = new PackedPixelBuffer<Rgb48>(inputBuffer.Width, inputBuffer.Height);
        Parallel.For(0, inputBuffer.Height, parameters.ParallelOptions, y =>
        {
            var orgRowRed = inputBuffer.GetRow(0, y);
            var orgRowGreen = inputBuffer.GetRow(1, y);
            var orgRowBlue = inputBuffer.GetRow(2, y);

            var resultRow = resultBuffer.GetRow(y);
            for (var x = 0; x < inputBuffer.Width; x++)
            {
                resultRow[x] = new Rgb48(Pixels.Convert.ToUInt16(orgRowRed[x]),
                                        Pixels.Convert.ToUInt16(orgRowGreen[x]),
                                        Pixels.Convert.ToUInt16(orgRowBlue[x]));
            }
        });
        return resultBuffer;
    }
}
