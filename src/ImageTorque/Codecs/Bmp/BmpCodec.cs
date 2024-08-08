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

namespace ImageTorque.Codecs.Bmp;

/// <summary>
/// Represents a codec for the BMP image format.
/// </summary>
public sealed class BmpCodec : ICodec
{
    /// <inheritdoc/>
    public int HeaderSize { get; } = BmpFileHeader.Size;

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new BmpDecoder();

    /// <inheritdoc/>
    public IImageEncoder Encoder { get; } = new BmpEncoder();

    /// <inheritdoc/>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header)
    {
        if (header.Length < HeaderSize)
        {
            return false;
        }

        short headerField = BinaryPrimitives.ReadInt16BigEndian(header[..2]);
        return headerField is BmpConstants.HeaderFields.Bitmap
            or BmpConstants.HeaderFields.BitmapArray
            or BmpConstants.HeaderFields.ColorIcon
            or BmpConstants.HeaderFields.ColorPointer
            or BmpConstants.HeaderFields.Icon
            or BmpConstants.HeaderFields.Pointer;
    }

    /// <inheritdoc/>
    public bool IsSupportedEncoderFormat(string encoderType) => encoderType.Equals("bmp", StringComparison.InvariantCultureIgnoreCase);
}
