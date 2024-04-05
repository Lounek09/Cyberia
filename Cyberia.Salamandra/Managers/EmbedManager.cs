using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Enums;

using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Managers;

public static class EmbedManager
{
    private static readonly string s_baseIconUrl = $"{DofusApi.Config.CdnUrl}/images/embed_categories";
    private static readonly string s_footerIconUrl = $"{DofusApi.Config.CdnUrl}/images/mini-salamandra.png";
    private static readonly DiscordColor s_embedColor = new(Bot.Config.EmbedColor);

    public static DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText)
    {
        return new DiscordEmbedBuilder()
            .WithColor(s_embedColor)
            .WithFooter(
                $"{Bot.Client.CurrentUser.Username} • {DateTime.Now.ToRolePlayString()} - {DateTime.Now:HH:mm}",
                s_footerIconUrl)
            .WithAuthor(authorText, iconUrl: GetIconUrl(category));
    }

    private static string GetIconUrl(EmbedCategory category)
    {
        //TODO: change the name of the images with just the id of the enum in the new Salamandra.Cdn
        return $"{s_baseIconUrl}/category_{category.ToString().ToLower()}.png";
    }
}
