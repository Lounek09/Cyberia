using Cyberia.Amphibian.Extensions;
using Cyberia.Api;
using Cyberia.Cytrusaurus;
using Cyberia.Cytrusaurus.Extensions;
using Cyberia.Database.Extentsions;
using Cyberia.Langzilla;
using Cyberia.Langzilla.Enums;
using Cyberia.Langzilla.Extensions;
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
            .Enrich.FromLogContext()
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

            services.AddLogging(x => x.ClearProviders().AddSerilog());

            services.AddDatabase(connectionString);
            services.AddCytrusaurus();
            services.AddLangzilla();
            DofusApi.Initialize(cyberiaConfig.ApiConfig); //TODO: Use DI
            services.AddSalamandra(cyberiaConfig.BotConfig);
            // Add this last to ensure all dependencies are registered in its internal service collection.
            // Since it's a WebApplication, it will create a new service collection internally.
            services.AddAmphibian(cyberiaConfig.WebConfig);

            var provider = services.BuildServiceProvider();

            await provider.CreateTablesAsync();

            if (cyberiaConfig.EnableSalamandra)
            {
                await provider.StartSalamandraAsync();
            }

            if (cyberiaConfig.EnableAmphibian)
            {
                provider.StartAmphibian();
            }

            if (cyberiaConfig.EnableCheckCytrus)
            {
                Log.Information("Watching Cytrus each {CytrusInterval}", cyberiaConfig.CheckCytrusInterval);
                provider.GetRequiredService<CytrusWatcher>().Watch(TimeSpan.FromSeconds(20), cyberiaConfig.CheckCytrusInterval);
            }

            if (cyberiaConfig.EnableCheckLang)
            {
                Log.Information("Watching {LangType} Langs each {OfficialLangInterval}", LangType.Official, cyberiaConfig.CheckLangInterval);
                provider.GetRequiredService<LangsWatcher>().Watch(LangType.Official, TimeSpan.FromSeconds(20), cyberiaConfig.CheckLangInterval);
            }

            if (cyberiaConfig.EnableCheckBetaLang)
            {
                Log.Information("Watching {LangType} Langs each {BetaLangInterval}", LangType.Beta, cyberiaConfig.CheckBetaLangInterval);
                provider.GetRequiredService<LangsWatcher>().Watch(LangType.Beta, TimeSpan.FromSeconds(140), cyberiaConfig.CheckBetaLangInterval);
            }

            if (cyberiaConfig.EnableCheckTemporisLang)
            {
                Log.Information("Watching {LangType} Langs each {TemporisLangInterval}", LangType.Temporis, cyberiaConfig.CheckTemporisLangInterval);
                provider.GetRequiredService<LangsWatcher>().Watch(LangType.Temporis, TimeSpan.FromSeconds(260), cyberiaConfig.CheckCytrusInterval);
            }

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
