namespace Cyberia.Amphibian;

/// <summary>
/// Represents the configuration settings for the Cyberia website.
/// </summary>
public sealed class WebConfig
{
    /// <summary>
    /// Gets the environment name.
    /// </summary>
    public required string Environment { get; init; }

    /// <summary>
    /// Gets the URLs the host will listen on.
    /// </summary>
    public required IReadOnlyList<string> Urls { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebConfig"/> class.
    /// </summary>
    public WebConfig() { }
}
