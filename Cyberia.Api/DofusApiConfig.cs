﻿using Cyberia.Langzilla.Enums;

namespace Cyberia.Api;

/// <summary>
/// Represents the configuration settings for the Cyberia API.
/// </summary>
public sealed class DofusApiConfig
{
    /// <summary>
    /// Gets the URL of the <a href="https://github.com/Lounek09/Cyberia.Cdn">CDN</a>.
    /// </summary>
    public string CdnUrl { get; init; }

    /// <summary>
    /// Gets the type of lang loaded at startup.
    /// </summary>
    public required LangType Type { get; init; }

    /// <summary>
    /// Gets the language of lang from which the base data is loaded, only the translations will be loaded from the others listed in <see cref="SupportedLanguages"/>.
    /// </summary>
    public required Language BaseLanguage { get; init; }

    /// <summary>
    /// Gets the list of the supported languages, the first one will be the default language.
    /// </summary>
    public required IReadOnlyList<Language> SupportedLanguages { get; init; }

    /// <summary>
    /// Gets the invitation URL of the support Discord guild.
    /// </summary>
    public string DiscordInviteUrl { get; init; }

    /// <summary>
    /// Gets the URL of the repository.
    /// </summary>
    public string GitRepositoryUrl { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DofusApiConfig"/> class.
    /// </summary>
    public DofusApiConfig()
    {
        CdnUrl = "https://raw.githubusercontent.com/Lounek09/Cyberia.Cdn/refs/heads/main";
        DiscordInviteUrl = string.Empty;
        GitRepositoryUrl = string.Empty;
    }
}
