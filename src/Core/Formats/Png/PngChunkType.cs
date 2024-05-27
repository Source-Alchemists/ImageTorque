namespace ImageTorque.Formats.Png;

internal enum PngChunkType : uint
{
    Data = 0x49444154U,

    End = 0x49454E44U,

    Header = 0x49484452U,

    Palette = 0x504C5445U,

    Exif = 0x65584966U,

    Gamma = 0x67414D41U,

    Physical = 0x70485973U,

    Text = 0x74455874U,

    CompressedText = 0x7A545874U,

    InternationalText = 0x69545874U,

    Transparency = 0x74524E53U,

    Time = 0x74494d45,

    Background = 0x624b4744,

    EmbeddedColorProfile = 0x69434350,

    SignificantBits = 0x73424954,

    StandardRgbColourSpace = 0x73524742,

    Histogram = 0x68495354,

    SuggestedPalette = 0x73504c54,

    Chroma = 0x6348524d,

    Cicp = 0x63494350,

    AnimationControl = 0x6163544cU,

    FrameControl = 0x6663544cU,

    FrameData = 0x66644154U,

    ProprietaryApple = 0x43674249
}
