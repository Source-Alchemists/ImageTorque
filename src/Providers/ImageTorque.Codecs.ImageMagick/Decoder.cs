using System.Runtime.CompilerServices;

using ImageMagick;

using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick;

internal sealed class Decoder : IImageDecoder
{
    /// <summary>
    /// Decodes the image from the specified <see cref="Stream"/> using the default configuration.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing the image data.</param>
    /// <returns>The decoded <see cref="IPixelBuffer"/>.</returns>
    public IPixelBuffer Decode(Stream stream) => Decode(stream, Configuration.Default);

    /// <summary>
    /// Decodes the image from the specified <see cref="Stream"/> using the specified configuration.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing the image data.</param>
    /// <param name="configuration">The <see cref="Configuration"/> to use for decoding.</param>
    /// <returns>The decoded <see cref="IPixelBuffer"/>.</returns>
    public IPixelBuffer Decode(Stream stream, Configuration configuration)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        using var image = new MagickImage(stream);
        if (image.ColorType == ColorType.Grayscale || image.ColorType == ColorType.Palette || image.ColorType == ColorType.GrayscaleAlpha)
        {
            if (image.Depth == 8)
            {
                return DecodeL8(image);
            }
            else if (image.Depth == 16)
            {
                return DecodeL16(image);
            }

            throw new NotSupportedException($"Unsupported bit depth '{image.Depth}'!");
        }

        if (image.ChannelCount == 3 || image.ChannelCount == 4)
        {
            if (image.Depth == 8)
            {
                return DecodeRgb24(image);
            }
            else if (image.Depth == 16)
            {
                return DecodeRgb48(image);
            }

            throw new NotSupportedException($"Unsupported bit depth '{image.Depth}'!");
        }
        else
        {
            throw new NotSupportedException($"Unsupported channel count '{image.ChannelCount}'!");
        }

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer DecodeL8(MagickImage image)
    {
        int width = Convert.ToInt32(image.Width);
        int height = Convert.ToInt32(image.Height);
        var targetBuffer = new PixelBuffer<L8>(width, height);
        IPixelCollection<ushort> sourcePixels = image.GetPixelsUnsafe();
        Parallel.For(0, height, y =>
        {
            Span<L8> row = targetBuffer.GetRow(y);
            for (int x = 0; x < width; x++)
            {
                ushort[] sourcePixel = sourcePixels.GetValue(x, y)!;
                row[x] = new L8(BitConverter.GetBytes(sourcePixel[0])[0]);
            }
        });

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer DecodeL16(MagickImage image)
    {
        int width = Convert.ToInt32(image.Width);
        int height = Convert.ToInt32(image.Height);
        var targetBuffer = new PixelBuffer<L16>(width, height);
        IPixelCollection<ushort> sourcePixels = image.GetPixelsUnsafe();
        Parallel.For(0, height, y =>
        {
            Span<L16> row = targetBuffer.GetRow(y);
            for (int x = 0; x < width; x++)
            {
                ushort[] sourcePixel = sourcePixels.GetValue(x, y)!;
                row[x] = new L16(sourcePixel[0]);
            }
        });

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer DecodeRgb24(MagickImage image)
    {
        int width = Convert.ToInt32(image.Width);
        int height = Convert.ToInt32(image.Height);
        var targetBuffer = new PixelBuffer<Rgb24>(width, height);
        IPixelCollection<ushort> sourcePixels = image.GetPixelsUnsafe();
        Parallel.For(0, height, y =>
        {
            Span<Rgb24> row = targetBuffer.GetRow(y);
            for (int x = 0; x < width; x++)
            {
                ushort[] sourcePixel = sourcePixels.GetValue(x, y)!;
                row[x] = new Rgb24(BitConverter.GetBytes(sourcePixel[0])[0], BitConverter.GetBytes(sourcePixel[1])[0], BitConverter.GetBytes(sourcePixel[2])[0]);
            }
        });

        return targetBuffer;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer DecodeRgb48(MagickImage image)
    {
        int width = Convert.ToInt32(image.Width);
        int height = Convert.ToInt32(image.Height);
        var targetBuffer = new PixelBuffer<Rgb48>(width, height);
        IPixelCollection<ushort> sourcePixels = image.GetPixelsUnsafe();
        Parallel.For(0, height, y =>
        {
            Span<Rgb48> row = targetBuffer.GetRow(y);
            for (int x = 0; x < width; x++)
            {
                ushort[] sourcePixel = sourcePixels.GetValue(x, y)!;
                row[x] = new Rgb48(sourcePixel[0], sourcePixel[1], sourcePixel[2]);
            }
        });

        return targetBuffer;
    }
}
