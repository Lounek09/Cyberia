﻿using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Admin.Kill;

public sealed class KillCommandModule
{
    [Command("kill"), Description("[Owner] Kill Salamandra")]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [RequireApplicationOwner]
    public static async Task ExecuteAsync(SlashCommandContext ctx)
    {
        await ctx.RespondAsync(@"\💀");

        await ctx.Client.DisconnectAsync();
    }
}
