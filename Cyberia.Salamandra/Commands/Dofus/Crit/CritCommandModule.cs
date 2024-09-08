using Cyberia.Api;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ArgumentModifiers;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.Metadata;
using DSharpPlus.Entities;

using System.ComponentModel;

namespace Cyberia.Salamandra.Commands.Dofus.Crit;

public sealed class CritCommandModule
{
    [Command("crit"), Description("Permet de calculer votre taux de crit")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("nombre"), Description("Nombre de crit")]
        [MinMaxValue(1, 999)]
        int number,
        [Parameter("taux"), Description("Taux de crit cible")]
        [MinMaxValue(1, 999)]
        int target,
        [Parameter("agilite") , Description("Votre agilité")]
        [MinMaxValue(1, 99999)]
        int agility)
    {
        CultureManager.SetCulture(ctx.Interaction);

        var rate = Formulas.GetCriticalRate(number, target, agility);
        var agilityNeeded = Formulas.GetAgilityForHalfCriticalRate(number, target);

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, BotTranslations.Embed_Crit_Author)
            .WithDescription(Translation.Format(
                BotTranslations.Embed_Crit_Description,
                Formatter.Bold($"1/{rate}"),
                Formatter.Bold($"1/{target}"),
                Formatter.Bold(number.ToString()),
                Formatter.Bold(agility.ToString()),
                Formatter.Bold(agilityNeeded.ToString())));

        await ctx.RespondAsync(embed);
    }
}
