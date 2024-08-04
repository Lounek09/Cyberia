using Cyberia.Api;
using Cyberia.Langzilla.Enums;

using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Data.Cytrus;

public sealed class ReloadCommandModule
{
    [Command("reload"), Description("[Owner] Reload the API data")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type of langs to load; if empty, use the value in the config")]
        LangType? type = null)
    {
        await ctx.DeferResponseAsync();

        DofusApi.Reload(type ?? DofusApi.Config.Type);

        await ctx.EditResponseAsync("Api reload !");
    }
}
