using Cyberia.Api;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus;

public sealed class CritCommandModule : ApplicationCommandModule
{
    [SlashCommand("crit", "Permet de calculer votre taux de crit")]
    public async Task Command(InteractionContext ctx,
        [Option("nombre", "Nombre de crit")]
        [Minimum(1), Maximum(999)]
        long number,
        [Option("taux", "Taux de crit cible")]
        [Minimum(1), Maximum(999)]
        long target,
        [Option("agilite", "Votre agilité")]
        [Minimum(1), Maximum(99999)]
        long agility)
    {
        var rate = Formulas.GetCriticalRate((int)number, (int)target, (int)agility);
        var agilityNeeded = Formulas.GetAgilityForHalfCriticalRate((int)number, (int)target);

        if (rate > -1)
        {
            var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Calculateur de coups critiques")
                .WithDescription($"Tu seras {Formatter.Bold($"1/{rate}")} au {Formatter.Bold($"1/{target}")} avec {Formatter.Bold(number.ToString())}crit et {Formatter.Bold(agility.ToString())}agi");

            if (rate != 2 && agilityNeeded > -1)
            {
                embed.Description += $"\nPour atteindre le 1/2 il te faudra au minimum {Formatter.Bold(agilityNeeded.ToString())}agi !";
            }

            await ctx.CreateResponseAsync(embed);
        }
        else
        {
            await ctx.CreateResponseAsync("Paramètre incorrect");
        }
    }
}
