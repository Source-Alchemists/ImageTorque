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
public sealed class Configuration : IConfiguration
{
    /// <inheritdoc/>
    public bool PreferContiguousImageBuffers { get; init; } = true;

    /// <inheritdoc/>
    public int MaxHeaderSize { get; init; }

    /// <inheritdoc/>
    public IReadOnlyCollection<ICodec> Codecs { get; init; } = [];

    /// <inheritdoc/>
    public bool UseCrcValidation { get; init; } = false; // We don't use CRC validation by default, because we're running in a machine vision context
}
