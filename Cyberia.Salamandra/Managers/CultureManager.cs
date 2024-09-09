using Cyberia.Api;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Managers;
public static class CultureManager
{
    /// <summary>
    /// Set the culture of the current thread to the one of the lang language.
    /// </summary>
    /// <param name="language">The lang language to get the culture from.</param>
    public static void SetCulture(LangLanguage language)
    {
        var culture = language.ToCulture();
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    /// <summary>
    /// Set the culture of the current thread to the one of the interaction.
    /// </summary>
    /// <param name="interaction">The interaction to get the culture from.</param>
    public static void SetCulture(DiscordInteraction interaction)
    {
        //TODO: temporary until translations of commands are implemented
        //var culture = GetCultureFromDiscordLocale(interaction.Locale ?? interaction.GuildLocale);
        var culture = new CultureInfo("fr");
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    /// <summary>
    /// Get the culture from a Discord locale string.
    /// </summary>
    /// <param name="discordLocale">The Discord locale string.</param>
    /// <returns>The culture or the default culture if the locale is invalid.</returns>
    public static CultureInfo GetCultureFromDiscordLocale(string? discordLocale)
    {
        if (discordLocale is null || discordLocale.Length < 2)
        {
            return DofusApi.Config.SupportedLanguages[0].ToCulture();
        }

        try
        {
            return new CultureInfo(discordLocale[..2]);
        }
        catch(CultureNotFoundException e)
        {
            Log.Error(e, $"Unknown culture {discordLocale} from Discord.");
        }

        return DofusApi.Config.SupportedLanguages[0].ToCulture();
    }
}
