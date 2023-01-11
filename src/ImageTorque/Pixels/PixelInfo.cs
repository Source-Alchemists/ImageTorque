namespace ImageTorque.Pixels;

public record PixelInfo
{
    public int BytesPerPixel { get; }
    public bool IsColor { get; }
    public int ChannelsPerPixel { get; }
    public int ChannelsPerImage { get; }
    public PixelType PixelType { get; }

    public PixelInfo(PixelType pixelType, int bytesPerPixel, int channelsPerPixel, int channelsPerImage, bool isColor)
    {
        PixelType = pixelType;
        BytesPerPixel = bytesPerPixel;
        ChannelsPerPixel = channelsPerPixel;
        ChannelsPerImage = channelsPerImage;
        IsColor = isColor;
    }
}