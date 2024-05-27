namespace ImageTorque.Formats.Png;

internal enum PngChunkType : uint
{
    Header = 0x49484452U,
    Palette = 0x504C5445U,
    Data = 0x49444154U,
    End = 0x49454E44U
}
