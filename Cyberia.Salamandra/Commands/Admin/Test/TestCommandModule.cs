using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin.Test;

public sealed class TestCommandModule
{
    [Command("test"), Description("[Owner] Command to test random stuff")]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.RespondAsync("test");
    }
}
