using Cyberia.Api;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Pets;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Managers;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.DsharpPlus;

public static class ExtendDiscordEmbedBuilder
{
    public static DiscordEmbedBuilder AddEmptyField(this DiscordEmbedBuilder embed, bool inline = false)
    {
        return embed.AddField(Constant.ZERO_WIDTH_SPACE, Constant.ZERO_WIDTH_SPACE, inline);
    }

    public static DiscordEmbedBuilder AddFields(this DiscordEmbedBuilder embed, string name, IEnumerable<string> rows, bool inline = false)
    {
        StringBuilder builder = new();

        foreach (var row in rows)
        {
            if (row.Length > 1024)
            {
                throw new ArgumentException("One row exceeds 1024 characters and embed field value cannot exceed this length.");
            }

            if (builder.Length + row.Length > 1024)
            {
                builder.Length--;
                embed.AddField(name, builder.ToString(), inline);
                builder.Clear();
            }

            builder.Append(row);
            builder.Append('\n');
        }

        builder.Length--;
        return embed.AddField(name, builder.ToString(), inline);
    }

    public static DiscordEmbedBuilder AddEffectFields(this DiscordEmbedBuilder embed, string name, IEnumerable<IEffect> effects, bool inline = false)
    {
        return embed.AddFields(name, GetEffectsParse(effects, x => Formatter.Bold(Formatter.Sanitize(x))), inline);
    }

    public static DiscordEmbedBuilder AddCriteriaFields(this DiscordEmbedBuilder embed, CriteriaCollection criteria, bool inline = false)
    {
        var criteriaParse = GetCriteriaParse(criteria, x => Formatter.Bold(Formatter.Sanitize(x)));
        return embed.AddFields("Conditions : ", criteriaParse, inline);
    }

    public static DiscordEmbedBuilder AddQuestObjectiveFields(this DiscordEmbedBuilder embed, IEnumerable<IQuestObjective> questObjectives, bool inline = false)
    {
        List<string> questObjectivesParse = [];

        foreach (var questObjective in questObjectives)
        {
            var questObjectiveDescription = questObjective.GetDescription().ToString(x => Formatter.Bold(Formatter.Sanitize(x)));
            questObjectivesParse.Add(questObjectiveDescription);
        }

        return embed.AddFields("Objectifs :", questObjectivesParse, inline);
    }

    public static DiscordEmbedBuilder WithCraftDescription(this DiscordEmbedBuilder embed, CraftData craftData, int qte, bool recursive)
    {
        List<string> result = [];

        var ingredients = recursive ? craftData.GetIngredientsWithSubCraft(qte) : craftData.GetIngredients(qte);
        foreach (var ingredient in ingredients)
        {
            var quantity = Formatter.Bold(ingredient.Value.ToStringThousandSeparator());
            var itemName = Formatter.Sanitize(ingredient.Key.Name);

            if (!recursive)
            {
                var subCraftData = DofusApi.Datacenter.CraftsData.GetCraftDataById(ingredient.Key.Id);
                if (subCraftData is not null)
                {
                    itemName = Formatter.Bold(itemName);
                }
            }

            result.Add($"{quantity}x {itemName}");
        }

        return embed.WithDescription(string.Join('\n', result));
    }

    public static DiscordEmbedBuilder AddCraftField(this DiscordEmbedBuilder embed, CraftData craftData, int qte, bool inline = false)
    {
        List<string> result = [];

        foreach (var ingredient in craftData.GetIngredients(qte))
        {
            var quantity = Formatter.Bold(ingredient.Value.ToStringThousandSeparator());
            var itemName = Formatter.Sanitize(ingredient.Key.Name);

            result.Add($"{quantity}x {itemName}");
        }

        return embed.AddField("Craft :", string.Join(" + ", result), inline);
    }

    public static DiscordEmbedBuilder AddWeaponInfosField(this DiscordEmbedBuilder embed, ItemWeaponData itemWeaponData, bool twoHanded, ItemTypeData? itemTypeData, bool inline = false)
    {
        StringBuilder builder = new();

        builder.Append("PA : ");
        builder.Append(Formatter.Bold(itemWeaponData.ActionPointCost.ToString()));
        builder.Append('\n');

        builder.Append("Portée : ");
        builder.Append(Formatter.Bold(itemWeaponData.MinRange.ToString()));
        builder.Append(" à ");
        builder.Append(Formatter.Bold(itemWeaponData.MaxRange.ToString()));
        builder.Append('\n');

        if (itemWeaponData.CriticalBonus != 0)
        {
            builder.Append("Bonus coups critique : ");
            builder.Append(Formatter.Bold(itemWeaponData.CriticalBonus.ToString()));
            builder.Append('\n');
        }

        if (itemWeaponData.CriticalHitRate != 0)
        {
            builder.Append("Critique : 1/");
            builder.Append(Formatter.Bold(itemWeaponData.CriticalHitRate.ToString()));
            builder.Append(itemWeaponData.CriticalFailureRate != 0 ? " - " : "\n");
        }

        if (itemWeaponData.CriticalFailureRate != 0)
        {
            builder.Append("Échec : 1/");
            builder.Append(Formatter.Bold(itemWeaponData.CriticalFailureRate.ToString()));
            builder.Append('\n');
        }

        if (itemWeaponData.LineOnly)
        {
            builder.Append("Lancer en ligne uniquement\n");
        }

        if (!itemWeaponData.LineOfSight && itemWeaponData.MaxRange > 1)
        {
            builder.Append("Ne possède pas de ligne de vue\n");

        }

        builder.Append(twoHanded ? "Arme à deux mains" : "Arme à une main");

        if (itemTypeData is not null && itemTypeData.EffectArea != EffectArea.Default)
        {
            builder.Append("\nZone : ");
            builder.Append(Emojis.Area(itemTypeData.EffectArea.Id));
            builder.Append(' ');
            builder.Append(itemTypeData.EffectArea.GetDescription());
        }

        return embed.AddField("Caractéristiques :", builder.ToString(), inline);
    }

    public static DiscordEmbedBuilder AddPetField(this DiscordEmbedBuilder embed, PetData petData, bool inline = false)
    {
        StringBuilder builder = new();

        if (petData.MinFoodInterval.HasValue && petData.MaxFoodInterval.HasValue)
        {
            builder.Append("Repas entre ");
            builder.Append(Formatter.Bold(petData.MinFoodInterval.Value.TotalHours.ToString()));
            builder.Append("h et ");
            builder.Append(Formatter.Bold(petData.MaxFoodInterval.Value.TotalHours.ToString()));
            builder.Append("h\n");
        }

        foreach (var petFoodsData in petData.Foods)
        {
            if (petFoodsData.Effect is not null)
            {
                builder.Append(Emojis.Effect(petFoodsData.Effect.Id));
                builder.Append(' ');
                builder.Append(Formatter.Bold(petFoodsData.Effect.GetDescription()));
                builder.Append(" :\n");

                if (petFoodsData.ItemsId.Count > 0)
                {
                    var itemsName = petFoodsData.ItemsId
                        .Select(x => DofusApi.Datacenter.ItemsData.GetItemNameById(x));

                    builder.Append(string.Join(", ", itemsName));
                    builder.Append('\n');
                }

                if (petFoodsData.ItemTypesId.Count > 0)
                {
                    var itemTypesName = petFoodsData.ItemTypesId
                        .Select(x => DofusApi.Datacenter.ItemsData.GetItemTypeNameById(x));

                    builder.Append(string.Join(", ", itemTypesName));
                    builder.Append('\n');
                }

                if (petFoodsData.MonstersIdQuantities.Count > 0)
                {
                    foreach (var group in petFoodsData.MonstersIdQuantities.GroupBy(x => x.Value))
                    {
                        var monstersName = group
                            .Select(x => DofusApi.Datacenter.MonstersData.GetMonsterNameById(x.Key));

                        builder.Append(Formatter.Bold(group.Key.ToString()));
                        builder.Append("x ");
                        builder.Append(string.Join(", ", monstersName));
                        builder.Append('\n');
                    }
                }

                if (petFoodsData.MonsterRacesIdQuantities.Count > 0)
                {
                    foreach (var group in petFoodsData.MonsterRacesIdQuantities.GroupBy(x => x.Value))
                    {
                        var monsterRacesName = group
                            .Select(x => DofusApi.Datacenter.MonstersData.GetMonsterRaceNameById(x.Key));

                        builder.Append(Formatter.Bold(group.Key.ToString()));
                        builder.Append("x ");
                        builder.Append(string.Join(", ", monsterRacesName));
                        builder.Append('\n');
                    }
                }

                if (petFoodsData.MonsterSuperRacesIdQuantities.Count > 0)
                {
                    foreach (var group in petFoodsData.MonsterSuperRacesIdQuantities.GroupBy(x => x.Value))
                    {
                        var monsterSuperRacesName = group
                            .Select(x => DofusApi.Datacenter.MonstersData.GetMonsterSuperRaceNameById(x.Key));

                        builder.Append(Formatter.Bold(group.Key.ToString()));
                        builder.Append("x ");
                        builder.Append(string.Join(", ", monsterSuperRacesName));
                        builder.Append('\n');

                    }
                }
            }
        }

        return embed.AddField("Familier :", builder.ToString(), inline);
    }

    private static List<string> GetEffectsParse(IEnumerable<IEffect> effects, Func<string, string>? parametersDecorator = null)
    {
        List<string> effectsParse = [];

        foreach (var effect in effects)
        {
            if (effect is DisplayEffectsFromItemEffect displayEffectsFromItemEffect)
            {
                var itemStatsData = displayEffectsFromItemEffect.GetItemData()?.GetItemStatsData();
                if (itemStatsData is not null)
                {
                    effectsParse.AddRange(GetEffectsParse(itemStatsData.Effects, parametersDecorator));
                }

                continue;
            }

            var emoji = effect switch
            {
                StateEffect stateEffect => Emojis.State(stateEffect.StateId),
                _ => Emojis.Effect(effect.Id)
            };

            var effectDescription = parametersDecorator is null ? effect.GetDescription() : effect.GetDescription().ToString(parametersDecorator);

            var areaInfo = effect.EffectArea.Size == EffectArea.Default.Size ? string.Empty : $" - {Emojis.Area(effect.EffectArea.Id)} {effect.EffectArea.GetSize()}";

            var effectParse = $"{emoji} {effectDescription}{areaInfo}";

            if (effect.Criteria.Count > 0)
            {
                var criteriaParse = GetCriteriaParse(effect.Criteria);
                effectParse += $" {Formatter.InlineCode(string.Join(' ', criteriaParse))}";
            }

            effectsParse.Add(effectParse);
        }

        return effectsParse;
    }

    private static List<string> GetCriteriaParse(CriteriaCollection criteria, Func<string, string>? parametersDecorator = null)
    {
        List<string> criteriaParse = [string.Empty];

        foreach (var element in criteria)
        {
            switch (element)
            {
                case CriteriaLogicalOperator logicalOperator:
                    criteriaParse[^1] += $"{logicalOperator.GetDescription()} ";
                    break;
                case CriteriaCollection subCriteria:
                    var subCriteriaParse = GetCriteriaParse(subCriteria, parametersDecorator);
                    criteriaParse[^1] += "(" + subCriteriaParse[0];

                    if (subCriteriaParse.Count > 1)
                    {
                        criteriaParse.AddRange(subCriteriaParse.GetRange(1, subCriteriaParse.Count - 1));
                    }

                    criteriaParse[^1] += ")";
                    criteriaParse.Add(string.Empty);
                    break;
                case ICriterion criterion:
                    criteriaParse[^1] += parametersDecorator is null ? criterion.GetDescription() : criterion.GetDescription().ToString(parametersDecorator);
                    criteriaParse.Add(string.Empty);
                    break;
            }
        }

        if (string.IsNullOrEmpty(criteriaParse[^1]))
        {
            criteriaParse.RemoveAt(criteriaParse.Count - 1);
        }

        return criteriaParse;
    }
}
