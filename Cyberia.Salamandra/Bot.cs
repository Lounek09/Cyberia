using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

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
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            LogTimestampFormat = "yyyy/MM/dd HH:mm:ss:ffff"
        });
        Client.GuildCreated += GuildManager.OnGuildCreated;
        Client.GuildDeleted += GuildManager.OnGuildDeleted;
        Client.MessageCreated += MessageManager.OnMessageCreated;
        Client.ComponentInteractionCreated += InteractionManager.OnComponentInteractionCreated;

        SlashCommands = Client.UseSlashCommands();
        SlashCommands.SlashCommandErrored += CommandManager.OnSlashCommandErrored;

        CytrusWatcher.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;

        LangsWatcher.CheckLangFinished += LangsManager.OnCheckLangFinished;
    }

    public static async Task Launch()
    {
        CommandManager.RegisterCommands();

        DiscordActivity activity = new("Dofus Retro", ActivityType.Playing);
        await Client.ConnectAsync(activity);
    }
}
