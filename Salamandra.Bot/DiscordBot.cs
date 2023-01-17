global using Salamandra.Utils;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

using Microsoft.Extensions.Logging;

using Salamandra.Api;
using Salamandra.Bot.Managers;
using Salamandra.Cytrus;
using Salamandra.Langs;

namespace Salamandra.Bot
{
    public class DiscordBot
    {
        public Logger Logger { get; private set; }
        public BotConfig Config { get; private set; }
        public DiscordClient Client { get; private set; }
        public SlashCommandsExtension SlashCommands { get; private set; }

        internal AnkamaCytrus Cytrus { get; private set; }
        internal DofusLangs Langs { get; private set; }
        internal DofusApi Api { get; private set; }

        internal static DiscordBot Instance {
            get => _instance is null ? throw new NullReferenceException("Build the Bot before !") : _instance;
        }
        private static DiscordBot? _instance;

        internal DiscordBot(AnkamaCytrus cytrus, DofusLangs langs, DofusApi api)
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
            Langs = langs;
            Api = api;
        }

        public static DiscordBot Build(AnkamaCytrus cytrus, DofusLangs langs, DofusApi api)
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
