using Cyberia.Api;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class EscapeCommandModule
{
    [Command("fuite"), Description("Permet de calculer son % de fuite")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("agilite"), Description("Votre agilité")]
        [SlashMinMaxValue(MinValue = 1, MaxValue = 99999)]
        long agility,
        [Parameter("agilite_ennemi"), Description("Agilité de l'ennemi à votre contact")]
        [SlashMinMaxValue(MinValue = 1, MaxValue = 99999)]
        long foeAgility)
    {
        var escapePercent = Formulas.GetEscapePercent((int)agility, (int)foeAgility);
        var agilityToEscapeForSure = Formulas.GetAgilityToEscape((int)foeAgility);

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Calculateur de % de fuite")
            .WithDescription($"""
                Avec {Formatter.Bold(agility.ToString())}agi tu auras {Formatter.Bold(escapePercent.ToString())}% de chance de fuir contre {Formatter.Bold(foeAgility.ToString())}agi
                Pour fuir à 100% il te faudra au minimum {Formatter.Bold(agilityToEscapeForSure.ToString())}agi !
                """);

        await ctx.RespondAsync(embed);
    }
}
