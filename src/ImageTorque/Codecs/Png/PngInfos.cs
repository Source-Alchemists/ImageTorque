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

using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents the information of a PNG image.
/// </summary>
internal sealed record PngInfos
{
    /// <summary>
    /// Gets or sets the bit depth of the image.
    /// </summary>
    public byte? BitDepth { get; init; }

    /// <summary>
    /// Gets or sets the color type of the image.
    /// </summary>
    public PngColorType? ColorType { get; init; }

    /// <summary>
    /// Gets or sets the resolution units of the image.
    /// </summary>
    public PixelResolutionUnit ResolutionUnits { get; init; }

    /// <summary>
    /// Gets or sets the horizontal resolution of the image.
    /// </summary>
    public double HorizontalResolution { get; init; }

    /// <summary>
    /// Gets or sets the vertical resolution of the image.
    /// </summary>
    public double VerticalResolution { get; init; }

    /// <summary>
    /// Gets or sets the color palette of the image.
    /// </summary>
    public byte[] ColorPalette { get; set; } = [];

    /// <summary>
    /// Gets or sets the color table of the image.
    /// </summary>
    public Rgb24[] ColorTable { get; set; } = [];

    /// <summary>
    /// Gets or sets the number of bytes per pixel of the image.
    /// </summary>
    public int BytesPerPixel { get; set; }

    /// <summary>
    /// Gets or sets the number of bytes per scanline of the image.
    /// </summary>
    public int BytesPerScanline { get; set; }

    /// <summary>
    /// Gets or sets the number of bytes per sample of the image.
    /// </summary>
    public int BytesPerSample { get; set; }
}
