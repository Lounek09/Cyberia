using Cyberia.Amphibian;
using Cyberia.Api;
using Cyberia.Salamandra;

namespace Cyberia;

/// <summary>
/// Represents the configuration settings for the Cyberia application.
/// </summary>
public sealed class CyberiaConfig
{
    /// <summary>
    /// Gets a value indicating whether the bot is launched at startup.
    /// </summary>
    public required bool EnableSalamandra { get; init; }

    /// <summary>
    /// Gets a value indicating whether the website is launched at startup.
    /// </summary>
    public required bool EnableAmphibian { get; init; }

    /// <summary>
    /// Gets a value indicating whether the automatic check of Cytrus is enabled.
    /// </summary>
    public required bool EnableCheckCytrus { get; init; }

    /// <summary>
    /// Gets the interval between each Cytrus check.
    /// </summary>
    public required TimeSpan CheckCytrusInterval { get; init; }

    /// <summary>
    /// Gets a value indicating whether the automatic check of the Official lang is enabled.
    /// </summary>
    public required bool EnableCheckLang { get; init; }

    /// <summary>
    /// Gets the interval between each Official lang check.
    /// </summary>
    public required TimeSpan CheckLangInterval { get; init; }

    /// <summary>
    /// Gets a value indicating whether the automatic check of Beta Lang is enabled.
    /// </summary>
    public required bool EnableCheckBetaLang { get; init; }

    /// <summary>
    /// Gets the interval between each Beta Lang check.
    /// </summary>
    public required TimeSpan CheckBetaLangInterval { get; init; }

    /// <summary>
    /// Gets a value indicating whether the automatic check of Temporis Lang is enabled.
    /// </summary>
    public required bool EnableCheckTemporisLang { get; init; }

    /// <summary>
    /// Gets the interval between each Temporis Lang check.
    /// </summary>
    public required TimeSpan CheckTemporisLangInterval { get; init; }

    /// <summary>
    /// Gets the configuration related to the API.
    /// </summary>
    public required DofusApiConfig DofusApiConfig { get; init; }

    /// <summary>
    /// Gets the configuration related to the bot.
    /// </summary>
    public required BotConfig BotConfig { get; init; }

    /// <summary>
    /// Gets the configuration related to the website.
    /// </summary>
    public required WebConfig WebConfig { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CyberiaConfig"/> class.
    /// </summary>
    public CyberiaConfig() { }

    public bool Validate()
    {
        if (DofusApiConfig.SupportedLanguages.Count == 0)
        {
            Log.Error("You must specify at least one supported language in the API configuration.");
            return false;
        }

        if (EnableAmphibian)
        {
            if (WebConfig.Urls.Count == 0)
            {
                Log.Error("You must specify at least one URL in the Web configuration.");
                return false;
            }
        }

        return true;
    }
}
