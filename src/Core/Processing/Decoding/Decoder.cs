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

        Configuration configuration = Configuration.Default;

        if (stream.CanSeek)
        {
            stream.Position = 0;
        }

        IImageFormat format = DetectFormat(stream, configuration);
        return format.Decoder.Decode(stream, configuration);
    }

    private static IImageFormat DetectFormat(Stream stream, Configuration configuration)
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

        IImageFormat? format = null;
        foreach (IImageFormat formatDetector in configuration.Formats)
        {
            if (formatDetector.HeaderSize <= targetHeadersBuffer.Length && formatDetector.IsSupportedFileFormat(targetHeadersBuffer))
            {
                format = formatDetector;
                break;
            }
        }

        if (format is null)
        {
            throw new InvalidDataException("Format could not be detected!");
        }

        return format;
    }
}
