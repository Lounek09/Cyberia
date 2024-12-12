using Cyberia.Api;
using Cyberia.Api.Data.Crafts;
using Cyberia.Api.Data.Items;
using Cyberia.Api.Data.Pets;
using Cyberia.Api.Factories;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.Effects;
using Cyberia.Api.Factories.Effects.Elements;
using Cyberia.Api.Factories.QuestObjectives;
using Cyberia.Api.Factories.QuestObjectives.Elements;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Extensions.DSharpPlus;

/// <summary>
/// Provides extension methods for <see cref="DiscordEmbedBuilder"/>.
/// </summary>
public static class DiscordEmbedBuilderExtensions
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
            if (row.Length > Constant.MaxEmbedFieldContentSize)
            {
                throw new ArgumentException($"One row exceeds {Constant.MaxEmbedFieldContentSize} characters and embed field value cannot exceed this length.");
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

    public static DiscordEmbedBuilder AddEffectFields(this DiscordEmbedBuilder embed, string name, IEnumerable<IEffect> effects, bool sort, CultureInfo? culture, bool inline = false)
    {
        if (effects.Any())
        {
            var effectsParse = GetEffectsDescription(effects, sort, culture, x => Formatter.Bold(Formatter.Sanitize(x)));
            return embed.AddFields(name, effectsParse, inline);
        }

        return embed.AddField(name, Translation.Get<BotTranslations>("Embed.Field.Effects.Content.None", culture), inline);
    }

    public static DiscordEmbedBuilder AddCriteriaFields(this DiscordEmbedBuilder embed, CriteriaReadOnlyCollection criteria, CultureInfo? culture, bool inline = false)
    {
        var criteriaParse = GetCriteriaDescription(criteria, culture, x => Formatter.Bold(Formatter.Sanitize(x)));
        return embed.AddFields(Translation.Get<BotTranslations>("Embed.Field.Criteria.Title", culture), criteriaParse, inline);
    }

    public static DiscordEmbedBuilder AddQuestObjectivesFields(this DiscordEmbedBuilder embed, IEnumerable<IQuestObjective> questObjectives, CultureInfo? culture, bool inline = false)
    {
        List<string> questObjectivesParse = [];

        foreach (var questObjective in questObjectives)
        {
            var questObjectiveDescription = questObjective is FreeFormQuestObjective
                ? questObjective.GetDescription(culture)
                : questObjective.GetDescription(culture).ToString(x => Formatter.Bold(Formatter.Sanitize(x)));

            questObjectivesParse.Add(questObjectiveDescription);
        }

        return embed.AddFields(Translation.Get<BotTranslations>("Embed.Field.QuestObjectives.Title", culture), questObjectivesParse, inline);
    }

    public static DiscordEmbedBuilder WithCraftDescription(this DiscordEmbedBuilder embed, CraftData craftData, int quantity, bool recursive, CultureInfo? culture)
    {
        List<string> result = [];

        var ingredients = recursive ? craftData.GetIngredientsWithSubCraft(quantity) : craftData.GetIngredients(quantity);
        foreach (var ingredient in ingredients)
        {
            var quantityFormatted = Formatter.Bold(ingredient.Value.ToFormattedString(culture));
            var itemName = Formatter.Sanitize(ingredient.Key.Name.ToString(culture));

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

    public static DiscordEmbedBuilder AddCraftField(this DiscordEmbedBuilder embed, CraftData craftData, int quantity, CultureInfo? culture, bool inline = false)
    {
        List<string> result = [];

        foreach (var ingredient in craftData.GetIngredients(quantity))
        {
            var quantityFormatted = Formatter.Bold(ingredient.Value.ToFormattedString(culture));
            var itemName = Formatter.Sanitize(ingredient.Key.Name.ToString(culture));

            result.Add($"{quantityFormatted}x {itemName}");
        }

        return embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Craft.Title", culture), string.Join(" + ", result), inline);
    }

    public static DiscordEmbedBuilder AddWeaponInfosField(this DiscordEmbedBuilder embed, ItemWeaponData itemWeaponData, bool twoHanded, ItemTypeData? itemTypeData, CultureInfo? culture, bool inline = false)
    {
        StringBuilder builder = new();

        builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.AP", culture));
        builder.Append(' ');
        builder.Append(Formatter.Bold(itemWeaponData.ActionPointCost.ToString()));
        builder.Append('\n');

        builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.Range", culture));
        builder.Append(' ');
        builder.Append(Formatter.Bold(itemWeaponData.MinRange.ToString()));
        builder.Append(Translation.Get<BotTranslations>("to", culture));
        builder.Append(Formatter.Bold(itemWeaponData.MaxRange.ToString()));
        builder.Append('\n');

        if (itemWeaponData.CriticalBonus != 0)
        {
            builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.CriticalHitBonus", culture));
            builder.Append(' ');
            builder.Append(Formatter.Bold(itemWeaponData.CriticalBonus.ToString()));
            builder.Append('\n');
        }

        if (itemWeaponData.CriticalHitRate != 0)
        {
            builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.CriticalHit", culture));
            builder.Append(" 1/");
            builder.Append(Formatter.Bold(itemWeaponData.CriticalHitRate.ToString()));
        }

        if (itemWeaponData.CriticalFailureRate != 0)
        {
            builder.Append(" - ");
            builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.CriticalFailure", culture));
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
            builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.Linear", culture));
            builder.Append('\n');
        }

        if (!itemWeaponData.LineOfSight && itemWeaponData.MaxRange > 1)
        {
            builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.LineOfSight", culture));
            builder.Append('\n');
        }

        builder.Append(twoHanded
            ? Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.TwoHanded", culture)
            : Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.OneHanded", culture));

        if (itemTypeData is not null && itemTypeData.EffectArea != EffectAreaFactory.Default)
        {
            builder.Append('\n');
            builder.Append(Translation.Get<BotTranslations>("Embed.Field.Weapon.Content.Area", culture));
            builder.Append(' ');
            builder.Append(Emojis.EffectArea(itemTypeData.EffectArea));
            builder.Append(' ');
            builder.Append(itemTypeData.EffectArea.GetDescription(culture).ToString(Formatter.Bold));
        }

        return embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Weapon.Title", culture), builder.ToString(), inline);
    }

    public static DiscordEmbedBuilder AddPetField(this DiscordEmbedBuilder embed, PetData petData, CultureInfo? culture, bool inline = false)
    {
        StringBuilder builder = new();

        if (petData.MinFoodInterval.HasValue && petData.MaxFoodInterval.HasValue)
        {
            builder.Append(Translation.Get<BotTranslations>("Embed.Field.Pet.Content.MealBetween", culture));
            builder.Append(' ');
            builder.Append(Formatter.Bold(petData.MinFoodInterval.Value.TotalHours.ToString()));
            builder.Append('h');
            builder.Append(Translation.Get<BotTranslations>("and", culture));
            builder.Append(Formatter.Bold(petData.MaxFoodInterval.Value.TotalHours.ToString()));
            builder.Append("h\n");
        }

        foreach (var petFoodsData in petData.Foods)
        {
            builder.Append(Emojis.Effect(petFoodsData.Effect));
            builder.Append(' ');
            builder.Append(Formatter.Bold(petFoodsData.Effect.GetDescription(culture)));
            builder.Append(" :\n");

            if (petFoodsData.ItemsId.Count > 0)
            {
                var itemsName = petFoodsData.ItemsId
                    .Select(x => DofusApi.Datacenter.ItemsRepository.GetItemNameById(x, culture));

                builder.Append(string.Join(", ", itemsName));
                builder.Append('\n');
            }

            if (petFoodsData.ItemTypesId.Count > 0)
            {
                var itemTypesName = petFoodsData.ItemTypesId
                    .Select(x => DofusApi.Datacenter.ItemsRepository.GetItemTypeNameById(x, culture));

                builder.Append(string.Join(", ", itemTypesName));
                builder.Append('\n');
            }

            if (petFoodsData.MonstersIdQuantities.Count > 0)
            {
                foreach (var group in petFoodsData.MonstersIdQuantities.GroupBy(x => x.Value))
                {
                    var monstersName = group
                        .Select(x => DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(x.Key, culture));

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
                        .Select(x => DofusApi.Datacenter.MonstersRepository.GetMonsterRaceNameById(x.Key, culture));

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
                        .Select(x => DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceNameById(x.Key, culture));

                    builder.Append(Formatter.Bold(group.Key.ToString()));
                    builder.Append("x ");
                    builder.Append(string.Join(", ", monsterSuperRacesName));
                    builder.Append('\n');

                }
            }
        }

        return embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Pet.Title", culture), builder.ToString(), inline);
    }

    private static List<string> GetEffectsDescription(IEnumerable<IEffect> effects, bool sort, CultureInfo? culture, Func<string, string>? parametersDecorator = null)
    {
        List<string> effectsDescription = [];
        StringBuilder effectDescriptionBuilder = new();

        var sortedEffects = sort ? effects.OrderByDescending(x => x) : effects;

        foreach (var effect in sortedEffects)
        {
            if (effect is ReplaceEffect replaceEffect)
            {
                var itemStatsData = replaceEffect.GetItemData()?.GetItemStatsData();
                if (itemStatsData is not null)
                {
                    effectsDescription.AddRange(GetEffectsDescription(itemStatsData.Effects, sort, culture, parametersDecorator));
                    continue;
                }
            }

            effectDescriptionBuilder.Append(Emojis.Effect(effect));
            effectDescriptionBuilder.Append(' ');

            if (parametersDecorator is null)
            {
                effectDescriptionBuilder.Append(effect.GetDescription(culture));
            }
            else
            {
                effectDescriptionBuilder.Append(effect.GetDescription(culture).ToString(parametersDecorator));
            }

            if (!effect.Dispellable)
            {
                effectDescriptionBuilder.Append("\\🔒");
            }

            if (effect.EffectArea.Size != EffectAreaFactory.Default.Size)
            {
                effectDescriptionBuilder.Append(" - ");
                effectDescriptionBuilder.Append(Emojis.EffectArea(effect.EffectArea));
                effectDescriptionBuilder.Append(' ');
                effectDescriptionBuilder.Append(effect.EffectArea.GetSize(culture));
            }

            if (effect.Criteria.Count > 0)
            {
                var criteriaParse = GetCriteriaDescription(effect.Criteria, culture: culture);
                effectDescriptionBuilder.Append(' ');
                effectDescriptionBuilder.Append(Formatter.InlineCode(string.Join(' ', criteriaParse)));
            }

            effectsDescription.Add(effectDescriptionBuilder.ToString());
            effectDescriptionBuilder.Clear();
        }

        return effectsDescription;
    }

    private static List<string> GetCriteriaDescription(CriteriaReadOnlyCollection criteria, CultureInfo? culture, Func<string, string>? parametersDecorator = null)
    {
        List<string> criteriaDescription = [string.Empty];

        foreach (var element in criteria)
        {
            switch (element)
            {
                case CriteriaLogicalOperator logicalOperator:
                    criteriaDescription[^1] += logicalOperator.GetDescription(culture) + ' ';
                    break;
                case CriteriaReadOnlyCollection subCriteria:
                    var subCriteriaParse = GetCriteriaDescription(subCriteria, culture, parametersDecorator);

                    criteriaDescription[^1] += '(' + subCriteriaParse[0];
                    if (subCriteriaParse.Count > 1)
                    {
                        criteriaDescription.AddRange(subCriteriaParse.GetRange(1, subCriteriaParse.Count - 1));
                    }
                    criteriaDescription[^1] += ')';

                    criteriaDescription.Add(string.Empty);
                    break;
                case ICriterion criterion:
                    criteriaDescription[^1] += parametersDecorator is null
                        ? criterion.GetDescription(culture)
                        : criterion.GetDescription(culture).ToString(parametersDecorator);

                    criteriaDescription.Add(string.Empty);
                    break;
            }
        }

        if (string.IsNullOrEmpty(criteriaDescription[^1]))
        {
            criteriaDescription.RemoveAt(criteriaDescription.Count - 1);
        }

        return criteriaDescription;
    }
}
