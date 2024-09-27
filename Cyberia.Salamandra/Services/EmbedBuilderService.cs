using Cyberia.Api;
using Cyberia.Api.Managers;
using Cyberia.Salamandra.Enums;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Globalization;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to handle the base embeds creation logic.
/// </summary>
public sealed class EmbedBuilderService
{
    private readonly string _username;
    private readonly string _baseIconUrl;
    private readonly string _footerIconUrl;
    private readonly DiscordColor _embedColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbedBuilderService"/> class.
    /// </summary>
    /// <param name="config">The bot configuration.</param>
    /// <param name="client">The Discord client.</param>
    public EmbedBuilderService(BotConfig config, DiscordClient client)
    {
        _username = client.CurrentUser.Username;
        _baseIconUrl = $"{DofusApi.Config.CdnUrl}/images/discord/embed_categories";
        _footerIconUrl = $"{DofusApi.Config.CdnUrl}/images/discord/mini-salamandra.png";
        _embedColor = new(config.EmbedColor);
    }

    /// <summary>
    /// Creates a new embed builder with the specified category and author text.
    /// </summary>
    /// <param name="category">The embed category.</param>
    /// <param name="authorText">The author text.</param>
    /// <returns>The created embed builder.</returns>
    public DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText)
    {
        var now = DateTime.Now;
        var date = now.ToRolePlayString();
        var time = now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern);

        return new DiscordEmbedBuilder()
            .WithColor(_embedColor)
            .WithAuthor(authorText, iconUrl: GetIconUrl(category))
            .WithFooter($"{_username} • {date} - {time}", _footerIconUrl);
    }

    /// <summary>
    /// Gets the icon URL for the specified category.
    /// </summary>
    /// <param name="category">The embed category.</param>
    /// <returns>The icon URL.</returns>
    private string GetIconUrl(EmbedCategory category)
    {
        return $"{_baseIconUrl}/{(int)category}.png";
    }
}
