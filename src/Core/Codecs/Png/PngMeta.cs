using ImageTorque.Pixels;

namespace ImageTorque.Codecs.Png;

internal sealed record PngMeta
{
    public byte? BitDepth { get; init; }
    public PngColorType? ColorType { get; init; }
    public PngInterlaceMode? InterlaceMethod { get; set; } = PngInterlaceMode.None;
    public PixelResolutionUnit ResolutionUnits { get; set; }
    public double HorizontalResolution { get; set; }
    public double VerticalResolution { get; set; }
    public byte[] ColorPalette { get; set; } = [];
    public Rgb24[] ColorTable { get; set; } = [];
    public int BytesPerPixel { get; set; }
    public int BytesPerScanline { get; set; }
    public int BytesPerSample { get; set; }
}
