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

                if (config.EnableCheckCytrus && config.CheckCytrusInterval > 0)
                    cytrus.Watch(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(config.CheckCytrusInterval));

                if (config.EnableCheckLang && config.CheckLangInterval > 0)
                    langs.WatchAll(LangType.Official, TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(config.CheckLangInterval));

                if (config.EnableCheckBetaLang && config.CheckBetaLangInterval > 0)
                    langs.WatchAll(LangType.Beta, TimeSpan.FromSeconds(140), TimeSpan.FromMinutes(config.CheckBetaLangInterval));

                if (config.EnableCheckTemporisLang && config.CheckCytrusInterval > 0)
                    langs.WatchAll(LangType.Temporis, TimeSpan.FromSeconds(260), TimeSpan.FromMinutes(config.CheckCytrusInterval));

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
