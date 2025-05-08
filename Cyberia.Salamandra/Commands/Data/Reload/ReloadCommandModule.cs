using Cyberia.Api;
using Cyberia.Api.Data;
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
    private readonly DofusApiConfig _dofusApiConfig;
    private readonly DofusDatacenter _dofusDatacenter;

    public ReloadCommandModule(DofusApiConfig dofusApiConfig, DofusDatacenter dofusDatacenter)
    {
        _dofusApiConfig = dofusApiConfig;
        _dofusDatacenter = dofusDatacenter;
    }

    [Command("reload"), Description("[Owner] Reload the API data")]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("type"), Description("The type of langs to load; if empty, use the value in the config")]
        LangType? type = null)
    {
        await ctx.DeferResponseAsync();

        _dofusDatacenter.Load(type ?? _dofusApiConfig.Type);

        await ctx.EditResponseAsync("Api reload !");
    }
}
