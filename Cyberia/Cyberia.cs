global using Cyberia.Utils;
using Cyberia.Api;
using Cyberia.Chronicle;
using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra;

namespace Cyberia
{
    public static class Cyberia
    {
        public static Logger Logger { get; private set; }
        public static Config Config { get; private set; }
        public static AnkamaCytrus AnkamaCytrus { get; private set; }
        public static LangsWatcher LangsWatcher { get; private set; }
        public static DofusApi Api { get; private set; }
        public static Bot Salamandra { get; private set; }

        static Cyberia()
        {
            Logger = new("main");
            Config = Config.Load();
            AnkamaCytrus = AnkamaCytrus.Build();
            LangsWatcher = LangsWatcher.Create();
            Api = DofusApi.Build(Config.CdnUrl, Config.Temporis, LangsWatcher, FormatType.MarkDown);
            Salamandra = Bot.Build(AnkamaCytrus, LangsWatcher, Api);

            Directory.CreateDirectory("temp");
        }

        public static async Task Main()
        {
            await Salamandra.Launch();

            if (Config.EnableCheckCytrus)
                AnkamaCytrus.Listen(10000, 60000);

            if (Config.EnableCheckLang)
                LangsWatcher.WatchAll(LangType.Official, 20000, 360000);

            if (Config.EnableCheckBetaLang)
                LangsWatcher.WatchAll(LangType.Beta, 140000, 360000);

            if (Config.EnableCheckTemporisLang)
                LangsWatcher.WatchAll(LangType.Temporis, 260000, 360000);

            await Task.Delay(-1);
        }
    }
}