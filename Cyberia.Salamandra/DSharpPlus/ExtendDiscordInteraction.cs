using Cyberia.Api;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.DSharpPlus;

public static class ExtendDiscordInteraction
{
    /// <summary>
    /// Get the culture of the interaction.
    /// </summary>
    /// <param name="interaction">The interaction to get the culture from.</param>
    /// <returns>The culture or the default culture if the locale is invalid.</returns>
    public static CultureInfo GetCulture(this DiscordInteraction interaction)
    {
        var locale = interaction.Locale ?? interaction.GuildLocale;
        if (locale is null || locale.Length < 2)
        {
            return DofusApi.Config.SupportedLanguages[0].ToCulture();
        }

        try
        {
            return new CultureInfo(locale[..2]);
        }
        catch (CultureNotFoundException e)
        {
            Log.Error(e, $"Unknown culture {locale} from Discord.");
        }

        return DofusApi.Config.SupportedLanguages[0].ToCulture();
    }
}
