namespace Cyberia.Salamandra;

/// <summary>
/// Represents the configuration settings for the Cyberia bot.
/// </summary>
public sealed class BotConfig
{
    /// <summary>
    /// Gets the Discord bot token.
    /// </summary>
    public string Token { get; init; }

    /// <summary>
    /// Gets the color of the embeds (e.g. #CD853F).
    /// </summary>
    public string EmbedColor { get; init; }

    /// <summary>
    /// Gets the guild ID where the admin commands will be registered.
    /// </summary>
    public ulong AdminGuildId { get; init; }

    /// <summary>
    /// Gets the invitation URL of the bot.
    /// </summary>
    public string BotInviteUrl { get; init; }

    /// <summary>
    /// Gets the channel ID where logs from certains events (e.g. guild added/removed) will be sent.
    /// </summary>
    public ulong LogChannelId { get; init; }

    /// <summary>
    /// Gets the channel ID where errors related to command execution will be sent.
    /// </summary>
    public ulong ErrorChannelId { get; init; }

    /// <summary>
    /// Gets the forum channel ID where the automatic lang diff will be sent.
    /// </summary>
    public ulong LangForumChannelId { get; init; }

    /// <summary>
    /// Gets the channel ID where the Cytrus diff will be sent.
    /// </summary>
    public ulong CytrusChannelId { get; init; }

    /// <summary>
    /// Gets the channel ID where the game manifest diff from Cytrus will be sent.
    /// </summary>
    public ulong CytrusManifestChannelId { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BotConfig"/> class.
    /// </summary>
    public BotConfig()
    {
        Token = string.Empty;
        EmbedColor = string.Empty;
        BotInviteUrl = string.Empty;
    }
}
