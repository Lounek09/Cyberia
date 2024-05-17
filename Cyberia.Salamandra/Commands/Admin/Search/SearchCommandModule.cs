using Cyberia.Api;
using Cyberia.Api.Factories.Criteria;
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
        [MinMaxValue(0, 9999)]
        int effectId)
    {
        StringBuilder descriptionBuilder = new();

        switch (location)
        {
            case SearchLocation.Item:
                foreach (var itemStats in DofusApi.Datacenter.ItemsStatsRepository.ItemsStats)
                {
                    var itemHasEffect = itemStats.Value.Effects.Any(x => x.Id == effectId);

                    if (itemHasEffect)
                    {
                        var itemData = DofusApi.Datacenter.ItemsRepository.GetItemDataById(itemStats.Key);
                        if (itemData is not null)
                        {
                            descriptionBuilder.Append("- ");
                            descriptionBuilder.Append(itemData.Name);
                            descriptionBuilder.Append(" (");
                            descriptionBuilder.Append(itemData.Id);
                            descriptionBuilder.Append(")\n");
                        }
                    }
                }
                break;
            case SearchLocation.Spell:
                foreach (var spells in DofusApi.Datacenter.SpellsRepository.Spells)
                {
                    foreach (var spellLevelData in spells.Value.GetSpellLevelsData())
                    {
                        var spellHasEffect = spellLevelData.Effects.Any(x => x.Id == effectId);

                        if (spellHasEffect)
                        {
                            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(spells.Key);
                            if (spellData is not null)
                            {
                                descriptionBuilder.Append("- ");
                                descriptionBuilder.Append(spellData.Name);
                                descriptionBuilder.Append(" (");
                                descriptionBuilder.Append(spellData.Id);
                                descriptionBuilder.Append(")\n");

                                break;
                            }
                        }
                    }
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
                foreach (var items in DofusApi.Datacenter.ItemsRepository.Items)
                {
                    var itemHasCriterion = items.Value.Criteria
                        .OfType<ICriterion>()
                        .Any(x => x.Id.Equals(criterionId));

                    if (itemHasCriterion)
                    {
                        descriptionBuilder.Append("- ");
                        descriptionBuilder.Append(items.Value.Name);
                        descriptionBuilder.Append(" (");
                        descriptionBuilder.Append(items.Key);
                        descriptionBuilder.Append(")\n");
                    }
                }
                break;
            case SearchLocation.Spell:
                foreach (var spells in DofusApi.Datacenter.SpellsRepository.Spells)
                {
                    foreach (var spellLevelData in spells.Value.GetSpellLevelsData())
                    {
                        var spellHasCriterion = spellLevelData.Effects.Select(x => x.Criteria)
                            .Any(x =>
                            {
                                return x.OfType<ICriterion>()
                                    .Any(x => x.Id.Equals(criterionId));
                            });

                        if (spellHasCriterion)
                        {
                            var spellData = DofusApi.Datacenter.SpellsRepository.GetSpellDataById(spells.Key);
                            if (spellData is not null)
                            {
                                descriptionBuilder.Append("- ");
                                descriptionBuilder.Append(spellData.Name);
                                descriptionBuilder.Append(" (");
                                descriptionBuilder.Append(spellData.Id);
                                descriptionBuilder.Append(")\n");

                                break;
                            }
                        }
                    }
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
