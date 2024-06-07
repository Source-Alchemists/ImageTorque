using System.Buffers.Binary;

namespace ImageTorque.Codecs.Png;

/// <summary>
/// Represents the header information of a PNG image.
/// </summary>
/// <param name="width">The width of the image in pixels.</param>
/// <param name="height">The height of the image in pixels.</param>
/// <param name="bitDepth">The bit depth of the image.</param>
/// <param name="colorType">The color type of the image.</param>
/// <param name="compressionMethod">The compression method used in the image.</param>
/// <param name="filterMethod">The filter method used in the image.</param>
internal readonly struct PngHeader(int width, int height, byte bitDepth, PngColorType colorType, byte compressionMethod, byte filterMethod)
{
    /// <summary>
    /// The size of the PNG header in bytes.
    /// </summary>
    public const int Size = 13;

    /// <summary>
    /// Gets the width of the image in pixels.
    /// </summary>
    public int Width { get; } = width;

    /// <summary>
    /// Gets the height of the image in pixels.
    /// </summary>
    public int Height { get; } = height;

    /// <summary>
    /// Gets the bit depth of the image.
    /// </summary>
    public byte BitDepth { get; } = bitDepth;

    /// <summary>
    /// Gets the color type of the image.
    /// </summary>
    public PngColorType ColorType { get; } = colorType;

    /// <summary>
    /// Gets the compression method used in the image.
    /// </summary>
    public byte CompressionMethod { get; } = compressionMethod;

    /// <summary>
    /// Gets the filter method used in the image.
    /// </summary>
    public byte FilterMethod { get; } = filterMethod;

    /// <summary>
    /// Validates the PNG header.
    /// </summary>
    /// <exception cref="NotSupportedException">Thrown when the header contains unsupported values.</exception>
    public void Validate()
    {
        if (!PngConstants.ColorTypes.TryGetValue(ColorType, out byte[]? supportedBitDepths))
        {
            throw new NotSupportedException($"Unsupported color type '{ColorType}'.");
        }

        if (supportedBitDepths.AsSpan().IndexOf(BitDepth) == -1)
        {
            throw new NotSupportedException($"Unsupported bit depth '{BitDepth}'.");
        }

        if (FilterMethod != 0)
        {
            throw new NotSupportedException($"Invalid filter method '{FilterMethod}'.");
        }
    }

    /// <summary>
    /// Parses the PNG header from the given data.
    /// </summary>
    /// <param name="data">The data containing the PNG header.</param>
    /// <returns>The parsed PNG header.</returns>
    public static PngHeader Parse(ReadOnlySpan<byte> data)
        => new(
            width: BinaryPrimitives.ReadInt32BigEndian(data[..4]),
            height: BinaryPrimitives.ReadInt32BigEndian(data.Slice(4, 4)),
            bitDepth: data[8],
            colorType: (PngColorType)data[9],
            compressionMethod: data[10],
            filterMethod: data[11]);
}
