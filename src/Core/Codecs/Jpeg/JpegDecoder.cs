using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using ImageTorque.Buffers;

namespace ImageTorque.Codecs.Jpeg;

public sealed class JpegDecoder : IImageDecoder
{
    public IPixelBuffer Decode(Stream stream) => throw new NotImplementedException();
    public IPixelBuffer Decode(Stream stream, Configuration configuration) => throw new NotImplementedException();

    private static void ParseStream(Stream stream, JpegDataConverter dataConverter)
    {
        bool metadataOnly = dataConverter == null;
        var huffmanDecoder = new JpegHuffmanDecoder(stream, dataConverter!);
        object frame = null!;
        Span<byte> markerBuffer = stackalloc byte[2];
        stream.Read(markerBuffer);
        JpegMarker marker = new((JpegMarkerType)markerBuffer[1], 0);
        if (marker.MarkerType != JpegMarkerType.StartOfImage)
        {
            throw new InvalidDataException("Image doesn't contain 'start of image' marker!");
        }

        marker = FindNextMarker(stream);

        while(marker.MarkerType != JpegMarkerType.EndOfImage)
        {
            if(!marker.IsInvalid)
            {
                uint markerSize = (uint)(ReadUint16(stream, markerBuffer) - 2);
                if((stream.Length - stream.Position) < markerSize)
                {
                    // ToDo: Enough data cases
                    return;
                }

                switch(marker.MarkerType)
                {
                    case JpegMarkerType.StartOfFrame0:
                    case JpegMarkerType.StartOfFrame1:
                    case JpegMarkerType.StartOfFrame2:
                        if(frame != null)
                        {
                            throw new InvalidDataException("Multiple 'start of frame' markers detected!");
                        }
                        break;
                    case JpegMarkerType.StartOfFrame9:
                    case JpegMarkerType.StartOfFrame10:
                    case JpegMarkerType.StartOfFrame13:
                    case JpegMarkerType.StartOfFrame14:
                        if(frame != null)
                        {
                            throw new InvalidDataException("Multiple 'start of frame' markers detected!");
                        }
                        break;
                    case JpegMarkerType.StartOfFrame3:
                    case JpegMarkerType.StartOfFrame5:
                    case JpegMarkerType.StartOfFrame6:
                    case JpegMarkerType.StartOfFrame7:
                    case JpegMarkerType.StartOfFrame11:
                    case JpegMarkerType.StartOfFrame15:
                        throw new NotSupportedException($"Jpeg decoding with marker '{marker.MarkerType}' is not supported!");


                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static JpegMarker FindNextMarker(Stream stream)
    {
        while (true)
        {
            int data = stream.ReadByte();
            if (data == -1)
            {
                return new JpegMarker(JpegMarkerType.EndOfImage, stream.Length - 2);
            }

            JpegMarkerType markerType = (JpegMarkerType)data;

            if (markerType == JpegMarkerType.Prefix)
            {
                while (markerType == JpegMarkerType.Prefix)
                {
                    data = stream.ReadByte();
                    if (data == -1)
                    {
                        return new JpegMarker(JpegMarkerType.EndOfImage, stream.Length - 2);
                    }

                    markerType = (JpegMarkerType)data;
                }

                if (data is not 0 and (< (byte)JpegMarkerType.Restart0 or > (byte)JpegMarkerType.Restart7))
                {
                    return new JpegMarker(markerType, stream.Position - 2);
                }
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ushort ReadUint16(Stream stream, Span<byte> buffer)
    {
        int bytesRead = stream.Read(buffer.Slice(0, 2));
        if (bytesRead != 2)
        {
            throw new InvalidDataException("Jpeg contains not enough data!");
        }

        return BinaryPrimitives.ReadUInt16BigEndian(buffer);
    }
}
