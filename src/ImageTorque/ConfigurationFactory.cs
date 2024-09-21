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
/// Factory class for creating configurations.
/// </summary>
public static class ConfigurationFactory
{
    /// <summary>
    /// Builds a configuration object based on the provided codecs.
    /// </summary>
    /// <param name="codecs">The collection of codecs to include in the configuration.</param>
    /// <returns>A new configuration object.</returns>
    public static IConfiguration Build(IReadOnlyCollection<ICodec> codecs)
    {
        return new Configuration()
        {
            MaxHeaderSize = codecs.Max(f => f.HeaderSize),
            Codecs = codecs
        };
    }
}
