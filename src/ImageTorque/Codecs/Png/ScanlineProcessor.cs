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

using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Provides methods for processing scanlines in a PNG image.
/// </summary>
internal static class ScanlineProcessor
{
    /// <summary>
    /// Processes a grayscale scanline and assigns the corresponding pixel values to the specified row.
    /// </summary>
    /// <typeparam name="TPixel">The type of the pixel.</typeparam>
    /// <param name="header">The PNG header.</param>
    /// <param name="scanlineSpan">The span containing the grayscale scanline data.</param>
    /// <param name="rowSpan">The span representing the row where the pixel values will be assigned.</param>
    /// <remarks>
    /// This method assumes that the grayscale scanline data is in the range of 0 to 255.
    /// If the bit depth of the PNG header is 16, the scanline data is interpreted as 16-bit luminance values.
    /// Otherwise, the scanline data is interpreted as 8-bit luminance values.
    /// The pixel values are assigned to the row span using the specified pixel type.
    /// </remarks>
    public static void ProcessGrayscale<TPixel>(PngHeader header, ReadOnlySpan<byte> scanlineSpan, Span<TPixel> rowSpan) where TPixel : unmanaged, IPixel
    {
        ref byte scanlineSpanRef = ref MemoryMarshal.GetReference(scanlineSpan);
        ref TPixel rowSpanRef = ref MemoryMarshal.GetReference(rowSpan);
        int scaleFactor = 255 / (NumericMath.GetColorCountForBitDepth(header.BitDepth) - 1);

        if (header.BitDepth == 16)
        {
            int o = 0;
            for (nuint x = 0; x < (uint)header.Width; x++, o += 2)
            {
                ushort luminance = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o, 2));
                Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)Unsafe.As<ushort, L16>(ref luminance);
            }
        }
        else
        {
            for (nuint x = 0, o = 0; x < (uint)header.Width; x++, o++)
            {
                byte luminance = (byte)(Unsafe.Add(ref scanlineSpanRef, o) * scaleFactor);
                Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)Unsafe.As<byte, L8>(ref luminance);
            }
        }
    }

    /// <summary>
    /// Processes a scanline of a PNG image with a color palette and maps it to a row of pixels.
    /// </summary>
    /// <typeparam name="TPixel">The type of the pixel.</typeparam>
    /// <param name="header">The PNG header.</param>
    /// <param name="scanlineSpan">The span containing the scanline data.</param>
    /// <param name="rowSpan">The span representing the row of pixels.</param>
    /// <param name="palette">The color palette.</param>
    /// <exception cref="InvalidDataException">Thrown when the color palette is missing.</exception>
    public static void ProcessPalette<TPixel>(PngHeader header, ReadOnlySpan<byte> scanlineSpan, Span<TPixel> rowSpan, ReadOnlyMemory<Rgb24>? palette) where TPixel : unmanaged, IPixel
    {
        if (palette is null)
        {
            throw new InvalidDataException("Missing color palette!");
        }

        ref byte scanlineSpanRef = ref MemoryMarshal.GetReference(scanlineSpan);
        ref TPixel rowSpanRef = ref MemoryMarshal.GetReference(rowSpan);
        ref Rgb24 paletteBase = ref MemoryMarshal.GetReference(palette.Value.Span);
        int maxIndex = palette.Value.Length - 1;

        for (nuint x = 0, o = 0; x < (uint)header.Width; x++, o++)
        {
            uint index = Unsafe.Add(ref scanlineSpanRef, o);
            Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)Unsafe.Add(ref paletteBase, (int)Math.Min(index, maxIndex));
        }
    }

    /// <summary>
    /// Processes the RGB scanline data and populates the row span with the corresponding pixel values.
    /// </summary>
    /// <typeparam name="TPixel">The type of the pixel.</typeparam>
    /// <param name="header">The PNG header.</param>
    /// <param name="scanlineSpan">The span containing the scanline data.</param>
    /// <param name="rowSpan">The span to populate with the pixel values.</param>
    /// <param name="bytesPerPixel">The number of bytes per pixel.</param>
    /// <param name="bytesPerSample">The number of bytes per sample.</param>
    /// <remarks>
    /// This method processes the RGB scanline data and converts it into pixel values of type <typeparamref name="TPixel"/>.
    /// If the bit depth is 16, the method reads the RGB values from the scanline span and assigns them to the corresponding pixels in the row span.
    /// If the bit depth is not 16, the method interprets the scanline span as a span of <see cref="Rgb24"/> values and assigns them to the corresponding pixels in the row span.
    /// </remarks>
    public static void ProcessRgb<TPixel>(PngHeader header, ReadOnlySpan<byte> scanlineSpan, Span<TPixel> rowSpan, int bytesPerPixel, int bytesPerSample) where TPixel : unmanaged, IPixel
    {
        ref TPixel rowSpanRef = ref MemoryMarshal.GetReference(rowSpan);

        if (header.BitDepth == 16)
        {
            int o = 0;
            for (nuint x = 0; x < (uint)header.Width; x++, o += bytesPerPixel)
            {
                ushort r = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o, bytesPerSample));
                ushort g = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o + bytesPerSample, bytesPerSample));
                ushort b = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o + (2 * bytesPerSample), bytesPerSample));
                Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)new Rgb48(r, g, b);
            }
        }
        else
        {
            ReadOnlySpan<Rgb24> source = MemoryMarshal.Cast<byte, Rgb24>(scanlineSpan[..(header.Width * bytesPerPixel)]).Slice(0, header.Width);
            ref Rgb24 sourceBase = ref MemoryMarshal.GetReference(source);
            ref TPixel destinationBase = ref MemoryMarshal.GetReference(rowSpan.Slice(0, header.Width));

            for (nuint i = 0; i < (uint)source.Length; i++)
            {
                Unsafe.Add(ref destinationBase, i) = (TPixel)(IPixel)Unsafe.Add(ref sourceBase, i);
            }
        }
    }
}
