namespace ImageTorque;

internal sealed record ImageInfo
{
    public ImageInfo(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }
}
