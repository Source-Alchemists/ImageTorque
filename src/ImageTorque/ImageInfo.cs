namespace ImageTorque;

public sealed record ImageInfo
{
    public ImageInfo() { }

    public ImageInfo(int width, int height, int bitsPerPixel)
    {
        Width = width;
        Height = height;
        BitsPerPixel = bitsPerPixel;
    }

    public int Width { get; }

    public int Height { get; }

    public int BitsPerPixel { get; }
}
