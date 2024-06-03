namespace ImageTorque.Codecs.Jpeg;

/// <summary>
/// Originally ported from <see href="https://github.com/t0rakka/mango"/>.
/// </summary>
internal sealed class JpegHuffmanDecoder
{
    private readonly Stream _stream;
    private readonly JpegDataConverter _dataConverter;

    public JpegHuffmanDecoder(Stream stream, JpegDataConverter dataConverter)
    {
        _stream = stream;
        _dataConverter = dataConverter;
    }
}
