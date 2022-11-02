using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Salamandra.Bot.Manager;
using Salamandra.Langs;

namespace Salamandra.Bot
{
    public class DiscordBot
    {
        public Logger Logger { get; private set; }
        public BotConfig Config { get; private set; }
        public DiscordClient Client { get; private set; }
        public SlashCommandsExtension SlashCommands { get; private set; }

        internal DofusLangs Langs { get; private set; }

        internal static DiscordBot Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Bot before !") : _instance;
            private set => _instance = value;
        }
        private static DiscordBot? _instance;

        internal DiscordBot(Logger logger, DofusLangs langs)
        {
            Logger = logger;

            if (!File.Exists(Constant.CONFIG_PATH))
            {
                Logger.Crit($"Fichier de config introuvable à l'emplacement : '{Constant.CONFIG_PATH}'");
                Console.ReadLine();
                Environment.Exit(0);
            }
            Config = Json.Load<BotConfig>(Constant.CONFIG_PATH);

            DiscordConfiguration discordConfig = new()
            {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
#if DEBUG
                MinimumLogLevel = LogLevel.Debug,
#endif
                LogTimestampFormat = "dd/MM/yyyy HH:mm:ss:ffff"
            };
            Client = new DiscordClient(discordConfig);
            Client.GuildCreated += GuildManager.OnGuildCreated;
            Client.GuildDeleted += GuildManager.OnGuildDeleted;

            //SlashCommands
            SlashCommands = Client.UseSlashCommands();
            SlashCommands.SlashCommandErrored += CommandManager.OnSlashCommandErrored;
            SlashCommands.SlashCommandExecuted += CommandManager.OnSlashCommandExecuted;

            Langs = langs;
        }

        public static DiscordBot Build(Logger logger, DofusLangs langs)
        {
            _instance = new(logger, langs);
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
