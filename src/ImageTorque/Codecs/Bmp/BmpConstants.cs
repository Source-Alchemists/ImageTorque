namespace ImageTorque.Codecs.Bmp;

internal static class BmpConstants
{
    public const int Rgb16RMask = 0x7C00;
    public const int Rgb16GMask = 0x3E0;
    public const int Rgb16BMask = 0x1F;

    internal static class HeaderFields
    {
        public const short Bitmap = 0x424D;
        public const short BitmapArray = 0x4241;
        public const int ColorIcon = 0x4349;
        public const int ColorPointer = 0x4350;
        public const int Icon = 0x4943;
        public const int Pointer = 0x5054;
    }
}
