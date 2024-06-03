namespace ImageTorque.Codecs.Jpeg;

internal enum JpegMarkerType : byte
{
    Prefix = 0xFF,
    StartOfImage = 0xD8,

    /// <summary>
    /// Indicates that this is a baseline DCT-based JPEG, and specifies the width, height, number of components, and component subsampling (e.g., 4:2:0).
    /// </summary>
    StartOfFrame0 = 0xC0,

    StartOfFrame1 = 0xC1,
    /// <summary>
    /// Indicates that this is a progressive DCT-based JPEG, and specifies the width, height, number of components, and component subsampling (e.g., 4:2:0).
    /// </summary>
    StartOfFrame2 = 0xC2,

    StartOfFrame3 = 0xC3,
    StartOfFrame5 = 0xC5,
    StartOfFrame6 = 0xC6,
    StartOfFrame7 = 0xC7,
    StartOfFrame9 = 0xC9,
    StartOfFrame10 = 0xCA,
    StartOfFrame11 = 0xCB,
    StartOfFrame13 = 0xCD,
    StartOfFrame14 = 0xCE,
    StartOfFrame15 = 0xCF,

    /// <summary>
    /// Specifies one or more Huffman tables.
    /// </summary>
    DHT = 0xC4,

    /// <summary>
    /// Specifies one or more quantization tables.
    /// </summary>
    DQT = 0xDB,

    /// <summary>
    /// Specifies the interval between RSTn markers, in Minimum Coded Units (MCUs).
    /// This marker is followed by two bytes indicating the fixed size so it can be treated like any other variable size segment.
    /// </summary>
    DRI = 0xDD,

    /// <summary>
    /// Begins a top-to-bottom scan of the image. In baseline DCT JPEG images, there is generally a single scan. Progressive DCT JPEG images usually contain multiple scans.
    /// This marker specifies which slice of data it will contain, and is immediately followed by entropy-coded data.
    /// </summary>
    StartOfScan = 0xDA,

    /// <summary>
    /// Inserted every r macroblocks, where r is the restart interval set by a DRI marker.
    /// Not used if there was no DRI marker. The low three bits of the marker code cycle in value from 0 to 7.
    /// </summary>
    Restart0 = 0xD0,

    /// <summary>
    /// Inserted every r macroblocks, where r is the restart interval set by a DRI marker.
    /// Not used if there was no DRI marker. The low three bits of the marker code cycle in value from 0 to 7.
    /// </summary>
    Restart7 = 0xD7,

    APP0 = 0xE0,
    APP1 = 0xE1,
    APP2 = 0xE2,
    APP3 = 0xE3,
    APP4 = 0xE4,
    APP5 = 0xE5,
    APP6 = 0xE6,
    APP7 = 0xE7,
    APP8 = 0xE8,
    APP9 = 0xE9,
    APP10 = 0xEA,
    APP11 = 0xEB,
    APP12 = 0xEC,
    APP13 = 0xED,
    APP14 = 0xEE,
    APP15 = 0xEF,
    DAC = 0xCC,

    COM = 0xFE,

    EndOfImage = 0xD9
}
