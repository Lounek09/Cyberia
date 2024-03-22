using Cyberia.Api;
using Cyberia.Salamandra;

namespace Cyberia;

/// <summary>
/// Represents the configuration settings for the Cyberia application.
/// </summary>
public sealed class CyberiaConfig
{
    /// <summary>
    /// Gets a value indicating whether Salamandra is enabled.
    /// </summary>
    public bool EnableSalamandra { get; init; }

    /// <summary>
    /// Gets a value indicating whether Salamandra Web is enabled.
    /// </summary>
    public bool EnableAmphibian { get; init; }

    /// <summary>
    /// Gets a value indicating whether Cytrus check is enabled.
    /// </summary>
    public bool EnableCheckCytrus { get; init; }

    /// <summary>
    /// Gets the interval for checking Cytrus.
    /// </summary>
    public TimeSpan CheckCytrusInterval { get; init; }

    /// <summary>
    /// Gets a value indicating whether Lang check is enabled.
    /// </summary>
    public bool EnableCheckLang { get; init; }

    /// <summary>
    /// Gets the interval for checking Lang.
    /// </summary>
    public TimeSpan CheckLangInterval { get; init; }

    /// <summary>
    /// Gets a value indicating whether Beta Lang check is enabled.
    /// </summary>
    public bool EnableCheckBetaLang { get; init; }

    /// <summary>
    /// Gets the interval for checking Beta Lang.
    /// </summary>
    public TimeSpan CheckBetaLangInterval { get; init; }

    /// <summary>
    /// Gets a value indicating whether Temporis Lang check is enabled.
    /// </summary>
    public bool EnableCheckTemporisLang { get; init; }

    /// <summary>
    /// Gets the interval for checking Temporis Lang.
    /// </summary>
    public TimeSpan CheckTemporisLangInterval { get; init; }

    /// <summary>
    /// Gets the API configuration settings.
    /// </summary>
    public ApiConfig ApiConfig { get; init; }

    /// <summary>
    /// Gets the Bot configuration settings.
    /// </summary>
    public BotConfig BotConfig { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyberiaConfig"/> class.
    /// </summary>
    public CyberiaConfig()
    {
        ApiConfig = new();
        BotConfig = new();
    }
}
