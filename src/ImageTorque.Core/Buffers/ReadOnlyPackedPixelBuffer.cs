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

namespace ImageTorque.Buffers;

/// <summary>
/// Represents a read only packed pixel buffer.
/// </summary>
public sealed record ReadOnlyPackedPixelBuffer<TPixel> : ReadOnlyPixelBuffer<TPixel>
    where TPixel : unmanaged, IPixel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyPackedPixelBuffer{TPixel}"/> class.
    /// </summary>
    /// <param name="pixelBuffer">The pixel buffer.</param>
    public ReadOnlyPackedPixelBuffer(IPixelBuffer<TPixel> pixelBuffer)
        : base(pixelBuffer)
    {
    }

    /// <inheritdoc/>
    public override ReadOnlySpan<TPixel> GetChannel(int channelIndex) => PixelBuffer.GetChannel(channelIndex);

    /// <summary>
    /// Gets the row of pixels at the specified <paramref name="rowIndex"/>.
    /// </summary>
    /// <param name="rowIndex">The row index.</param>
    /// <returns>The row of pixels.</returns>
    public ReadOnlySpan<TPixel> GetRow(int rowIndex) => PixelBuffer.GetRow(rowIndex);
}
