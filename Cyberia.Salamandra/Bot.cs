using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.UserCommands;
using DSharpPlus.Entities;

namespace Cyberia.Salamandra;

public static class Bot
{
    public const string OutputPath = "bot";

    public static BotConfig Config { get; private set; } = default!;
    public static DiscordClient Client { get; private set; } = default!;

    public static void Initialize(BotConfig config)
    {
        Directory.CreateDirectory(OutputPath);

        Config = config;

        Client = DiscordClientBuilder.CreateDefault(Config.Token, DiscordIntents.Guilds)
            .ConfigureLogging(logger => logger.AddSerilog(Log.Logger))
            .ConfigureEventHandlers(eventHandler =>
            {
                eventHandler.HandleGuildDownloadCompleted(ClientManager.OnGuildDownloadCompleted);
                eventHandler.HandleGuildCreated(GuildManager.OnGuildCreated);
                eventHandler.HandleGuildDeleted(GuildManager.OnGuildDeleted);
                eventHandler.HandleComponentInteractionCreated(InteractionManager.OnComponentInteractionCreated);
            })
            .ConfigureExtraFeatures(config =>
            {
                config.LogUnknownAuditlogs = false;
                config.LogUnknownEvents = false;
            })
            .UseCommands(
                setup =>
                {
                    setup.CommandErrored += CommandManager.OnCommandErrored;
                    setup.AddProcessor(new SlashCommandProcessor());
                    setup.AddProcessor(new UserCommandProcessor());
                    setup.RegisterCommands(Config.AdminGuildId);
                },
                new CommandsConfiguration()
                {
                    UseDefaultCommandErrorHandler = false,
                    RegisterDefaultCommandProcessors = false
                }
            )
            .Build();

        CytrusWatcher.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;

        LangsWatcher.CheckLangFinished += LangManager.OnCheckLangFinished;
    }

    public static async Task LaunchAsync()
    {
        DiscordActivity activity = new("Dofus Retro", DiscordActivityType.Playing);
        await Client.ConnectAsync(activity);
    }
}
