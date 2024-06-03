using ImageTorque.Codecs;

namespace ImageTorque;

public static class ConfigurationFactory
{
    public static Configuration Build(IReadOnlyCollection<ICodec> codecs)
    {
        return new Configuration()
        {
            MaxHeaderSize = codecs.Max(f => f.HeaderSize),
            Codecs = codecs
        };
    }
}
