using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class ReloadCommandModule
{
    [Command("reload"), Description("[Owner] Recharge les données de Salamandra")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.DeferResponseAsync();

        await DofusApi.ReloadAsync();

        await ctx.EditResponseAsync("Api reload !");
    }
}
