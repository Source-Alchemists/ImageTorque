using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Png;

internal sealed record PngInfos
{
    public byte? BitDepth { get; init; }
    public PngColorType? ColorType { get; init; }
    public PngInterlaceMode? InterlaceMethod { get; init; } = PngInterlaceMode.None;
    public PixelResolutionUnit ResolutionUnits { get; init; }
    public double HorizontalResolution { get; init; }
    public double VerticalResolution { get; init; }
    public byte[] ColorPalette { get; set; } = [];
    public Rgb24[] ColorTable { get; set; } = [];
    public int BytesPerPixel { get; set; }
    public int BytesPerScanline { get; set; }
    public int BytesPerSample { get; set; }
}
