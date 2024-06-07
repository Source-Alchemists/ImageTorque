using System.Buffers;
using System.Buffers.Binary;
using System.IO.Hashing;
using System.Runtime.CompilerServices;

using ImageTorque.Memory;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Provides utility methods for working with PNG chunks.
/// </summary>
internal static class PngChunkUtils
{
    /// <summary>
    /// Validates the integrity of a PNG chunk by calculating and comparing the CRC32 checksum.
    /// </summary>
    /// <param name="stream">The input stream containing the PNG data.</param>
    /// <param name="chunk">The PNG chunk to validate.</param>
    /// <param name="buffer">A span of bytes used for buffering.</param>
    /// <exception cref="InvalidDataException">Thrown when the chunk's CRC32 checksum does not match the calculated checksum.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ValidateChunk(in Stream stream, in PngChunk chunk, Span<byte> buffer)
    {
        uint inputCrc = ReadChunkCrc(stream, buffer);
        Span<byte> chunkType = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(chunkType, (uint)chunk.Type);

        Crc32 crc32 = new();
        crc32.Append(chunkType);
        crc32.Append(chunk.Data.Memory.Span.Slice(0, chunk.Length));

        if (crc32.GetCurrentHashAsUInt32() != inputCrc)
        {
            chunk.Data?.Dispose();
            throw new InvalidDataException("Invalid chunk crc!");
        }
    }

    /// <summary>
    /// Reads the data of a PNG chunk from the specified stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="length">The length of the chunk data.</param>
    /// <returns>An <see cref="IMemoryOwner{T}"/> containing the chunk data.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IMemoryOwner<byte> ReadChunkData(in Stream stream, int length)
    {
        if (length == 0)
        {
            return OptimizedMemoryPool<byte>.Shared.Rent(0);
        }

        length = (int)Math.Min(length, stream.Length - stream.Position);
        IMemoryOwner<byte> buffer = OptimizedMemoryPool<byte>.Shared.Rent(length);
        stream.Read(buffer.Memory.Span.Slice(0, length));
        return buffer;
    }

    /// <summary>
    /// Reads the type of a PNG chunk from the specified stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="buffer">A span of bytes used for buffering.</param>
    /// <returns>The type of the PNG chunk.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PngChunkType ReadChunkType(in Stream stream, Span<byte> buffer)
    {
        if (stream.Read(buffer.Slice(0, 4)) == 4)
        {
            return (PngChunkType)BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }

        throw new InvalidDataException("Invalid chunk type!");
    }

    /// <summary>
    /// Reads the header chunk of a PNG image.
    /// </summary>
    /// <param name="pngMeta">The PNG metadata.</param>
    /// <param name="data">The data containing the PNG header.</param>
    /// <returns>A tuple containing the PNG header and metadata.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (PngHeader, PngInfos) ReadHeaderChunk(in PngInfos pngMeta, ReadOnlySpan<byte> data)
    {
        var header = PngHeader.Parse(data);
        header.Validate();

        PngInfos meta = pngMeta with
        {
            BitDepth = header.BitDepth,
            ColorType = header.ColorType
        };

        return (header, meta);
    }

    /// <summary>
    /// Reads the length of a PNG chunk from the specified stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="buffer">A span of bytes used for buffering.</param>
    /// <param name="result">The length of the PNG chunk.</param>
    /// <returns><c>true</c> if the length was successfully read; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryReadChunkLength(in Stream stream, Span<byte> buffer, out int result)
    {
        if (stream.Read(buffer.Slice(0, 4)) == 4)
        {
            result = BinaryPrimitives.ReadInt32BigEndian(buffer);
            return true;
        }

        result = default;
        return false;
    }

    /// <summary>
    /// Reads the CRC32 checksum of a PNG chunk from the specified stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <param name="buffer">A span of bytes used for buffering.</param>
    /// <returns>The CRC32 checksum of the PNG chunk.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadChunkCrc(Stream stream, Span<byte> buffer)
    {
        uint crc = 0;
        if (stream.Read(buffer.Slice(0, 4)) == 4)
        {
            crc = BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }

        return crc;
    }
}
