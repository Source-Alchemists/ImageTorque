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
/// <see href="https://www.w3.org/TR/2003/REC-PNG-20031110/#11Critical-chunks"/>
/// </summary>
internal enum PngChunkType : uint
{
    ImageHeader = 0x49484452U, // IHDR
    Palette = 0x504C5445U, // PLTE
    ImageData = 0x49444154U, // IDAT
    ImageTrailer = 0x49454E44U // IEND
}
