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

using ImageTorque.Codecs;

namespace ImageTorque;

/// <summary>
/// Represents the configuration settings for the ImageTorque library.
/// </summary>
public interface IConfiguration
{
    /// <summary>
    /// Gets or sets the default configuration instance.
    /// </summary>
    static IConfiguration Default { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether to prefer contiguous image buffers.
    /// </summary>
    bool PreferContiguousImageBuffers { get; init; }

    /// <summary>
    /// Gets or sets the maximum size of the image header.
    /// </summary>
    int MaxHeaderSize { get; init; }

    /// <summary>
    /// Gets or sets the collection of codecs used for image encoding and decoding.
    /// </summary>
    IReadOnlyCollection<ICodec> Codecs { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to use CRC validation for image data.
    /// </summary>
    bool UseCrcValidation { get; init; }
}
