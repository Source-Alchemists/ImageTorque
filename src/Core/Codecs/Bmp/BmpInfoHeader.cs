using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImageTorque.Codecs.Bmp;

/// <summary>
/// <see href="https://en.wikipedia.org/wiki/BMP_file_format"/>
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal record struct BmpInfoHeader
{
    internal enum HeaderVersionType
    {
        BITMAPCOREHEADER = 12,
        OS22XBITMAPHEADER = 64,
        OS22XBITMAPHEADER_ALT = 16,
        BITMAPINFOHEADER = 40,
        BITMAPV2INFOHEADER = 52,
        BITMAPV3INFOHEADER = 56,
        BITMAPV4HEADER = 108,
        BITMAPV5HEADER = 124
    }

    internal enum CompressionType
    {
        RGB = 0,
        RLE8 = 1,
        RLE4 = 2,
        BitFields = 3,
        JPEG = 4,
        PNK = 5,
        AlphaBitFields = 6,
        CMYK = 11,
        CMYKRLE8 = 12,
        CMYKRLE4 = 13
    }

    public const HeaderVersionType SupportedHeaderVersion = HeaderVersionType.BITMAPV5HEADER;
    public const int HeaderSize = 4;

    public int Size { get; }
    public int Width { get; init; }
    public int Height { get; init; }
    public short Planes { get; }
    public ushort BitsPerPixel { get; }
    public CompressionType Compression { get; init; }
    public int ImageSize { get; init; }
    public int ClrUsed { get; init; }
    public int ClrImportant { get; init; }
    public int RedMask { get; set; }
    public int GreenMask { get; set; }
    public int BlueMask { get; set; }
    public HeaderVersionType HeaderVersion { get; }

    public BmpInfoHeader(HeaderVersionType version, int size, int width, int height, short planes, ushort bitsPerPixel)
    {
        HeaderVersion = version;
        Size = size;
        Width = width;
        Height = height;
        Planes = planes;
        BitsPerPixel = bitsPerPixel;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsBITMAPCOREHEADER(ReadOnlySpan<byte> data) => new(
        version: HeaderVersionType.BITMAPCOREHEADER,
        size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
        width: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(4, 2)),
        height: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(6, 2)),
        planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(8, 2)),
        bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(10, 2))
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsOS22XBITMAPHEADER(ReadOnlySpan<byte> data)
    {
        BmpInfoHeader infoHeader = new(
            version: HeaderVersionType.OS22XBITMAPHEADER,
            size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
            width: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4, 4)),
            height: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8, 4)),
            planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(12, 2)),
            bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(14, 2)));

        int compressionData = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(16, 4));
        CompressionType compression = compressionData switch
        {
            0 => CompressionType.RGB,
            1 => CompressionType.RLE8,
            2 => CompressionType.RLE4,
            _ => throw new NotSupportedException($"Compression type '{compressionData}' is not supported!"),
        };

        infoHeader = infoHeader with
        {
            Compression = compression,
            ImageSize = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(20, 4)),
            ClrUsed = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(32, 4)),
            ClrImportant = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(36, 4))
        };

        return infoHeader;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsOS22XBITMAPHEADER_ALT(ReadOnlySpan<byte> data) => new(
            version: HeaderVersionType.OS22XBITMAPHEADER_ALT,
            size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
            width: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4, 4)),
            height: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8, 4)),
            planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(12, 2)),
            bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(14, 2))
    );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsBITMAPINFOHEADER(Stream stream, ReadOnlySpan<byte> data)
    {
        var infoHeader = new BmpInfoHeader(
            version: HeaderVersionType.BITMAPINFOHEADER,
            size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
            width: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4, 4)),
            height: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8, 4)),
            planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(12, 2)),
            bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(14, 2)))
        {
            Compression = (CompressionType)BinaryPrimitives.ReadInt32LittleEndian(data.Slice(16, 4)),
            ImageSize = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(20, 4)),
            ClrUsed = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(32, 4)),
            ClrImportant = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(36, 4))
        };

        if (infoHeader.Compression == CompressionType.BitFields || infoHeader.Compression == CompressionType.AlphaBitFields)
        {
            Span<byte> buffer = stackalloc byte[12];
            stream.Read(buffer);
            infoHeader.RedMask = BinaryPrimitives.ReadInt32LittleEndian(buffer[..4]);
            infoHeader.GreenMask = BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(4, 4));
            infoHeader.BlueMask = BinaryPrimitives.ReadInt32LittleEndian(buffer.Slice(8, 4));
        }

        return infoHeader;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsBITMAPV2INFOHEADER(ReadOnlySpan<byte> data) => new(
            version: HeaderVersionType.BITMAPV2INFOHEADER,
            size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
            width: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4, 4)),
            height: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8, 4)),
            planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(12, 2)),
            bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(14, 2)))
    {
        Compression = (CompressionType)BinaryPrimitives.ReadInt32LittleEndian(data.Slice(16, 4)),
        ImageSize = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(20, 4)),
        ClrUsed = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(32, 4)),
        ClrImportant = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(36, 4)),
        RedMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(40, 4)),
        GreenMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(44, 4)),
        BlueMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(48, 4)),
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsBITMAPV3INFOHEADER(ReadOnlySpan<byte> data) => new(
            version: HeaderVersionType.BITMAPV3INFOHEADER,
            size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
            width: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4, 4)),
            height: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8, 4)),
            planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(12, 2)),
            bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(14, 2)))
    {
        Compression = (CompressionType)BinaryPrimitives.ReadInt32LittleEndian(data.Slice(16, 4)),
        ImageSize = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(20, 4)),
        ClrUsed = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(32, 4)),
        ClrImportant = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(36, 4)),
        RedMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(40, 4)),
        GreenMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(44, 4)),
        BlueMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(48, 4)),
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsBITMAPV4HEADER(ReadOnlySpan<byte> data) => new(
        version: HeaderVersionType.BITMAPV4HEADER,
        size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
        width: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4, 4)),
        height: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8, 4)),
        planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(12, 2)),
        bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(14, 2)))
        {
        Compression = (CompressionType)BinaryPrimitives.ReadInt32LittleEndian(data.Slice(16, 4)),
        ImageSize = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(20, 4)),
        ClrUsed = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(32, 4)),
        ClrImportant = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(36, 4)),
        RedMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(40, 4)),
        GreenMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(44, 4)),
        BlueMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(48, 4))
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static BmpInfoHeader ParseAsBITMAPV5HEADER(ReadOnlySpan<byte> data) => new(
        version: HeaderVersionType.BITMAPV4HEADER,
        size: BinaryPrimitives.ReadInt32LittleEndian(data[..4]),
        width: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(4, 4)),
        height: BinaryPrimitives.ReadInt32LittleEndian(data.Slice(8, 4)),
        planes: BinaryPrimitives.ReadInt16LittleEndian(data.Slice(12, 2)),
        bitsPerPixel: BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(14, 2)))
        {
        Compression = (CompressionType)BinaryPrimitives.ReadInt32LittleEndian(data.Slice(16, 4)),
        ImageSize = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(20, 4)),
        ClrUsed = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(32, 4)),
        ClrImportant = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(36, 4)),
        RedMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(40, 4)),
        GreenMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(44, 4)),
        BlueMask = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(48, 4))
    };
}
