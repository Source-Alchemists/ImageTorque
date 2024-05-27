using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;

namespace ImageTorque.Compression.Zlib;

internal sealed class ZlibInflateStream(Stream innerStream, Func<int> getData) : Stream
{
    private static readonly byte[] s_checksumBuffer = new byte[4];

    private static readonly Func<int> s_getDataNoOp = () => 0;

    private readonly Stream _innerStream = innerStream;

    private bool _isDisposed;

    private int _currentDataRemaining;

    private readonly Func<int> _getData = getData;

    public ZlibInflateStream(Stream innerStream) : this(innerStream, s_getDataNoOp)
    {
    }

    public override bool CanRead => _innerStream.CanRead;

    public override bool CanSeek => false;

    public override bool CanWrite => throw new NotSupportedException();

    public override long Length => throw new NotSupportedException();

    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

    public DeflateStream? DeflateStream { get; private set; }

    [MemberNotNullWhen(true, nameof(DeflateStream))]
    public bool AllocateNewBytes(int bytes, bool isCriticalChunk)
    {
        _currentDataRemaining = bytes;
        if (DeflateStream is null)
        {
            return InitializeInflateStream(isCriticalChunk);
        }

        return true;
    }

    public override void Flush() => throw new NotSupportedException();

    public override int ReadByte()
    {
        _currentDataRemaining--;
        return _innerStream.ReadByte();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_currentDataRemaining is 0)
        {
            _currentDataRemaining = _getData();

            if (_currentDataRemaining is 0)
            {
                return 0;
            }
        }

        int bytesToRead = Math.Min(count, _currentDataRemaining);
        _currentDataRemaining -= bytesToRead;
        int totalBytesRead = _innerStream.Read(buffer, offset, bytesToRead);
        long innerStreamLength = _innerStream.Length;

        int bytesRead = 0;
        offset += totalBytesRead;
        while (_currentDataRemaining is 0 && totalBytesRead < count)
        {
            _currentDataRemaining = _getData();

            if (_currentDataRemaining is 0)
            {
                return totalBytesRead;
            }

            offset += bytesRead;

            if (offset >= innerStreamLength || offset >= count)
            {
                return totalBytesRead;
            }

            bytesToRead = Math.Min(count - totalBytesRead, _currentDataRemaining);
            _currentDataRemaining -= bytesToRead;
            bytesRead = _innerStream.Read(buffer, offset, bytesToRead);
            if (bytesRead == 0)
            {
                return totalBytesRead;
            }

            totalBytesRead += bytesRead;
        }

        return totalBytesRead;
    }

    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_isDisposed)
        {
            if (DeflateStream != null)
            {
                DeflateStream.Dispose();
                DeflateStream = null;
            }
            base.Dispose(disposing);
            _isDisposed = true;
        }
    }

    [MemberNotNullWhen(true, nameof(DeflateStream))]
    private bool InitializeInflateStream(bool isCriticalChunk)
    {
        // Read the zlib header : http://tools.ietf.org/html/rfc1950
        int cmf = _innerStream.ReadByte();
        int flag = _innerStream.ReadByte();
        _currentDataRemaining -= 2;
        if (cmf == -1 || flag == -1)
        {
            return false;
        }

        if ((cmf & 0x0F) == 8)
        {
            int cinfo = (cmf & 0xF0) >> 4;

            if (cinfo > 7)
            {
                if (isCriticalChunk)
                {
                    throw new InvalidDataException($"Invalid window size for ZLIB header: cinfo={cinfo}");
                }

                return false;
            }
        }
        else if (isCriticalChunk)
        {
            throw new InvalidDataException($"Bad method for ZLIB header: cmf={cmf}");
        }
        else
        {
            return false;
        }

        bool fdict = (flag & 32) != 0;
        if (fdict)
        {
            // https://tools.ietf.org/html/rfc1950#page-6
            if (_innerStream.Read(s_checksumBuffer, 0, 4) != 4)
            {
                return false;
            }

            _currentDataRemaining -= 4;
        }

        DeflateStream = new DeflateStream(this, CompressionMode.Decompress, true);

        return true;
    }
}
