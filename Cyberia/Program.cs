﻿using Cyberia.Api;
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
            .SetBasePath(Directory.GetCurrentDirectory())
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

            Log.Information("Initializing Salamandra");
            Bot.Initialize(config.BotConfig);

            await Bot.Launch();

            if (config.EnableCheckCytrus)
            {
                Log.Information("Waching Cytrus each {interval}", config.CheckCytrusInterval);
                CytrusWatcher.Watch(TimeSpan.FromSeconds(10), config.CheckCytrusInterval);
            }

            if (config.EnableCheckLang)
            {
                Log.Information("Waching {LangType} Langs each {Interval}", LangType.Official, config.CheckLangInterval);
                LangsWatcher.Watch(LangType.Official, TimeSpan.FromSeconds(20), config.CheckLangInterval);
            }

            if (config.EnableCheckBetaLang)
            {
                Log.Information("Waching {LangType} Langs each {Interval}", LangType.Beta, config.CheckBetaLangInterval);
                LangsWatcher.Watch(LangType.Beta, TimeSpan.FromSeconds(140), config.CheckBetaLangInterval);
            }

            if (config.EnableCheckTemporisLang)
            {
                Log.Information("Waching {LangType} Langs each {Interval}", LangType.Temporis, config.CheckTemporisLangInterval);
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