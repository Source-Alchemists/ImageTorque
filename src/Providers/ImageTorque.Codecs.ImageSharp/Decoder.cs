/*
 * Copyright 2024 Source Alchemists
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Runtime.InteropServices;

using ImageTorque.Buffers;
using ImageTorque.Pixels;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace ImageTorque.Codecs.ImageSharp;

/// <summary>
/// Represents a image decoder that implements the <see cref="IImageDecoder"/> interface.
/// </summary>
internal sealed class Decoder : IImageDecoder
{
    /// <summary>
    /// Decodes the image from the specified <see cref="Stream"/> using the default configuration.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing the image data.</param>
    /// <returns>The decoded <see cref="IPixelBuffer"/>.</returns>
    public IPixelBuffer Decode(Stream stream) => Decode(stream, IConfiguration.Default);

    /// <summary>
    /// Decodes the image from the specified <see cref="Stream"/> using the specified configuration.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> containing the image data.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> to use for decoding.</param>
    /// <returns>The decoded <see cref="IPixelBuffer"/>.</returns>
    public IPixelBuffer Decode(Stream stream, IConfiguration configuration)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        ImageInfo info = Image.Identify(stream) ?? throw new InvalidOperationException("Could not identify image.");

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        SixLabors.ImageSharp.Configuration sixConfig = SixLabors.ImageSharp.Configuration.Default.Clone();
        sixConfig.PreferContiguousImageBuffers = configuration.PreferContiguousImageBuffers;

        IPixelBuffer pixelBuffer = null!;
        switch (info.PixelType.BitsPerPixel)
        {
            case 8:
                using (var imageL8 = Image.Load<SixLabors.ImageSharp.PixelFormats.L8>(new DecoderOptions { Configuration = sixConfig }, stream))
                {
                    if (imageL8.DangerousTryGetSinglePixelMemory(out Memory<SixLabors.ImageSharp.PixelFormats.L8> pixelsL8))
                    {
                        Span<L8> buffer = MemoryMarshal.Cast<SixLabors.ImageSharp.PixelFormats.L8, L8>(pixelsL8.Span);
                        pixelBuffer = new PixelBuffer<L8>(imageL8.Width, imageL8.Height, buffer);
                    }
                    else
                    {
                        throw new InvalidOperationException("Could not get pixel buffer.");
                    }
                }
                break;
            case 24:
            case 32:
                using (var imageRgb24 = Image.Load<SixLabors.ImageSharp.PixelFormats.Rgb24>(new DecoderOptions { Configuration = sixConfig }, stream))
                {
                    if (imageRgb24.DangerousTryGetSinglePixelMemory(out Memory<SixLabors.ImageSharp.PixelFormats.Rgb24> pixelsRgb24))
                    {
                        Span<Rgb24> buffer = MemoryMarshal.Cast<SixLabors.ImageSharp.PixelFormats.Rgb24, Rgb24>(pixelsRgb24.Span);
                        pixelBuffer = new PixelBuffer<Rgb24>(imageRgb24.Width, imageRgb24.Height, buffer);
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
