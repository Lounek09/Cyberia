using Cyberia.Api;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace Cyberia.Salamandra.Commands.Dofus
{
#pragma warning disable CA1822 // Mark members as static
    public sealed class EscapeCommandModule : ApplicationCommandModule
    {
        [SlashCommand("fuite", "Permet de calculer son % de fuite")]
        public async Task Command(InteractionContext ctx,
            [Option("agilite", "Votre agilité")]
            [Minimum(0), Maximum(99999)]
            long agility,
            [Option("agilite_ennemi", "Agilité de l'ennemi à votre contact")]
            [Minimum(0), Maximum(99999)]
            long foeAgility)
        {
            double escapePercent = Formulas.GetEscapePercent((int)agility, (int)foeAgility);
            int agilityToEscapeForSure = Formulas.GetAgilityToEscapeForSure((int)foeAgility);

            DiscordEmbedBuilder embed = EmbedManager.BuildDofusEmbed(DofusEmbedCategory.Tools, "Calculateur de % de fuite")
                .WithDescription($"""
                                  Avec {Formatter.Bold(agility.ToString())}agi tu auras {Formatter.Bold(escapePercent.ToString())}% de chance de fuir contre {Formatter.Bold(foeAgility.ToString())}agi
                                  Pour fuir à 100% il te faudra au minimum {Formatter.Bold(agilityToEscapeForSure.ToString())}agi !
                                  """);

            await ctx.CreateResponseAsync(embed);
        }
    }
}
