using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin;

public sealed class RestartCommandModule
{
    [Command("restart"), Description("[Owner] Restart the bot")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.RespondAsync(new DiscordInteractionResponseBuilder()
            .WithContent("🔃")
            .AsEphemeral());

        await ctx.Client.DisconnectAsync();

        await ctx.Client.ConnectAsync(new DiscordActivity("Dofus Retro", DiscordActivityType.Playing));
    }
}
