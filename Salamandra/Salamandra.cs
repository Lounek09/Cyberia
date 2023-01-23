global using Salamandra.Utils;
using Salamandra.Api;
using Salamandra.Bot;
using Salamandra.Cytrus;
using Salamandra.Langs;
using Salamandra.Langs.Enums;
using Salamandra.Managers;

namespace Salamandra
{
    public static class Salamandra
    {
        public static Logger Logger { get; private set; }
        public static Config Config { get; private set; }
        public static AnkamaCytrus Cytrus { get; private set; }
        public static DofusLangs Langs { get; private set; }
        public static DofusApi Api { get; private set; }
        public static DiscordBot Bot { get; private set; }

        static Salamandra()
        {
            Logger = new("main");
            Config = Config.Build();
            Cytrus = AnkamaCytrus.Build();
            Cytrus.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;
            Langs = DofusLangs.Build();
            Langs.CheckLangFinished += LangsManager.OnCheckLangFinished;
            Api = DofusApi.Build();
            Bot = DiscordBot.Build(Cytrus, Langs, Api);
        }

        public static async Task Main()
        {
            await Bot.Launch();

            if (Config.EnableCheckCytrus)
                Cytrus.Listen(10000, 60000);

            if (Config.EnableCheckLang)
                Langs.ListenAll(LangType.Official, 20000, 360000);

            if (Config.EnableCheckBetaLang)
                Langs.ListenAll(LangType.Beta, 140000, 360000);

            if (Config.EnableCheckTemporisLang)
                Langs.ListenAll(LangType.Temporis, 260000, 360000);

            await Task.Delay(-1);
        }
    }
}