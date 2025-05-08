using Cyberia.Api;
using Cyberia.Database.Repositories;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle culture-related logic for Discord users.
/// </summary>
public sealed class CultureService
{
    private readonly DiscordCachedUserRepository _discordCachedUserRepository;
    private readonly DofusApiConfig _dofusApiConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureService"/> class.
    /// </summary>
    /// <param name="discordCachedUserRepository">The repository to get the cached user from.</param>
    public CultureService(DiscordCachedUserRepository discordCachedUserRepository, DofusApiConfig dofusApiConfig)
    {
        _discordCachedUserRepository = discordCachedUserRepository;
        _dofusApiConfig = dofusApiConfig;
    }

    /// <summary>
    /// Gets the locale of the user from the database or the provided interaction if not found.
    /// </summary>
    /// <param name="interaction">The interaction to get the locale from.</param>
    /// <returns>The locale of the user.</returns>
    public async Task<string?> GetLocaleAsync(DiscordInteraction interaction)
    {
        var user = await _discordCachedUserRepository.GetAsync(interaction.User.Id);

        return user?.Locale ?? interaction.Locale ?? interaction.GuildLocale;
    }

    /// <summary>
    /// Gets the culture of the user from the database or the provided interaction if not found.
    /// </summary>
    /// <param name="interaction">The interaction to get the culture from.</param>
    /// <returns>The culture of the user.</returns>
    public async Task<CultureInfo> GetCultureAsync(DiscordInteraction interaction)
    {
        var user = await _discordCachedUserRepository.GetAsync(interaction.User.Id);

        return string.IsNullOrEmpty(user?.Locale)
            ? GetInteractionCulture(interaction)
            : CultureInfo.GetCultureInfo(user.Locale);
    }

    private CultureInfo GetInteractionCulture(DiscordInteraction interaction)
    {
        var locale = interaction.Locale ?? interaction.GuildLocale;
        if (locale is null)
        {
            return _dofusApiConfig.SupportedLanguages[0].ToCulture();
        }

        try
        {
            return CultureInfo.GetCultureInfo(locale[..2]);
        }
        catch (CultureNotFoundException e)
        {
            Log.Error(e, $"Unknown {locale} culture from Discord.");
        }

        return _dofusApiConfig.SupportedLanguages[0].ToCulture();
    }
}
