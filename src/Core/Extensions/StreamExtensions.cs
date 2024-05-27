using System.Buffers;

namespace ImageTorque;

internal static class StreamExtensions
{
    public static void Skip(this Stream stream, int count)
    {
        if(stream.CanSeek)
        {
            stream.Seek(count, SeekOrigin.Current);
            return;
        }

        byte[] buffer = ArrayPool<byte>.Shared.Rent(count);
        try
        {
            while (count > 0)
            {
                int bytesRead = stream.Read(buffer, 0, count);
                if (bytesRead == 0)
                {
                    break;
                }

                count -= bytesRead;
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
