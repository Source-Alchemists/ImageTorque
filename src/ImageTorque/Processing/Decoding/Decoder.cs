using ImageTorque.Buffers;
using ImageTorque.Codecs;

namespace ImageTorque.Processing;

internal class Decoder : IProcessor<DecoderParameters, IPixelBuffer>
{
    public IPixelBuffer Execute(DecoderParameters parameters)
    {
        Stream? stream = parameters.Input;

        if (!stream!.CanRead)
        {
            throw new NotSupportedException("Cannot read from the stream.");
        }

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        using BufferedStream bs = new(stream);
        ICodec format = DetectCodec(bs, parameters.Configuration!);
        return format.Decoder.Decode(bs, parameters.Configuration!);
    }

    private static ICodec DetectCodec(Stream stream, Configuration configuration)
    {
        int headerSize = (int)Math.Min(configuration.MaxHeaderSize, stream.Length);
        if (headerSize <= 0)
        {
            throw new InvalidDataException("Header size could not be estimated!");
        }

        Span<byte> headersBuffer = headerSize > 512 ? new byte[headerSize] : stackalloc byte[headerSize];
        long startPosition = stream.Position;

        int n = 0;
        int i;
        do
        {
            i = stream.Read(headersBuffer[n..headerSize]);
            n += i;
        }
        while (n < headerSize && i > 0);

        stream.Position = startPosition;

        ReadOnlySpan<byte> targetHeadersBuffer = headersBuffer[..n];

        ICodec? codec = null;
        foreach (ICodec confCodec in configuration.Codecs)
        {
            if (confCodec.HeaderSize <= targetHeadersBuffer.Length && confCodec.IsSupportedDecoderFormat(targetHeadersBuffer))
            {
                codec = confCodec;
                break;
            }
        }

        if (codec is null)
        {
            throw new InvalidDataException("Format could not be detected!");
        }

        return codec;
    }
}
