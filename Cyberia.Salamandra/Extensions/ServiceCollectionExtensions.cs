using Cyberia.Salamandra.EventHandlers;
using Cyberia.Salamandra.Managers;

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
                eventHandler.HandleGuildDownloadCompleted(ClientManager.OnGuildDownloadCompleted);
                eventHandler.HandleGuildCreated(GuildManager.OnGuildCreated);
                eventHandler.HandleGuildDeleted(GuildManager.OnGuildDeleted);
                eventHandler.AddEventHandlers<InteractionsEventHandler>();
            })
            .Configure<DiscordConfiguration>(config =>
            {
                config.LogUnknownAuditlogs = false;
                config.LogUnknownEvents = false;
            })
            .AddCommandsExtension(
                setup =>
                {
                    setup.CommandErrored += CommandManager.OnCommandErrored;
                    setup.AddProcessor(new SlashCommandProcessor());
                    setup.AddProcessor(new UserCommandProcessor());
                    setup.RegisterCommands(config.AdminGuildId);
                },
                new CommandsConfiguration()
                {
                    UseDefaultCommandErrorHandler = false,
                    RegisterDefaultCommandProcessors = false
                }
            );

        services.AddSingleton<CultureService>();
        services.AddSingleton<CytrusService>();
        services.AddSingleton<LangsService>();

        return services;
    }
}
