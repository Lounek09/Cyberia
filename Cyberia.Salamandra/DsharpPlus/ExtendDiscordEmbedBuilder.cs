using Cyberia.Api;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Pets;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.QuestObjectives;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Text;

namespace Cyberia.Salamandra.DsharpPlus;

/// <summary>
/// Provides extension methods for <see cref="DiscordEmbedBuilder"/>.
/// </summary>
public static class ExtendDiscordEmbedBuilder
{
    /// <summary>
    /// Adds an empty field to the embed.
    /// </summary>
    /// <param name="embed">The embed.</param>
    /// <param name="inline">Whether the field is to be inline or not.</param>
    /// <returns>The embed builder.</returns>
    public static DiscordEmbedBuilder AddEmptyField(this DiscordEmbedBuilder embed, bool inline = false)
    {
        return embed.AddField(Constant.ZeroWidthSpace, Constant.ZeroWidthSpace, inline);
    }

    /// <summary>
    /// Adds fields to the embed splitting the content if it exceeds 1024 characters.
    /// </summary>
    /// <param name="embed">The embed.</param>
    /// <param name="name">The name of the field.</param>
    /// <param name="rows">The content of the field.</param>
    /// <param name="inline">Whether the field is to be inline or not.</param>
    /// <returns>The embed builder.</returns>
    /// <exception cref="ArgumentException">Thrown if one row exceeds 1024 characters.</exception>
    public static DiscordEmbedBuilder AddFields(this DiscordEmbedBuilder embed, string name, IEnumerable<string> rows, bool inline = false)
    {
        StringBuilder builder = new();

        foreach (var row in rows)
        {
            if (row.Length > Constant.MaxEmbedFieldSize)
            {
                throw new ArgumentException($"One row exceeds {Constant.MaxEmbedFieldSize} characters and embed field value cannot exceed this length.");
            }

            if (builder.Length + row.Length > 1024)
            {
                embed.AddField(name, builder.ToString(), inline);
                builder.Clear();
            }

            builder.Append(row);
            builder.Append('\n');
        }

        return embed.AddField(name, builder.ToString(), inline);
    }

    public static DiscordEmbedBuilder AddEffectFields(this DiscordEmbedBuilder embed, string name, IEnumerable<IEffect> effects, bool inline = false)
    {
        var effectsParse = GetEffectsParse(effects, x => Formatter.Bold(Formatter.Sanitize(x)));
        return embed.AddFields(name, effectsParse, inline);
    }

    public static DiscordEmbedBuilder AddCriteriaFields(this DiscordEmbedBuilder embed, CriteriaReadOnlyCollection criteria, bool inline = false)
    {
        var criteriaParse = GetCriteriaParse(criteria, x => Formatter.Bold(Formatter.Sanitize(x)));
        return embed.AddFields(BotTranslations.Embed_Field_Criteria_Title, criteriaParse, inline);
    }

    public static DiscordEmbedBuilder AddQuestObjectiveFields(this DiscordEmbedBuilder embed, IEnumerable<IQuestObjective> questObjectives, bool inline = false)
    {
        List<string> questObjectivesParse = [];

        foreach (var questObjective in questObjectives)
        {
            var questObjectiveDescription = questObjective is FreeFormQuestObjective
                ? questObjective.GetDescription()
                : questObjective.GetDescription().ToString(x => Formatter.Bold(Formatter.Sanitize(x)));

            questObjectivesParse.Add(questObjectiveDescription);
        }

        return embed.AddFields(BotTranslations.Embed_Field_QuestObjectives_Title, questObjectivesParse, inline);
    }

    public static DiscordEmbedBuilder WithCraftDescription(this DiscordEmbedBuilder embed, CraftData craftData, int quantity, bool recursive)
    {
        List<string> result = [];

        var ingredients = recursive ? craftData.GetIngredientsWithSubCraft(quantity) : craftData.GetIngredients(quantity);
        foreach (var ingredient in ingredients)
        {
            var quantityFormatted = Formatter.Bold(ingredient.Value.ToFormattedString());
            var itemName = Formatter.Sanitize(ingredient.Key.Name);

            if (!recursive)
            {
                var subCraftData = DofusApi.Datacenter.CraftsRepository.GetCraftDataById(ingredient.Key.Id);
                if (subCraftData is not null)
                {
                    itemName = Formatter.Bold(itemName);
                }
            }

            result.Add($"{quantityFormatted}x {itemName}");
        }

        return embed.WithDescription(string.Join('\n', result));
    }

    public static DiscordEmbedBuilder AddCraftField(this DiscordEmbedBuilder embed, CraftData craftData, int quantity, bool inline = false)
    {
        List<string> result = [];

        foreach (var ingredient in craftData.GetIngredients(quantity))
        {
            var quantityFormatted = Formatter.Bold(ingredient.Value.ToFormattedString());
            var itemName = Formatter.Sanitize(ingredient.Key.Name);

            result.Add($"{quantityFormatted}x {itemName}");
        }

        return embed.AddField(BotTranslations.Embed_Field_Craft_Title, string.Join(" + ", result), inline);
    }

    public static DiscordEmbedBuilder AddWeaponInfosField(this DiscordEmbedBuilder embed, ItemWeaponData itemWeaponData, bool twoHanded, ItemTypeData? itemTypeData, bool inline = false)
    {
        StringBuilder builder = new();

        builder.Append(BotTranslations.Embed_Field_Weapon_Content_AP);
        builder.Append(' ');
        builder.Append(Formatter.Bold(itemWeaponData.ActionPointCost.ToString()));
        builder.Append('\n');

        builder.Append(BotTranslations.Embed_Field_Weapon_Content_Range);
        builder.Append(' ');
        builder.Append(Formatter.Bold(itemWeaponData.MinRange.ToString()));
        builder.Append(BotTranslations.to);
        builder.Append(Formatter.Bold(itemWeaponData.MaxRange.ToString()));
        builder.Append('\n');

        if (itemWeaponData.CriticalBonus != 0)
        {
            builder.Append(BotTranslations.Embed_Field_Weapon_Content_CriticalHitBonus);
            builder.Append(' ');
            builder.Append(Formatter.Bold(itemWeaponData.CriticalBonus.ToString()));
            builder.Append('\n');
        }

        if (itemWeaponData.CriticalHitRate != 0)
        {
            builder.Append(BotTranslations.Embed_Field_Weapon_Content_CriticalHit);
            builder.Append(" 1/");
            builder.Append(Formatter.Bold(itemWeaponData.CriticalHitRate.ToString()));
        }

        if (itemWeaponData.CriticalFailureRate != 0)
        {
            builder.Append(" - ");
            builder.Append(BotTranslations.Embed_Field_Weapon_Content_CriticalFailure);
            builder.Append(" 1/");
            builder.Append(Formatter.Bold(itemWeaponData.CriticalFailureRate.ToString()));
            builder.Append('\n');
        }
        else
        {
            builder.Append('\n');
        }

        if (itemWeaponData.Linear)
        {
            builder.Append(BotTranslations.Embed_Field_Weapon_Content_Linear);
            builder.Append('\n');
        }

        if (!itemWeaponData.LineOfSight && itemWeaponData.MaxRange > 1)
        {
            builder.Append(BotTranslations.Embed_Field_Weapon_Content_LineOfSight);
            builder.Append('\n');
        }

        builder.Append(twoHanded ? BotTranslations.Embed_Field_Weapon_Content_OneHanded : BotTranslations.Embed_Field_Weapon_Content_TwoHanded);

        if (itemTypeData is not null && itemTypeData.EffectArea != EffectAreaFactory.Default)
        {
            builder.Append('\n');
            builder.Append(BotTranslations.Embed_Field_Weapon_Content_Area);
            builder.Append(' ');
            builder.Append(Emojis.EffectArea(itemTypeData.EffectArea.Id));
            builder.Append(' ');
            builder.Append(itemTypeData.EffectArea.GetDescription().ToString(Formatter.Bold));
        }

        return embed.AddField(BotTranslations.Embed_Field_Weapon_Title, builder.ToString(), inline);
    }

    public static DiscordEmbedBuilder AddPetField(this DiscordEmbedBuilder embed, PetData petData, bool inline = false)
    {
        StringBuilder builder = new();

        if (petData.MinFoodInterval.HasValue && petData.MaxFoodInterval.HasValue)
        {
            builder.Append(BotTranslations.Embed_Field_Pet_Content_MealBetween);
            builder.Append(' ');
            builder.Append(Formatter.Bold(petData.MinFoodInterval.Value.TotalHours.ToString()));
            builder.Append('h');
            builder.Append(BotTranslations.and);
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
                        .Select(x => DofusApi.Datacenter.ItemsRepository.GetItemNameById(x));

                    builder.Append(string.Join(", ", itemsName));
                    builder.Append('\n');
                }

                if (petFoodsData.ItemTypesId.Count > 0)
                {
                    var itemTypesName = petFoodsData.ItemTypesId
                        .Select(x => DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(x));

                    builder.Append(string.Join(", ", itemTypesName));
                    builder.Append('\n');
                }

                if (petFoodsData.MonstersIdQuantities.Count > 0)
                {
                    foreach (var group in petFoodsData.MonstersIdQuantities.GroupBy(x => x.Value))
                    {
                        var monstersName = group
                            .Select(x => DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(x.Key));

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
                            .Select(x => DofusApi.Datacenter.MonstersRepository.GetMonsterRaceNameById(x.Key));

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
                            .Select(x => DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceNameById(x.Key));

                        builder.Append(Formatter.Bold(group.Key.ToString()));
                        builder.Append("x ");
                        builder.Append(string.Join(", ", monsterSuperRacesName));
                        builder.Append('\n');

                    }
                }
            }
        }

        return embed.AddField(BotTranslations.Embed_Field_Pet_Title, builder.ToString(), inline);
    }

    private static List<string> GetEffectsParse(IEnumerable<IEffect> effects, Func<string, string>? parametersDecorator = null)
    {
        List<string> effectsParse = [];

        foreach (var effect in effects)
        {
            if (effect is ReplaceEffect replaceEffect)
            {
                var itemStatsData = replaceEffect.GetItemData()?.GetItemStatsData();
                if (itemStatsData is not null)
                {
                    effectsParse.AddRange(GetEffectsParse(itemStatsData.Effects, parametersDecorator));
                }

                continue;
            }

            var effectDescription = parametersDecorator is null
                ? effect.GetDescription()
                : effect.GetDescription().ToString(parametersDecorator);

            var areaInfo = effect.EffectArea.Size == EffectAreaFactory.Default.Size
                ? string.Empty
                : $" - {Emojis.EffectArea(effect.EffectArea.Id)} {effect.EffectArea.GetSize()}";

            var effectParse = $"{Emojis.Effect(effect.Id)} {effectDescription}{areaInfo}";

            if (effect.Criteria.Count > 0)
            {
                var criteriaParse = GetCriteriaParse(effect.Criteria);
                effectParse += $" {Formatter.InlineCode(string.Join(' ', criteriaParse))}";
            }

            effectsParse.Add(effectParse);
        }

        return effectsParse;
    }

    private static List<string> GetCriteriaParse(CriteriaReadOnlyCollection criteria, Func<string, string>? parametersDecorator = null)
    {
        List<string> criteriaParse = [string.Empty];

        foreach (var element in criteria)
        {
            switch (element)
            {
                case CriteriaLogicalOperator logicalOperator:
                    criteriaParse[^1] += $"{logicalOperator.GetDescription()} ";
                    break;
                case CriteriaReadOnlyCollection subCriteria:
                    var subCriteriaParse = GetCriteriaParse(subCriteria, parametersDecorator);
                    criteriaParse[^1] += '(' + subCriteriaParse[0];

                    if (subCriteriaParse.Count > 1)
                    {
                        criteriaParse.AddRange(subCriteriaParse.GetRange(1, subCriteriaParse.Count - 1));
                    }

                    criteriaParse[^1] += ')';
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
