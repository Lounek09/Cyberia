using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra;

using Microsoft.Extensions.Configuration;

using Serilog;

namespace Cyberia
{
    public static class Cyberia
    {
        public static async Task Main()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false);

            IConfigurationRoot appConfig = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(appConfig)
                .CreateLogger();

            try
            {
                CyberiaConfig config = appConfig.GetSection("Cyberia").Get<CyberiaConfig>()!;

                Directory.CreateDirectory("temp");

                CytrusWatcher cytrus = CytrusWatcher.Create(Log.Logger);

                LangsWatcher langs = LangsWatcher.Create(Log.Logger);

                Bot salamandra = Bot.Build(Log.Logger, config.BotConfig, cytrus, langs);
                await salamandra.Launch();

                if (config.EnableCheckCytrus)
                    cytrus.Watch(10000, 60000);

                if (config.EnableCheckLang)
                    langs.WatchAll(LangType.Official, 20000, 360000);

                if (config.EnableCheckBetaLang)
                    langs.WatchAll(LangType.Beta, 140000, 360000);

                if (config.EnableCheckTemporisLang)
                    langs.WatchAll(LangType.Temporis, 260000, 360000);

                await Task.Delay(-1);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Fatal error");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
