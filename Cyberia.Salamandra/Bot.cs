global using Cyberia.Utils;

using Cyberia.Api;
using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Serilog;

namespace Cyberia.Salamandra
{
    public sealed class Bot
    {
        public const string OUTPUT_PATH = "bot";

        public ILogger Log { get; init; }
        public BotConfig Config { get; init; }
        public DiscordClient Client { get; init; }
        public SlashCommandsExtension SlashCommands { get; init; }

        internal CytrusWatcher CytrusWatcher { get; init; }
        internal LangsWatcher LangsWatcher { get; init; }
        internal DofusApi Api { get; init; }


        internal static Bot Instance
        {
            get => _instance is null ? throw new NullReferenceException("Build the Bot before !") : _instance;
        }
        private static Bot? _instance;

        internal Bot(ILogger logger, BotConfig config, CytrusWatcher cytrus, LangsWatcher langs)
        {
            Directory.CreateDirectory(OUTPUT_PATH);

            Log = logger;
            Config = config;

            Client = new(new DiscordConfiguration()
            {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
#if DEBUG
                MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug,
#endif
                LogTimestampFormat = "yyyy/MM/dd HH:mm:ss:ffff"
            });
            Client.GuildCreated += GuildManager.OnGuildCreated;
            Client.GuildDeleted += GuildManager.OnGuildDeleted;
            Client.MessageCreated += MessageManager.OnMessageCreated;
            Client.ComponentInteractionCreated += InteractionManager.OnComponentInteractionCreated;

            SlashCommands = Client.UseSlashCommands();
            SlashCommands.SlashCommandErrored += CommandManager.OnSlashCommandErrored;
            SlashCommands.SlashCommandExecuted += CommandManager.OnSlashCommandExecuted;

            CytrusWatcher = cytrus;
            CytrusWatcher.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;

            LangsWatcher = langs;
            LangsWatcher.CheckLangFinished += LangsManager.OnCheckLangFinished;

            Api = DofusApi.Build(logger, config.ApiConfig, langs);
        }

        public static Bot Build(ILogger logger, BotConfig config, CytrusWatcher cytrus, LangsWatcher langs)
        {
            _instance ??= new(logger, config, cytrus, langs);
            return _instance;
        }

        public async Task Launch()
        {
            CommandManager.RegisterCommands();

            DiscordActivity activity = new("Dofus Retro", ActivityType.Playing);
            await Client.ConnectAsync(activity);
        }
    }
}
