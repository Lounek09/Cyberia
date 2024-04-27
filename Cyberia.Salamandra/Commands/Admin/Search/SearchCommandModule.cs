using Cyberia.Api;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.SlashCommands.ArgumentModifiers;
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
        SearchLocation where,
        [Parameter("id"), Description("Effect id")]
        [SlashMinMaxValue(MinValue = 0, MaxValue = 9999)]
        int effectId)
    {
        StringBuilder descriptionBuilder = new();

        switch (where)
        {
            case SearchLocation.Item:
                foreach (var itemStats in DofusApi.Datacenter.ItemsStatsData.ItemsStats)
                {
                    var itemHasEffect = itemStats.Value.Effects.Any(x => x.Id == effectId);

                    if (itemHasEffect)
                    {
                        var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(itemStats.Key);
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
                foreach (var spells in DofusApi.Datacenter.SpellsData.Spells)
                {
                    foreach (var spellLevelData in spells.Value.GetSpellLevelsData())
                    {
                        var spellHasEffect = spellLevelData.Effects.Any(x => x.Id == effectId);

                        if (spellHasEffect)
                        {
                            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spells.Key);
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
                await ctx.RespondAsync($"Unknown {Formatter.Bold(where.ToString())}");
                return;
        }

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, "Tools")
            .WithTitle($"Search effect {effectId} in {where}")
            .WithDescription(descriptionBuilder.ToString().WithMaxLength(4000));

        await ctx.RespondAsync(embed);
    }

    [Command("criterion"), Description("Search where the criterion is used")]
    [SlashCommandTypes(DiscordApplicationCommandType.SlashCommand)]
    public static async Task CriterionExecuteAsync(SlashCommandContext ctx,
        [Parameter("where"), Description("Where to look for the criterion")]
        SearchLocation where,
        [Parameter("id"), Description("Criterion id")]
        [SlashMinMaxLength(MinLength = 2, MaxLength = 2)]
        string criterionId)
    {
        StringBuilder descriptionBuilder = new();

        switch (where)
        {
            case SearchLocation.Item:
                foreach (var items in DofusApi.Datacenter.ItemsData.Items)
                {
                    var itemHasCriterion = items.Value.Criteria.OfType<ICriterion>()
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
                foreach (var spells in DofusApi.Datacenter.SpellsData.Spells)
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
                            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spells.Key);
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
                await ctx.RespondAsync($"Unknown {Formatter.Bold(where.ToString())}");
                return;
        }

        await ctx.RespondAsync(EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, $"Search criterion {criterionId} in {where}")
            .WithDescription(descriptionBuilder.ToString().WithMaxLength(4000)));
    }
}
