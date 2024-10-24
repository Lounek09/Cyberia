using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Manages the caching and retrieval of specific channels.
/// </summary>
public sealed class CachedChannelsService
{
    /// <summary>
    /// Gets the log channel.
    /// </summary>
    public DiscordChannel? LogChannel { get; internal set; }

    /// <summary>
    /// Gets the error channel.
    /// </summary>
    public DiscordChannel? ErrorChannel { get; internal set; }

    /// <summary>
    /// Gets the language forum channel.
    /// </summary>
    public DiscordForumChannel? LangsForumChannel { get; internal set; }

    /// <summary>
    /// Gets the Cytrus channel.
    /// </summary>
    public DiscordChannel? CytrusChannel { get; internal set; }

    /// <summary>
    /// Gets the Cytrus manifest channel.
    /// </summary>
    public DiscordChannel? CytrusManifestChannel { get; internal set; }

    private readonly BotConfig _botConfig;
    private readonly DiscordClient _client;

    /// <summary>
    /// Initializes a new instance of the <see cref="CachedChannelsService"/> class.
    /// </summary>
    /// <param name="botConfig">The bot configuration.</param>
    /// <param name="client">The Discord client.</param>
    public CachedChannelsService(BotConfig botConfig, DiscordClient client)
    {
        _botConfig = botConfig;
        _client = client;
    }

    /// <summary>
    /// Loads and caches the channels.
    /// </summary>
    public async Task LoadChannelsAsync()
    {
        LogChannel = await GetChannelAsync<DiscordChannel>(_botConfig.LogChannelId);
        ErrorChannel = await GetChannelAsync<DiscordChannel>(_botConfig.ErrorChannelId);
        LangsForumChannel = await GetChannelAsync<DiscordForumChannel>(_botConfig.LangForumChannelId);
        CytrusChannel = await GetChannelAsync<DiscordChannel>(_botConfig.CytrusChannelId);
        CytrusManifestChannel = await GetChannelAsync<DiscordChannel>(_botConfig.CytrusManifestChannelId);
    }

    /// <summary>
    /// Sends a message to the log channel.
    /// </summary>
    /// <param name="content">The content of the message.</param>
    public async Task SendLogMessage(string content)
    {
        if (LogChannel is null)
        {
            return;
        }

        await LogChannel.SendMessageSafeAsync(new DiscordMessageBuilder().WithContent(content));
    }

    /// <summary>
    /// Sends a message to the error channel.
    /// </summary>
    /// <param name="embed">The embed of the message.</param>
    public async Task SendErrorMessage(DiscordEmbed embed)
    {
        if (ErrorChannel is null)
        {
            return;
        }

        await ErrorChannel.SendMessageSafeAsync(new DiscordMessageBuilder().AddEmbed(embed));
    }

    /// <summary>
    /// Gets a channel by its ID and casts it to the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the channel to get.</typeparam>
    /// <param name="id">The ID of the channel.</param>
    /// <returns>The channel cast to the specified type, or <see langword="null"/> if not found or the type does not match.</returns>
    private async Task<T?> GetChannelAsync<T>(ulong id)
        where T : DiscordChannel
    {
        if (id == 0)
        {
            return null;
        }

        try
        {
            var channel = await _client.GetChannelAsync(id);
            if (channel is T typedChannel)
            {
                return typedChannel;
            }

            Log.Error("The channel with ID {ChannelId} is not of type {ChannelType}", id, typeof(T).Name);
        }
        catch
        {
            Log.Error("Failed to get the channel with ID {ChannelId}", id);
        }

        return null;
    }
}
