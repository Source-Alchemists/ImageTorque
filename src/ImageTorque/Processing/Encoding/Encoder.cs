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

using ImageTorque.Buffers;
using ImageTorque.Pixels;

namespace ImageTorque.Processing;

internal class Encoder : IProcessor<EncoderParameters, bool>
{
    public bool Execute(EncoderParameters parameters)
    {
        Codecs.ICodec? codec = (parameters.Configuration?.Codecs.FirstOrDefault(codec => codec.IsSupportedEncoderFormat(parameters.EncoderType))) ?? throw new NotSupportedException($"The codec for the encoder type {parameters.EncoderType} is not supported.");

        switch(parameters.Input)
        {
            case ReadOnlyPackedPixelBuffer<Rgb24> packedPixelBuffer:
                codec.Encoder.Encode(parameters.Stream!, packedPixelBuffer, parameters.EncoderType, parameters.Quality);
                break;
            case ReadOnlyPackedPixelBuffer<Rgb48> packedPixelBuffer:
            codec.Encoder.Encode(parameters.Stream!, packedPixelBuffer, parameters.EncoderType, parameters.Quality);
                break;
            case ReadOnlyPackedPixelBuffer<L8> packedPixelBuffer:
                codec.Encoder.Encode(parameters.Stream!, packedPixelBuffer, parameters.EncoderType, parameters.Quality);
                break;
            case ReadOnlyPackedPixelBuffer<L16> packedPixelBuffer:
                codec.Encoder.Encode(parameters.Stream!, packedPixelBuffer, parameters.EncoderType, parameters.Quality);
                break;
            default:
                throw new NotSupportedException("Only packed pixel buffers are supported.");
        }

        return true;
    }
}
