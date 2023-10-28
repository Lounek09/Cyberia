using Cyberia.Api;
using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra;

using Microsoft.Extensions.Configuration;

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
                Log.Information("Starting Cyberia");

                CyberiaConfig config = appConfig.GetSection("Cyberia").Get<CyberiaConfig>()!;

                CytrusWatcher cytrus = CytrusWatcher.Create();

                LangsWatcher langs = LangsWatcher.Create();

                Log.Information("Initializing DofusApi");
                DofusApi.Initialize(config.ApiConfig, langs);

                Bot salamandra = Bot.Build(config.BotConfig, cytrus, langs);
                await salamandra.Launch();

                if (config.EnableCheckCytrus)
                {
                    Log.Information("Listening to Cytrus each {interval}", config.CheckCytrusInterval);
                    cytrus.Listen(TimeSpan.FromSeconds(10), config.CheckCytrusInterval);
                }

                if (config.EnableCheckLang)
                {
                    Log.Information("Listening to {type} Lang each {interval}", LangType.Official, config.CheckLangInterval);
                    langs.Listen(LangType.Official, TimeSpan.FromSeconds(20), config.CheckLangInterval);
                }

                if (config.EnableCheckBetaLang)
                {
                    Log.Information("Listening to {type} Lang each {interval}", LangType.Beta, config.CheckBetaLangInterval);
                    langs.Listen(LangType.Beta, TimeSpan.FromSeconds(140), config.CheckBetaLangInterval);
                }

                if (config.EnableCheckTemporisLang)
                {
                    Log.Information("Listening to {type} Lang each {interval}", LangType.Temporis, config.CheckTemporisLangInterval);
                    langs.Listen(LangType.Temporis, TimeSpan.FromSeconds(260), config.CheckCytrusInterval);
                }

                Log.Information("Cyberia started");
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
