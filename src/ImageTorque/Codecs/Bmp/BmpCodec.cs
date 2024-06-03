
using System.Buffers.Binary;

namespace ImageTorque.Codecs.Bmp;

public sealed class BmpCodec : ICodec
{
    public int HeaderSize { get; } = BmpFileHeader.Size;

    public IImageDecoder Decoder { get; } = new BmpDecoder();

    public bool IsSupportedFileFormat(ReadOnlySpan<byte> header)
    {
        if (header.Length < HeaderSize)
        {
            return false;
        }

        short headerField = BinaryPrimitives.ReadInt16BigEndian(header[..2]);
        return headerField is BmpConstants.HeaderFields.Bitmap
            or BmpConstants.HeaderFields.BitmapArray
            or BmpConstants.HeaderFields.ColorIcon
            or BmpConstants.HeaderFields.ColorPointer
            or BmpConstants.HeaderFields.Icon
            or BmpConstants.HeaderFields.Pointer;
    }
}
