using System.Runtime.CompilerServices;

namespace ImageTorque.Pixels;

public record struct Mono8 : IPackedPixel<byte>
{
    public const byte White = 255;

    public const byte Black = 0;

    public byte Value { get; set; }

    public PixelInfo PixelInfo
    {
        get
        {
            return new PixelInfo(PixelType.Mono8, 1, 1, 1, false);
        }
    }

    public Mono8(byte value)
    {
        Value = value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator byte(Mono8 mono)
    {
        return mono.Value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Mono8(byte value)
    {
        return new Mono8(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mono ToMono()
    {
        return new Mono(Convert.ToSingle(Value));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Mono16 ToMono16()
    {
        return new Mono16(Convert.ToUInt16(Value));
    }
}
