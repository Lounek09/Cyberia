using Cyberia.Amphibian;
using Cyberia.Api;
using Cyberia.Cytrusaurus;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra;

using Microsoft.Extensions.Configuration;

namespace Cyberia;

public static class Program
{
    public static async Task Main()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false);

        var appConfig = builder.Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(appConfig)
            .CreateLogger();

        try
        {
            Log.Information("Starting Cyberia");

            var config = appConfig.GetSection("Cyberia").Get<CyberiaConfig>()!;

            Log.Information("Initializing CytrusWatcher");
            CytrusWatcher.Initialize();

            Log.Information("Initializing LangsWatcher");
            LangsWatcher.Initialize();

            Log.Information("Initializing DofusApi");
            DofusApi.Initialize(config.ApiConfig);

            if (config.EnableSalamandra)
            {
                Log.Information("Initializing Salamandra");
                await Bot.InitializeAsync(config.BotConfig);

                await Bot.LaunchAsync();
            }

            if (config.EnableAmphibian)
            {
                Log.Information("Initializing Amphibian");
                Web.Initialize();

                _ = Web.LaunchAsync();
            }

            if (config.EnableCheckCytrus)
            {
                Log.Information("Watching Cytrus each {CytrusInterval}",
                    config.CheckCytrusInterval);
                CytrusWatcher.Watch(TimeSpan.FromSeconds(10), config.CheckCytrusInterval);
            }

            if (config.EnableCheckLang)
            {
                Log.Information("Watching {LangType} Langs each {OfficialLangInterval}", LangType.Official, config.CheckLangInterval);
                LangsWatcher.Watch(LangType.Official, TimeSpan.FromSeconds(20), config.CheckLangInterval);
            }

            if (config.EnableCheckBetaLang)
            {
                Log.Information("Watching {LangType} Langs each {BetaLangInterval}", LangType.Beta, config.CheckBetaLangInterval);
                LangsWatcher.Watch(LangType.Beta, TimeSpan.FromSeconds(140), config.CheckBetaLangInterval);
            }

            if (config.EnableCheckTemporisLang)
            {
                Log.Information("Watching {LangType} Langs each {TemporisLangInterval}", LangType.Temporis, config.CheckTemporisLangInterval);
                LangsWatcher.Watch(LangType.Temporis, TimeSpan.FromSeconds(260), config.CheckCytrusInterval);
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
