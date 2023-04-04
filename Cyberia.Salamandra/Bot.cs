global using Cyberia.Utils;
using Cyberia.Api;
using Cyberia.Chronicle;
using Cyberia.Cytrusaur;
using Cyberia.Langzilla;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Microsoft.Extensions.Logging;

namespace Cyberia.Salamandra
{
    public class Bot
    {
        public Logger Logger { get; init; }
        public BotConfig Config { get; init; }
        public DiscordClient Client { get; init; }
        public SlashCommandsExtension SlashCommands { get; init; }

        internal AnkamaCytrus Cytrus { get; init; }
        internal DofusLangs Langs { get; init; }
        internal DofusApi Api { get; init; }

        internal static Bot Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Bot before !") : _instance;
        }
        private static Bot? _instance;

        internal Bot(AnkamaCytrus cytrus, DofusLangs langs, DofusApi api)
        {
            Logger = new("bot");
            Config = BotConfig.Build();

            DiscordConfiguration discordConfig = new()
            {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
#if DEBUG
                MinimumLogLevel = LogLevel.Debug,
#endif
                LogTimestampFormat = "yyyy/MM/dd HH:mm:ss:ffff"
            };
            Client = new DiscordClient(discordConfig);
            Client.GuildCreated += GuildManager.OnGuildCreated;
            Client.GuildDeleted += GuildManager.OnGuildDeleted;

            SlashCommands = Client.UseSlashCommands();
            SlashCommands.SlashCommandErrored += CommandManager.OnSlashCommandErrored;
            SlashCommands.SlashCommandExecuted += CommandManager.OnSlashCommandExecuted;

            Cytrus = cytrus;
            Cytrus.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;

            Langs = langs;
            Langs.CheckLangFinished += LangsManager.OnCheckLangFinished;

            Api = api;
        }

        public static Bot Build(AnkamaCytrus cytrus, DofusLangs langs, DofusApi api)
        {
            _instance ??= new(cytrus, langs, api);
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
