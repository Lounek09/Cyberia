using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Enums;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Services;

public sealed class EmbedBuilderService
{
    private readonly string _username;
    private readonly string _baseIconUrl;
    private readonly string _footerIconUrl;
    private readonly DiscordColor s_embedColor;

    public EmbedBuilderService(BotConfig config, DiscordClient client)
    {
        _username = client.CurrentUser.Username;
        _baseIconUrl = $"{DofusApi.Config.CdnUrl}/images/discord/embed_categories";
        _footerIconUrl = $"{DofusApi.Config.CdnUrl}/images/discord/mini-salamandra.png";
        s_embedColor = new(config.EmbedColor);
    }

    public DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText)
    {
        var now = DateTime.Now;
        var date = now.ToRolePlayString();
        var time = now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

        return new DiscordEmbedBuilder()
            .WithColor(s_embedColor)
            .WithAuthor(authorText, iconUrl: GetIconUrl(category))
            .WithFooter($"{_username} • {date} - {time}", _footerIconUrl);
    }

    private string GetIconUrl(EmbedCategory category)
    {
        return $"{_baseIconUrl}/{(int)category}.png";
    }
}
