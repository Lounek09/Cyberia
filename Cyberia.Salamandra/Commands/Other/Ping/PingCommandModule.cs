using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Other.Ping;

public sealed class PingCommandModule
{
    [Command(PingInteractionLocalizer.CommandName), Description(PingInteractionLocalizer.CommandDescription)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<PingInteractionLocalizer>]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        var latency = ctx.Client.GetConnectionLatency(ctx.Channel.Id);

        await ctx.RespondAsync($"Pong... {latency.TotalMilliseconds}ms !", true);
    }
}
