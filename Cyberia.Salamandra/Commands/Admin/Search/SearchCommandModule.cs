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
using System.Text;

namespace Cyberia.Salamandra.Commands.Admin.Search;

[Command("search")]
[InteractionInstallType(DiscordApplicationIntegrationType.GuildInstall)]
[InteractionAllowedContexts(DiscordInteractionContextType.Guild)]
public sealed class SearchCommandModule
{
    [Command("effect"), Description("Search where the effect is used")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task EffectExecuteAsync(SlashCommandContext ctx,
        [Parameter("where"), Description("Where to look for the effect")]
        SearchLocation location,
        [Parameter("id"), Description("Effect id")]
        [MinMaxValue(-1, 9999)]
        int effectId)
    {
        StringBuilder descriptionBuilder = new();

        switch (location)
        {
            case SearchLocation.Item:
                foreach (var itemData in DofusApi.Datacenter.ItemsRepository.GetItemsDataWithEffectId(effectId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(itemData.Name);
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(itemData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            case SearchLocation.Spell:
                foreach (var spellData in DofusApi.Datacenter.SpellsRepository.GetSpellsDataWithEffectId(effectId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(spellData.Name);
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(spellData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            default:
                await ctx.RespondAsync($"Unknown {Formatter.Bold(location.ToString())}");
                return;
        }

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
            .WithTitle($"Search effect {effectId} in {location}")
            .WithDescription(descriptionBuilder.ToString().WithMaxLength(4000));

        await ctx.RespondAsync(embed);
    }

    [Command("criterion"), Description("Search where the criterion is used")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task CriterionExecuteAsync(SlashCommandContext ctx,
        [Parameter("where"), Description("Where to look for the criterion")]
        SearchLocation location,
        [Parameter("id"), Description("Criterion id")]
        [MinMaxLength(2, 2)]
        string criterionId)
    {
        StringBuilder descriptionBuilder = new();

        switch (location)
        {
            case SearchLocation.Item:
                foreach (var itemData in DofusApi.Datacenter.ItemsRepository.GetItemsDataWithCriterionId(criterionId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(itemData.Name);
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(itemData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            case SearchLocation.Spell:
                foreach (var spellData in DofusApi.Datacenter.SpellsRepository.GetSpellsDataWithCriterionId(criterionId))
                {
                    descriptionBuilder.Append("- ");
                    descriptionBuilder.Append(spellData.Name);
                    descriptionBuilder.Append(" (");
                    descriptionBuilder.Append(spellData.Id);
                    descriptionBuilder.Append(")\n");
                }
                break;
            default:
                await ctx.RespondAsync($"Unknown {Formatter.Bold(location.ToString())}");
                return;
        }

        await ctx.RespondAsync(EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, $"Search criterion {criterionId} in {location}")
            .WithDescription(descriptionBuilder.ToString().WithMaxLength(4000)));
    }
}
