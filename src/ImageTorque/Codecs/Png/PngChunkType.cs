namespace ImageTorque.Codecs.Png;

/// <summary>
/// <see href="https://www.w3.org/TR/2003/REC-PNG-20031110/#11Critical-chunks"/>
/// </summary>
internal enum PngChunkType : uint
{
    ImageHeader = 0x49484452U, // IHDR
    Palette = 0x504C5445U, // PLTE
    ImageData = 0x49444154U, // IDAT
    ImageTrailer = 0x49454E44U // IEND
}
