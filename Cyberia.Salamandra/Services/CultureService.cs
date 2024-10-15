using Cyberia.Database.Repositories;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle culture-related logic for Discord users.
/// </summary>
public sealed class CultureService
{
    private readonly DiscordCachedUserRepository _discordCachedUserRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureService"/> class.
    /// </summary>
    /// <param name="discordCachedUserRepository">The repository to get the cached user from.</param>
    public CultureService(DiscordCachedUserRepository discordCachedUserRepository)
    {
        _discordCachedUserRepository = discordCachedUserRepository;
    }

    public async Task<string?> GetDiscordLocaleAsync(DiscordInteraction interaction)
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
            ? interaction.GetCulture()
            : new CultureInfo(user.Locale);
    }
}
