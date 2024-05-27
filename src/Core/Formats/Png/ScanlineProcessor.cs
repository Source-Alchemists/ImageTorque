using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImageTorque.Pixels;

namespace ImageTorque.Formats.Png;

internal static class ScanlineProcessor
{
    public static void ProcessGrayscaleScanline<TPixel>(int bitDepth, in FrameControl frameControl, ReadOnlySpan<byte> scanlineSpan, Span<TPixel> rowSpan) where TPixel : unmanaged, IPixel =>
        ProcessInterlacedGrayscaleScanline(bitDepth, frameControl, scanlineSpan, rowSpan, 0, 1);

    public static void ProcessInterlacedGrayscaleScanline<TPixel>(int bitDepth, in FrameControl frameControl, ReadOnlySpan<byte> scanlineSpan, Span<TPixel> rowSpan, uint pixelOffset, uint increment) where TPixel : unmanaged, IPixel
    {
        uint offset = pixelOffset + frameControl.XOffset;
        ref byte scanlineSpanRef = ref MemoryMarshal.GetReference(scanlineSpan);
        ref TPixel rowSpanRef = ref MemoryMarshal.GetReference(rowSpan);
        int scaleFactor = 255 / (NumericMath.GetColorCountForBitDepth(bitDepth) - 1);

        if (bitDepth == 16)
        {
            int o = 0;
            for (nuint x = offset; x < frameControl.XMax; x += increment, o += 2)
            {
                ushort luminance = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o, 2));
                Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)Unsafe.As<ushort, L16>(ref luminance);
            }
        }
        else
        {
            for (nuint x = offset, o = 0; x < frameControl.XMax; x += increment, o++)
            {
                byte luminance = (byte)(Unsafe.Add(ref scanlineSpanRef, o) * scaleFactor);
                Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)Unsafe.As<byte, L8>(ref luminance);
            }
        }
    }

    public static void ProcessPaletteScanline<TPixel>(
        in FrameControl frameControl,
        ReadOnlySpan<byte> scanlineSpan,
        Span<TPixel> rowSpan,
        ReadOnlyMemory<Rgb24>? palette)
        where TPixel : unmanaged, IPixel =>
        ProcessInterlacedPaletteScanline(
            frameControl,
            scanlineSpan,
            rowSpan,
            0,
            1,
            palette);

    public static void ProcessInterlacedPaletteScanline<TPixel>(
        in FrameControl frameControl,
        ReadOnlySpan<byte> scanlineSpan,
        Span<TPixel> rowSpan,
        uint pixelOffset,
        uint increment,
        ReadOnlyMemory<Rgb24>? palette)
        where TPixel : unmanaged, IPixel
    {
        if (palette is null)
        {
            throw new InvalidDataException("Missing color palette!");
        }

        ref byte scanlineSpanRef = ref MemoryMarshal.GetReference(scanlineSpan);
        ref TPixel rowSpanRef = ref MemoryMarshal.GetReference(rowSpan);
        ref Rgb24 paletteBase = ref MemoryMarshal.GetReference(palette.Value.Span);
        uint offset = pixelOffset + frameControl.XOffset;
        int maxIndex = palette.Value.Length - 1;

        for (nuint x = offset, o = 0; x < frameControl.XMax; x += increment, o++)
        {
            uint index = Unsafe.Add(ref scanlineSpanRef, o);
            Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)Unsafe.Add(ref paletteBase, (int)Math.Min(index, maxIndex));
        }
    }

    public static void ProcessRgbScanline<TPixel>(
        int bitDepth,
        in FrameControl frameControl,
        ReadOnlySpan<byte> scanlineSpan,
        Span<TPixel> rowSpan,
        int bytesPerPixel,
        int bytesPerSample)
       where TPixel : unmanaged, IPixel =>
       ProcessInterlacedRgbScanline(
           bitDepth,
           frameControl,
           scanlineSpan,
           rowSpan,
           0,
           1,
           bytesPerPixel,
           bytesPerSample);

    public static void ProcessInterlacedRgbScanline<TPixel>(
        int bitDepth,
        in FrameControl frameControl,
        ReadOnlySpan<byte> scanlineSpan,
        Span<TPixel> rowSpan,
        uint pixelOffset,
        uint increment,
        int bytesPerPixel,
        int bytesPerSample)
        where TPixel : unmanaged, IPixel
    {
        uint offset = pixelOffset + frameControl.XOffset;
        ref byte scanlineSpanRef = ref MemoryMarshal.GetReference(scanlineSpan);
        ref TPixel rowSpanRef = ref MemoryMarshal.GetReference(rowSpan);

        if (bitDepth == 16)
        {
            int o = 0;
            for (nuint x = offset; x < frameControl.XMax; x += increment, o += bytesPerPixel)
            {
                ushort r = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o, bytesPerSample));
                ushort g = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o + bytesPerSample, bytesPerSample));
                ushort b = BinaryPrimitives.ReadUInt16BigEndian(scanlineSpan.Slice(o + (2 * bytesPerSample), bytesPerSample));
                Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)new Rgb48(r, g, b);
            }
        }
        else if (pixelOffset == 0 && increment == 1)
        {
            ReadOnlySpan<Rgb24> source = MemoryMarshal.Cast<byte, Rgb24>(scanlineSpan[..(int)(frameControl.Width * bytesPerPixel)]).Slice(0, (int)frameControl.Width);
            ref Rgb24 sourceBase = ref MemoryMarshal.GetReference(source);
            ref TPixel destinationBase = ref MemoryMarshal.GetReference(rowSpan.Slice((int)frameControl.XOffset, (int)frameControl.Width));

            for (nuint i = 0; i < (uint)source.Length; i++)
            {
                Unsafe.Add(ref destinationBase, i) = (TPixel)(IPixel)Unsafe.Add(ref sourceBase, i);
            }
        }
        else
        {
            int o = 0;
            for (nuint x = offset; x < frameControl.XMax; x += increment, o += bytesPerPixel)
            {
                byte r = Unsafe.Add(ref scanlineSpanRef, (uint)o);
                byte g = Unsafe.Add(ref scanlineSpanRef, (uint)(o + bytesPerSample));
                byte b = Unsafe.Add(ref scanlineSpanRef, (uint)(o + (2 * bytesPerSample)));
                Unsafe.Add(ref rowSpanRef, x) = (TPixel)(IPixel)new Rgb24(r, g, b);
            }
        }
    }
}
