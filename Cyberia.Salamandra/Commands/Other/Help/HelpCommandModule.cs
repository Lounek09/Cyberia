using Cyberia.Salamandra.Managers;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Other;

public sealed class HelpCommandModule
{
    [Command("help"), Description("Liste les commandes du bot")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        if (EmbedManager.HelpEmbed is null)
        {
            await ctx.RespondAsync(new DiscordInteractionResponseBuilder()
                .WithContent("Le bot est en cours de démarrage, veuillez réessayer dans quelques secondes.")
                .AsEphemeral());

            return;
        }

        await ctx.RespondAsync(EmbedManager.HelpEmbed);
    }
}
