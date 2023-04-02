global using Cyberia.Utils;
using Cyberia.Api;
using Cyberia.Chronicle;
using Cyberia.Cytrus;
using Cyberia.Langs;
using Cyberia.Langs.Enums;
using Cyberia.Managers;
using Cyberia.Salamandra;
using Cyberia.Scripts;

namespace Cyberia
{
    public static class Cyberia
    {
        public static Logger Logger { get; private set; }
        public static Config Config { get; private set; }
        public static AnkamaCytrus Cytrus { get; private set; }
        public static DofusLangs Langs { get; private set; }
        public static DofusApi Api { get; private set; }
        public static Bot Salamandra { get; private set; }

        static Cyberia()
        {
            Logger = new("main");
            Config = Config.Build();
            Cytrus = AnkamaCytrus.Build();
            Cytrus.NewCytrusDetected += CytrusManager.OnNewCytrusDetected;
            Langs = DofusLangs.Build();
            Langs.CheckLangFinished += LangsManager.OnCheckLangFinished;
            Api = DofusApi.Build();
            Salamandra = Bot.Build(Cytrus, Langs, Api);
        }

        public static async Task Main()
        {
            await Salamandra.Launch();

            if (Config.EnableCheckCytrus)
                Cytrus.Listen(10000, 60000);

            if (Config.EnableCheckLang)
                Langs.ListenAll(LangType.Official, 20000, 360000);

            if (Config.EnableCheckBetaLang)
                Langs.ListenAll(LangType.Beta, 140000, 360000);

            if (Config.EnableCheckTemporisLang)
                Langs.ListenAll(LangType.Temporis, 260000, 360000);

            await DatabaseBuilder.Launch(LangType.Official, Language.FR);

            await Task.Delay(-1);
        }
    }
}