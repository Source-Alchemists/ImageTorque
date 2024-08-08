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

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents a PNG codec that implements the <see cref="ICodec"/> interface.
/// </summary>
public sealed class PngCodec : ICodec
{
    /// <inheritdoc/>
    public int HeaderSize { get; } = PngConstants.HeaderSize;

    /// <inheritdoc/>
    public IImageDecoder Decoder { get; } = new PngDecoder();

    /// <inheritdoc/>
    public IImageEncoder Encoder => throw new NotSupportedException();

    /// <inheritdoc/>
    public bool IsSupportedDecoderFormat(ReadOnlySpan<byte> header) => header.Length >= HeaderSize && BinaryPrimitives.ReadUInt64BigEndian(header) == PngConstants.HeaderValue;

    /// <inheritdoc/>
    public bool IsSupportedEncoderFormat(string encoderType) => false;
}
