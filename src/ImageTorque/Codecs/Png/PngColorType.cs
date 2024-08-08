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

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents the color type of a PNG image.
/// </summary>
internal enum PngColorType : byte
{
    /// <summary>
    /// Grayscale color type.
    /// </summary>
    Grayscale = 0,

    /// <summary>
    /// RGB color type.
    /// </summary>
    Rgb = 2,

    /// <summary>
    /// Palette color type.
    /// </summary>
    Palette = 3,

    /// <summary>
    /// Grayscale with alpha color type.
    /// </summary>
    GrayscaleWithAlpha = 4,

    /// <summary>
    /// RGB with alpha color type.
    /// </summary>
    RgbWithAlpha = 6
}
