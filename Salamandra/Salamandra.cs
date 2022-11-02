using Salamandra.Bot;
using Salamandra.Langs;
using Salamandra.Manager;
using Salamandra.Script;
using Salamandra.Utils;

namespace Salamandra
{
    public static class Salamandra
    {
        public static Logger Logger { get; private set; }
        public static Config Config { get; private set; }
        public static DofusLangs Langs { get; private set; }
        public static DiscordBot Bot { get; private set; }

        static Salamandra()
        {
            Logger = new();
            Config = Config.Build();
            Langs = DofusLangs.Build(Logger);
            Bot = DiscordBot.Build(Logger, Langs);

            if (Directory.Exists(Constant.TEMP_PATH))
                Directory.CreateDirectory(Constant.TEMP_PATH);
        }

        public static async Task Main()
        {
            await Bot.Launch();

            LangsManager.Listen();
            CytrusManager.Listen();

            await Task.Delay(-1);
        }
    }
}