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

public sealed class CritCommandModule
{
    [Command("crit"), Description("Permet de calculer votre taux de crit")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nombre"), Description("Nombre de crit")]
        [SlashMinMaxValue(MinValue = 1, MaxValue = 999)]
        long number,
        [Parameter("taux"), Description("Taux de crit cible")]
        [SlashMinMaxValue(MinValue = 1, MaxValue = 999)]
        long target,
        [Parameter("agilite") , Description("Votre agilité")]
        [SlashMinMaxValue(MinValue = 1, MaxValue = 99999)]
        long agility)
    {
        var rate = Formulas.GetCriticalRate((int)number, (int)target, (int)agility);
        if (rate < 0)
        {
            await ctx.RespondAsync("Paramètre incorrect");
            return;
        }

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Calculateur de coups critiques")
            .WithDescription($"Tu seras {Formatter.Bold($"1/{rate}")} au {Formatter.Bold($"1/{target}")} avec {Formatter.Bold(number.ToString())}crit et {Formatter.Bold(agility.ToString())}agi");

        var agilityNeeded = Formulas.GetAgilityForHalfCriticalRate((int)number, (int)target);
        if (rate != 2 && agilityNeeded > -1)
        {
            embed.Description += $"\nPour atteindre le 1/2 il te faudra au minimum {Formatter.Bold(agilityNeeded.ToString())}agi !";
        }

        await ctx.RespondAsync(embed);
    }
}
