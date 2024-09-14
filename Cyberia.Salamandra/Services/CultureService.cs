using Cyberia.Database.Repositories;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Services;

public sealed class CultureService
{
    private readonly UserRepository _userRepository;

    public CultureService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Set the culture of the current thread to the culture of the user.
    /// </summary>
    /// <param name="interaction">The interaction to get the culture from.</param>
    public async Task SetCultureAsync(DiscordInteraction interaction)
    {
        var user = await _userRepository.GetAsync(interaction.User.Id);

        var culture = string.IsNullOrEmpty(user?.Locale)
            ? interaction.GetCulture()
            : new CultureInfo(user.Locale);

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }
}
