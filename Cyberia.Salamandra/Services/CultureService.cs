using Cyberia.Database.Repositories;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to set the culture of the current thread to the culture of the user.
/// </summary>
public sealed class CultureService
{
    private readonly DiscordCachedUserRepository _discordCachedUserRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CultureService"/> class.
    /// </summary>
    /// <param name="discordCachedUserRepository">The user repository to get the user from.</param>
    public CultureService(DiscordCachedUserRepository discordCachedUserRepository)
    {
        _discordCachedUserRepository = discordCachedUserRepository;
    }

    /// <summary>
    /// Gets the culture of the user.
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
