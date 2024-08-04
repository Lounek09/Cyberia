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

namespace Cyberia.Salamandra.Commands.Dofus.Escape;

public sealed class EscapeCommandModule
{
    [Command("fuite"), Description("Permet de calculer son % de fuite")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    [InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall, DiscordApplicationIntegrationType.UserInstall)]
    [InteractionAllowedContexts(DiscordInteractionContextType.Guild, DiscordInteractionContextType.PrivateChannel)]
    public static async Task ExecuteAsync(SlashCommandContext ctx,
        [Parameter("agilite"), Description("Votre agilité")]
        [MinMaxValue(1, 99999)]
        int agility,
        [Parameter("agilite_ennemi"), Description("Agilité de l'ennemi à votre contact")]
        [MinMaxValue(1, 99999)]
        int foeAgility)
    {
        var escapePercent = Formulas.GetEscapePercent(agility, foeAgility);
        var agilityToEscapeForSure = Formulas.GetAgilityToEscapeForSure(foeAgility);

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, BotTranslations.Embed_Escape_Author)
            .WithDescription(Translation.Format(
                BotTranslations.Embed_Escape_Description,
                Formatter.Bold(agility.ToString()),
                Formatter.Bold(escapePercent.ToString()),
                Formatter.Bold(foeAgility.ToString()),
                Formatter.Bold(agilityToEscapeForSure.ToString())));

        await ctx.RespondAsync(embed);
    }
}
