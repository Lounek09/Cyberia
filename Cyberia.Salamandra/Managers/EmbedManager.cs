using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Enums;

using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Managers;

public static class EmbedManager
{
    private static readonly string s_baseIconUrl = $"{DofusApi.Config.CdnUrl}/images/discord/embed_categories";
    private static readonly string s_footerIconUrl = $"{DofusApi.Config.CdnUrl}/images/discord/mini-salamandra.png";
    private static readonly DiscordColor s_embedColor = new(Bot.Config.EmbedColor);

    public static DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText)
    {
        var now = DateTime.Now;
        var date = now.ToRolePlayString();
        var time = now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

        return new DiscordEmbedBuilder()
            .WithColor(s_embedColor)
            .WithAuthor(authorText, iconUrl: GetIconUrl(category))
            .WithFooter($"{Bot.Client.CurrentUser.Username} • {date} - {time}", s_footerIconUrl);
    }

    private static string GetIconUrl(EmbedCategory category)
    {
        return $"{s_baseIconUrl}/{(int)category}.png";
    }
}
