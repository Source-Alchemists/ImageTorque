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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImageMagick;

using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Codecs.ImageMagick;

internal sealed class Decoder : IImageDecoder
{
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
        Span<L8> buffer = MemoryMarshal.Cast<byte, L8>(image.ToByteArray(MagickFormat.Gray));
        return new PixelBuffer<L8>(width, height, buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer DecodeL16(MagickImage image)
    {
        int width = Convert.ToInt32(image.Width);
        int height = Convert.ToInt32(image.Height);
        unsafe {
            Span<L16> buffer = MemoryMarshal.Cast<ushort, L16>(new Span<ushort>((void*)image.GetPixelsUnsafe().GetAreaPointer(0, 0, image.Width, image.Height), (int)(image.Width * image.Height)));
            return new PixelBuffer<L16>(width, height, buffer);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer DecodeRgb24(MagickImage image)
    {
        int width = Convert.ToInt32(image.Width);
        int height = Convert.ToInt32(image.Height);
        Span<Rgb24> buffer = MemoryMarshal.Cast<byte, Rgb24>(image.ToByteArray(MagickFormat.Rgb));
        return new PixelBuffer<Rgb24>(width, height, buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static IPixelBuffer DecodeRgb48(MagickImage image)
    {
        int width = Convert.ToInt32(image.Width);
        int height = Convert.ToInt32(image.Height);
        unsafe {
            Span<Rgb48> buffer = MemoryMarshal.Cast<ushort, Rgb48>(new Span<ushort>((void*)image.GetPixelsUnsafe().GetAreaPointer(0, 0, image.Width, image.Height), (int)(image.Width * image.Height * 3)));
            return new PixelBuffer<Rgb48>(width, height, buffer);
        }
    }
}
