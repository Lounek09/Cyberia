using Cyberia.Salamandra.EventHandlers;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.UserCommands;
using DSharpPlus.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace Cyberia.Salamandra.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Salamandra dependencies to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The bot configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddSalamandra(this IServiceCollection services, BotConfig config)
    {
        Directory.CreateDirectory(Constant.OutputPath);

        services.AddSingleton(config);

        services.AddDiscordClient(config.Token, DiscordIntents.Guilds)
            .ConfigureEventHandlers(eventHandler =>
            {
                eventHandler.AddEventHandlers<ClientEventHandler>(ServiceLifetime.Singleton);
                eventHandler.AddEventHandlers<CommandsEventHandler>();
                eventHandler.AddEventHandlers<GuildsEventHandler>();
                eventHandler.AddEventHandlers<InteractionsEventHandler>();
            })
            .Configure<DiscordConfiguration>(config =>
            {
                config.LogUnknownAuditlogs = false;
                config.LogUnknownEvents = false;
            })
            .AddCommandsExtension
            (
                (provider, setup) =>
                {
                    setup.AddProcessor(new SlashCommandProcessor());
                    setup.AddProcessor(new UserCommandProcessor());
                    setup.RegisterCommands(config.AdminGuildId);

                    //TODO: Remove this when the extension supports the IEventHandler interface.
                    setup.CommandErrored += provider.GetRequiredService<CommandsEventHandler>().HandleEventAsync;
                },
                new CommandsConfiguration()
                {
                    UseDefaultCommandErrorHandler = false,
                    RegisterDefaultCommandProcessors = false
                }
            );

        services.AddTransient<CultureService>();

        services.AddSingleton<CachedChannelsService>();
        services.AddSingleton<CytrusService>();
        services.AddSingleton<LangsService>();
        services.AddSingleton<EmbedBuilderService>();

        return services;
    }
}
