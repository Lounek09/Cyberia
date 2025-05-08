using Cyberia.Salamandra.EventHandlers;
using Cyberia.Salamandra.Extensions.DSharpPlus;
using Cyberia.Salamandra.Services;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.InteractionNamingPolicies;
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
                eventHandler.AddEventHandlers<CommandsEventHandler>(ServiceLifetime.Singleton);
                eventHandler.AddEventHandlers<GuildsEventHandler>(ServiceLifetime.Singleton);
                eventHandler.AddEventHandlers<InteractionsEventHandler>(ServiceLifetime.Singleton);
            })
            .Configure<DiscordConfiguration>(discordConfig =>
            {
                discordConfig.LogUnknownAuditlogs = false;
                discordConfig.LogUnknownEvents = false;
            })
            .AddCommandsExtension
            (
                (provider, extention) =>
                {
                    extention.AddProcessor(new SlashCommandProcessor(new SlashCommandConfiguration()
                    {
#if DEBUG
                        UnconditionallyOverwriteCommands = true,
#endif
                        NamingPolicy = new SnakeCaseNamingFixer()
                    }));
                    extention.RegisterCommands(config.AdminGuildId);

                    //TODO: Remove this when the extension supports the IEventHandler interface.
                    extention.CommandErrored += provider.GetRequiredService<CommandsEventHandler>().HandleEventAsync;
                },
                new CommandsConfiguration()
                {
                    UseDefaultCommandErrorHandler = false,
                    RegisterDefaultCommandProcessors = false
                }
            );

        services.AddSingleton<ICachedChannelsService, CachedChannelsService>();
        services.AddSingleton<ICultureService, CultureService>();
        services.AddSingleton<ICytrusService, CytrusService>();
        services.AddSingleton<IEmojisService, EmojisService>();
        services.AddSingleton<ILangsService, LangsService>();
        services.AddSingleton<IEmbedBuilderService, EmbedBuilderService>();

        return services;
    }
}
