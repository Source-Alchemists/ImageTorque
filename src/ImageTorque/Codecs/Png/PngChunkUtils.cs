using System.Buffers;
using System.Buffers.Binary;
using System.IO.Hashing;
using System.Runtime.CompilerServices;

using ImageTorque.Memory;

namespace ImageTorque.Codecs.Png;

internal static class PngChunkUtils
{
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PngChunkType ReadChunkType(in Stream stream, Span<byte> buffer)
    {
        if (stream.Read(buffer.Slice(0, 4)) == 4)
        {
            return (PngChunkType)BinaryPrimitives.ReadUInt32BigEndian(buffer);
        }

        throw new InvalidDataException("Invalid chunk type!");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (PngHeader, PngInfos) ReadHeaderChunk(in PngInfos pngMeta, ReadOnlySpan<byte> data)
    {
        var header = PngHeader.Parse(data);
        header.Validate();

        PngInfos meta = pngMeta with
        {
            BitDepth = header.BitDepth,
            ColorType = header.ColorType,
            InterlaceMethod = header.InterlaceMethod
        };

        return (header, meta);
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SkipChunkDataAndCrc(Stream stream, in PngChunk chunk) => stream.Skip(chunk.Length + 4);

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
