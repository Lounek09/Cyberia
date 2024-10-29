using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin.Restart;

public sealed class RestartCommandModule
{
    [Command("restart"), Description("[Owner] Restart the bot")]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequirePermissions(DiscordPermissions.UseApplicationCommands)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.RespondAsync("🔃", true);

        await ctx.Client.DisconnectAsync();

        await ctx.Client.ConnectAsync(new DiscordActivity("Dofus Retro", DiscordActivityType.Playing));
    }
}
