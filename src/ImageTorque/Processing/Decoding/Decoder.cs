using System.Runtime.InteropServices;
using ImageTorque.Buffers;
using ImageTorque.Pixels;
using SixLabors.ImageSharp;

namespace ImageTorque.Processing;

internal class Decoder : IProcessor<DecoderParameters, IPixelBuffer>
{
    private static Configuration s_configuration = Configuration.Default;

    public IPixelBuffer Execute(DecoderParameters parameters)
    {
        s_configuration = Configuration.Default;
        s_configuration.PreferContiguousImageBuffers = true;
        return DecodeStream(parameters);
    }

    private static IPixelBuffer DecodeStream(DecoderParameters parameters)
    {
        Stream? stream = parameters.Input;

        long position = stream!.Position;
        IImageInfo info = SixLabors.ImageSharp.Image.Identify(stream);
        if (info == null)
            throw new InvalidOperationException("Could not identify image.");

        stream.Seek(position, SeekOrigin.Begin);

        IPixelBuffer pixelBuffer = null!;
        switch (info.PixelType.BitsPerPixel)
        {
            case 8:
                using (var imageL8 = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.L8>(s_configuration, stream))
                {
                    if (imageL8.DangerousTryGetSinglePixelMemory(out Memory<SixLabors.ImageSharp.PixelFormats.L8> pixelsL8))
                    {
                        Span<L8> buffer = MemoryMarshal.Cast<SixLabors.ImageSharp.PixelFormats.L8, L8>(pixelsL8.Span);
                        pixelBuffer = new PackedPixelBuffer<L8>(imageL8.Width, imageL8.Height, buffer);
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not get pixel buffer.");
                    }
                }
                break;
            case 24:
            case 32:
                using (var imageRgb24 = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgb24>(s_configuration, stream))
                {
                    if (imageRgb24.DangerousTryGetSinglePixelMemory(out Memory<SixLabors.ImageSharp.PixelFormats.Rgb24> pixelsRgb24))
                    {
                        Span<Rgb24> buffer = MemoryMarshal.Cast<SixLabors.ImageSharp.PixelFormats.Rgb24, Rgb24>(pixelsRgb24.Span);
                        pixelBuffer = new PackedPixelBuffer<Rgb24>(imageRgb24.Width, imageRgb24.Height, buffer);
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not get pixel buffer.");
                    }
                }
                break;

            default:
                throw new NotSupportedException($"Pixel type {info.PixelType} is not supported.");
        }

        return pixelBuffer;
    }
}
