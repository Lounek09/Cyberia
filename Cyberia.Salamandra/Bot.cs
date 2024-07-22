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
    public static CommandsExtension Commands { get; private set; } = default!;

    public static async Task InitializeAsync(BotConfig config)
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
                config.GatewayCompressionLevel = GatewayCompressionLevel.Stream;
                config.LogUnknownAuditlogs = false;
                config.LogUnknownEvents = false;
            })
            .Build();

        Commands = Client.UseCommands(new CommandsConfiguration()
        {
            UseDefaultCommandErrorHandler = false,
            RegisterDefaultCommandProcessors = false
        });
        Commands.CommandErrored += CommandManager.OnCommandErrored;
        await Commands.AddProcessorAsync(new SlashCommandProcessor());
        await Commands.AddProcessorAsync(new UserCommandProcessor());
        Commands.RegisterCommands(Config.AdminGuildId);

        CytrusWatcher.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;

        LangsWatcher.CheckLangFinished += LangManager.OnCheckLangFinished;
    }

    public static async Task LaunchAsync()
    {
        DiscordActivity activity = new("Dofus Retro", DiscordActivityType.Playing);
        await Client.ConnectAsync(activity);
    }
}
