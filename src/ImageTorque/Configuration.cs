using ImageTorque.Codecs;
using ImageTorque.Codecs.Bmp;
using ImageTorque.Codecs.Png;
using Microsoft.IO;

namespace ImageTorque;

public sealed record Configuration
{
    public static Configuration Default { get; set; } = CreateDefaultInstance();

    public RecyclableMemoryStreamManager StreamManager { get; } = new RecyclableMemoryStreamManager();

    public int StreamProcessingBufferSize { get; init; } = 8096;

    public bool PreferContiguousImageBuffers { get; init; } = true;

    public int MaxHeaderSize { get; init; }

    public IReadOnlyCollection<ICodec> Codecs { get; init; } = [];

    public bool UseCrcValidation { get; init; } = false; // We don't use CRC validation by default, because we running in a machine vision context

    internal static Configuration CreateDefaultInstance()
    {

        var formats = new List<ICodec> {
            new PngCodec(),
            new BmpCodec()
        };

        var configuration = new Configuration()
        {
            MaxHeaderSize = formats.Max(f => f.HeaderSize),
            Codecs = formats
        };

        return configuration;
    }
}
