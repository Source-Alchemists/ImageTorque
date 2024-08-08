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

using System.Text;

namespace ImageTorque.Codecs.Png;

internal static class PngConstants
{
    public static readonly Encoding Encoding = Encoding.GetEncoding("ISO-8859-1");

    public static readonly Encoding LanguageEncoding = Encoding.ASCII;

    public static readonly Encoding TranslatedEncoding = Encoding.UTF8;

    public static readonly IEnumerable<string> MimeTypes = ["image/png", "image/apng"];

    public static readonly IEnumerable<string> FileExtensions = ["png", "apng"];

    public const int HeaderSize = 8;

    public const ulong HeaderValue = 0x89504E470D0A1A0AUL;

    public static readonly Dictionary<PngColorType, byte[]> ColorTypes = new()
    {
        [PngColorType.Grayscale] = [1, 2, 4, 8, 16],
        [PngColorType.Rgb] = [8, 16],
        [PngColorType.Palette] = [1, 2, 4, 8],
        [PngColorType.GrayscaleWithAlpha] = [8, 16],
        [PngColorType.RgbWithAlpha] = [8, 16]
    };

    public const int MaxTextKeywordLength = 79;

    public const int MinTextKeywordLength = 1;
    public const int MaxUncompressedAncillaryChunkSizeBytes  = 8 * 1024 * 1024;
}
