using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Microsoft.Extensions.Logging;

namespace Cyberia.Salamandra;

public static class Bot
{
    public const string OUTPUT_PATH = "bot";

    public static BotConfig Config { get; private set; } = default!;
    public static DiscordClient Client { get; private set; } = default!;
    public static SlashCommandsExtension SlashCommands { get; private set; } = default!;

    public static void Initialize(BotConfig config)
    {
        Directory.CreateDirectory(OUTPUT_PATH);

        Config = config;

        Client = new(new DiscordConfiguration()
        {
            Token = Config.Token,
            LoggerFactory = new LoggerFactory().AddSerilog(Log.Logger),
            LogUnknownAuditlogs = false,
            LogUnknownEvents = false,
            Intents = DiscordIntents.Guilds | DiscordIntents.GuildMessages
        });
        Client.GuildCreated += GuildManager.OnGuildCreated;
        Client.GuildDeleted += GuildManager.OnGuildDeleted;
        Client.MessageCreated += MessageManager.OnMessageCreated;
        Client.ComponentInteractionCreated += InteractionManager.OnComponentInteractionCreated;

        SlashCommands = Client.UseSlashCommands();
        SlashCommands.RegisterCommands();
        SlashCommands.SlashCommandErrored += CommandManager.OnSlashCommandErrored;

        CytrusWatcher.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;

        LangsWatcher.CheckLangFinished += LangsManager.OnCheckLangFinished;
    }

    public static async Task LaunchAsync()
    {
        var activity = new DiscordActivity("Dofus Retro", ActivityType.Playing);
        await Client.ConnectAsync(activity);
    }
}
