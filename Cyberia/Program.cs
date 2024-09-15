using Cyberia.Amphibian;
using Cyberia.Api;
using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.Extensions;
using Cyberia.Database.Extentsions;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Salamandra.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Globalization;

namespace Cyberia;

public static class Program
{
    public static async Task Main()
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        try
        {
            Log.Information("Starting Cyberia");

            var connectionString = config.GetConnectionString("Cyberia");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Invalid connection string");
            }

            var cyberiaConfig = config.GetSection("Cyberia").Get<CyberiaConfig>();
            if (cyberiaConfig is null || !cyberiaConfig.Validate())
            {
                throw new InvalidOperationException("Invalid configuration");
            }

            var defaultCulture = cyberiaConfig.ApiConfig.SupportedLanguages[0].ToCulture();
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

            var services = new ServiceCollection();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(Log.Logger);
            });

            services.AddDatabase(connectionString);
            services.AddCytrusaurus();
            LangsWatcher.Initialize();
            DofusApi.Initialize(cyberiaConfig.ApiConfig);
            services.AddSalamandra(cyberiaConfig.BotConfig);
            Web.Initialize(cyberiaConfig.WebConfig);

            var provider = services.BuildServiceProvider();

            await provider.CreateTablesAsync();

            if (cyberiaConfig.EnableSalamandra)
            {
                await provider.StartSalamandraAsync();
            }

            if (cyberiaConfig.EnableAmphibian)
            {
                _ = Web.LaunchAsync();
            }

            if (cyberiaConfig.EnableCheckCytrus)
            {
                Log.Information("Watching Cytrus each {CytrusInterval}", cyberiaConfig.CheckCytrusInterval);
                provider.GetRequiredService<CytrusWatcher>().Watch(TimeSpan.FromSeconds(20), cyberiaConfig.CheckCytrusInterval);
            }

            if (cyberiaConfig.EnableCheckLang)
            {
                Log.Information("Watching {LangType} Langs each {OfficialLangInterval}", LangType.Official, cyberiaConfig.CheckLangInterval);
                LangsWatcher.Watch(LangType.Official, TimeSpan.FromSeconds(20), cyberiaConfig.CheckLangInterval);
            }

            if (cyberiaConfig.EnableCheckBetaLang)
            {
                Log.Information("Watching {LangType} Langs each {BetaLangInterval}", LangType.Beta, cyberiaConfig.CheckBetaLangInterval);
                LangsWatcher.Watch(LangType.Beta, TimeSpan.FromSeconds(140), cyberiaConfig.CheckBetaLangInterval);
            }

            if (cyberiaConfig.EnableCheckTemporisLang)
            {
                Log.Information("Watching {LangType} Langs each {TemporisLangInterval}", LangType.Temporis, cyberiaConfig.CheckTemporisLangInterval);
                LangsWatcher.Watch(LangType.Temporis, TimeSpan.FromSeconds(260), cyberiaConfig.CheckCytrusInterval);
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
