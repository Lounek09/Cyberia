using Cyberia.Api;

using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Localization;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Other.Discord;

public sealed class DiscordCommandModule
{
    private readonly DofusApiConfig _dofusApiConfig;

    public DiscordCommandModule(DofusApiConfig dofusApiConfig)
    {
        _dofusApiConfig = dofusApiConfig;
    }

    [Command(DiscordInteractionLocalizer.CommandName), Description(DiscordInteractionLocalizer.CommandDescription)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    [InteractionLocalizer<DiscordInteractionLocalizer>]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public async Task ExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.RespondAsync(_dofusApiConfig.DiscordInviteUrl, true);
    }
}
