using ImageTorque.Codecs;

namespace ImageTorque;

/// <summary>
/// Factory class for creating configurations.
/// </summary>
public static class ConfigurationFactory
{
    /// <summary>
    /// Builds a configuration object based on the provided codecs.
    /// </summary>
    /// <param name="codecs">The collection of codecs to include in the configuration.</param>
    /// <returns>A new configuration object.</returns>
    public static Configuration Build(IReadOnlyCollection<ICodec> codecs)
    {
        return new Configuration()
        {
            MaxHeaderSize = codecs.Max(f => f.HeaderSize),
            Codecs = codecs
        };
    }
}
