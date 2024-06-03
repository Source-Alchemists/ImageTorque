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
