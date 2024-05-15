using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.UserCommands;
using DSharpPlus.Entities;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

        Client = new(new DiscordConfiguration()
        {
            Token = Config.Token,
            LoggerFactory = new LoggerFactory().AddSerilog(Log.Logger),
            LogUnknownAuditlogs = false,
            LogUnknownEvents = false,
            Intents = DiscordIntents.Guilds | DiscordIntents.GuildMessages
        });
        Client.GuildDownloadCompleted += ClientManager.OnGuildDownloadCompleted;
        Client.GuildCreated += GuildManager.OnGuildCreated;
        Client.GuildDeleted += GuildManager.OnGuildDeleted;
        Client.MessageCreated += MessageManager.OnMessageCreated;
        Client.ComponentInteractionCreated += InteractionManager.OnComponentInteractionCreated;

        Commands = Client.UseCommands(new CommandsConfiguration()
        {
            ServiceProvider = new ServiceCollection()
                .AddLogging(x => x.AddSerilog(Log.Logger))
                .BuildServiceProvider(),
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
        var activity = new DiscordActivity("Dofus Retro", DiscordActivityType.Playing);
        await Client.ConnectAsync(activity);
    }
}
