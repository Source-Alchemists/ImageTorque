namespace ImageTorque.Codecs.Jpeg;

internal readonly record struct JpegMarker
{
    public JpegMarkerType MarkerType { get; }
    public long Position { get; }
    public bool IsInvalid { get; }

    public JpegMarker(JpegMarkerType markerType, long position, bool invalid = false)
    {
        MarkerType = markerType;
        Position = position;
        IsInvalid = invalid;
    }
}
