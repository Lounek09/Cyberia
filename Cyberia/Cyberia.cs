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
                    cytrus.Watch(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1));

                if (config.EnableCheckLang)
                    langs.WatchAll(LangType.Official, TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(5));

                if (config.EnableCheckBetaLang)
                    langs.WatchAll(LangType.Beta, TimeSpan.FromSeconds(140), TimeSpan.FromMinutes(5));

                if (config.EnableCheckTemporisLang)
                    langs.WatchAll(LangType.Temporis, TimeSpan.FromSeconds(260), TimeSpan.FromMinutes(5));

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
