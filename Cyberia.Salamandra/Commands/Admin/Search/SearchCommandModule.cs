using Cyberia.Api;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Salamandra.Managers;

using DSharpPlus;
using DSharpPlus.SlashCommands;

using System.Text;

namespace Cyberia.Salamandra.Commands.Admin;

[SlashCommandGroup("search", "Search")]
public sealed class SearchCommandModule : ApplicationCommandModule
{
    [SlashCommand("effect", "Search where the effect is used")]
    public async Task EffectSearchCommand(InteractionContext ctx,
        [Option("Where", "Where to look for the effect")]
        [Choice("Item", "item")]
        [Choice("Spell", "spell")]
        string where,
        [Option("Id", "Effect id")]
        [Minimum(-1), Maximum(9999)]
        long id)
    {
        var description = new StringBuilder();

        switch (where)
        {
            case "item":
                foreach (var itemStats in DofusApi.Datacenter.ItemsStatsData.ItemsStats)
                {
                    var itemHasEffect = itemStats.Value.Effects.Any(x => x.Id == id);

                    if (itemHasEffect)
                    {
                        var itemData = DofusApi.Datacenter.ItemsData.GetItemDataById(itemStats.Key);
                        if (itemData is not null)
                        {
                            description.Append("- ");
                            description.Append(itemData.Name);
                            description.Append(" (");
                            description.Append(itemData.Id);
                            description.Append(")\n");
                        }
                    }
                }
                break;
            case "spell":
                foreach (var spells in DofusApi.Datacenter.SpellsData.Spells)
                {
                    foreach (var spellLevelData in spells.Value.GetSpellLevelsData())
                    {
                        var spellHasEffect = spellLevelData.Effects.Any(x => x.Id == id);

                        if (spellHasEffect)
                        {
                            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spells.Key);
                            if (spellData is not null)
                            {
                                description.Append("- ");
                                description.Append(spellData.Name);
                                description.Append(" (");
                                description.Append(spellData.Id);
                                description.Append(")\n");

                                break;
                            }
                        }
                    }
                }
                break;
            default:
                await ctx.CreateResponseAsync($"Unknown {Formatter.Bold(where)}");
                return;

        }

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, $"Search effect {id} in {where}")
            .WithDescription(description.ToString().WithMaxLength(4000));

        await ctx.CreateResponseAsync(embed);
    }

    [SlashCommand("criterion", "Search where the criterion is used")]
    public async Task CriterionSearchCommand(InteractionContext ctx,
        [Option("Where", "Where to look for the criterion")]
        [Choice("Item", "item")]
        [Choice("Spell", "spell")]
        string where,
        [Option("Id", "Criterion id")]
        [MinimumLength(2), MaximumLength(2)]
        string id)
    {
        var description = new StringBuilder();

        switch (where)
        {
            case "item":
                foreach (var items in DofusApi.Datacenter.ItemsData.Items)
                {
                    var itemHasCriterion = items.Value.Criteria.OfType<ICriterion>()
                        .Any(x => x.Id.Equals(id));

                    if (itemHasCriterion)
                    {
                        description.Append("- ");
                        description.Append(items.Value.Name);
                        description.Append(" (");
                        description.Append(items.Key);
                        description.Append(")\n");
                    }
                }
                break;
            case "spell":
                foreach (var spells in DofusApi.Datacenter.SpellsData.Spells)
                {
                    foreach (var spellLevelData in spells.Value.GetSpellLevelsData())
                    {
                        var spellHasCriterion = spellLevelData.Effects.Select(x => x.Criteria)
                            .Any(x =>
                            {
                                return x.OfType<ICriterion>()
                                    .Any(x => x.Id.Equals(id));
                            });

                        if (spellHasCriterion)
                        {
                            var spellData = DofusApi.Datacenter.SpellsData.GetSpellDataById(spells.Key);
                            if (spellData is not null)
                            {
                                description.Append("- ");
                                description.Append(spellData.Name);
                                description.Append(" (");
                                description.Append(spellData.Id);
                                description.Append(")\n");

                                break;
                            }
                        }
                    }
                }
                break;
            default:
                await ctx.CreateResponseAsync($"Unknown {Formatter.Bold(where)}");
                return;

        }

        var embed = EmbedManager.CreateEmbedBuilder(EmbedCategory.Tools, $"Search criterion {id} in {where}")
            .WithDescription(description.ToString().WithMaxLength(4000));

        await ctx.CreateResponseAsync(embed);
    }
}
