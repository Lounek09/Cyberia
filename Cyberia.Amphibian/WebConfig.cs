namespace Cyberia.Amphibian;

public sealed class WebConfig
{
    /// <summary>
    /// Gets the environment name.
    /// </summary>
    public string Environment { get; init; }

    /// <summary>
    /// Gets the base URL.
    /// </summary>
    public IReadOnlyList<string> Urls { get; init; }

    /// <summary>
    /// Gets the Git URL of the repository.
    /// </summary>
    public string GitUrl { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WebConfig"/> class.
    /// </summary>
    public WebConfig()
    {
        Environment = string.Empty;
        Urls = [];
        GitUrl = string.Empty;
    }
}
