using Cyberia.Api;
using Cyberia.Api.Data;
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
using Cyberia.Api.Utils;
using Cyberia.Salamandra.Enums;
using Cyberia.Salamandra.Extensions.DSharpPlus;

using DSharpPlus;
using DSharpPlus.Entities;

using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace Cyberia.Salamandra.Services;

/// <summary>
/// Represents a service to build Discord embeds.
/// </summary>
public interface IEmbedBuilderService
{
    /// <summary>
    /// Creates a new embed builder with the specified category and author text.
    /// </summary>
    /// <param name="category">The embed category.</param>
    /// <param name="authorText">The author text.</param>
    /// <param name="culture">The culture to use for the date and time.</param>
    /// <returns>The created embed builder.</returns>
    DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText, CultureInfo? culture);

    /// <summary>
    /// Adds effect fields to the embed builder.
    /// </summary>
    /// <param name="embed">The embed builder to add the fields to.</param>
    /// <param name="name">The name of the field.</param>
    /// <param name="effects">The effects to display.</param>
    /// <param name="sort">Wether to sort the effects.</param>
    /// <param name="culture">The culture to use for the description.</param>
    /// <param name="inline">Whether to display the fields inline.</param>
    /// <returns>The updated embed builder.</returns>
    DiscordEmbedBuilder AddEffectFields(DiscordEmbedBuilder embed, string name, IEnumerable<IEffect> effects, bool sort, CultureInfo? culture, bool inline = false);

    /// <summary>
    /// Adds criteria fields to the embed builder.
    /// </summary>
    /// <param name="embed">The embed builder to add the fields to.</param>
    /// <param name="criteria">The criteria to display.</param>
    /// <param name="culture">The culture to use for the description.</param>
    /// <param name="inline">Whether to display the fields inline.</param>
    /// <returns>The updated embed builder.</returns>
    DiscordEmbedBuilder AddCriteriaFields(DiscordEmbedBuilder embed, CriteriaReadOnlyCollection criteria, CultureInfo? culture, bool inline = false);

    /// <summary>
    /// Adds quest objectives fields to the embed builder.
    /// </summary>
    /// <param name="embed">The embed builder to add the fields to.</param>
    /// <param name="questObjectives">The quest objectives to display.</param>
    /// <param name="culture">The culture to use for the description.</param>
    /// <param name="inline">Whether to display the fields inline.</param>
    /// <returns>The updated embed builder.</returns>
    DiscordEmbedBuilder AddQuestObjectivesFields(DiscordEmbedBuilder embed, IEnumerable<IQuestObjective> questObjectives, CultureInfo? culture, bool inline = false);

    /// <summary>
    /// Sets the craft description for the embed builder.
    /// </summary>
    /// <param name="embed">The embed builder to set the description for.</param>
    /// <param name="craftData">The craft data to display.</param>
    /// <param name="quantity">The quantity of the item to craft.</param>
    /// <param name="recursive">Whether to include sub-crafts.</param>
    /// <param name="culture">The culture to use for the description.</param>
    /// <returns>The updated embed builder.</returns>
    DiscordEmbedBuilder SetCraftDescription(DiscordEmbedBuilder embed, CraftData craftData, int quantity, bool recursive, CultureInfo? culture);

    /// <summary>
    /// Adds a craft field to the embed builder.
    /// </summary>
    /// <param name="embed">The embed builder to add the field to.</param>
    /// <param name="craftData">The craft data to display.</param>
    /// <param name="quantity">The quantity of the item to craft.</param>
    /// <param name="culture">The culture to use for the description.</param>
    /// <param name="inline">Whether to display the field inline.</param>
    /// <returns>The updated embed builder.</returns>
    DiscordEmbedBuilder AddCraftField(DiscordEmbedBuilder embed, CraftData craftData, int quantity, CultureInfo? culture, bool inline = false);

    /// <summary>
    /// Adds a weapon info field to the embed builder.
    /// </summary>
    /// <param name="embed">The embed builder to add the field to.</param>
    /// <param name="itemWeaponData">The weapon data to display.</param>
    /// <param name="twoHanded">Whether the weapon is two-handed.</param>
    /// <param name="itemTypeData">The item type data of the weapon.</param>
    /// <param name="culture">The culture to use for the description.</param>
    /// <param name="inline"></param>
    /// <returns>The updated embed builder.</returns>
    DiscordEmbedBuilder AddWeaponInfosField(DiscordEmbedBuilder embed, ItemWeaponData itemWeaponData, bool twoHanded, ItemTypeData? itemTypeData, CultureInfo? culture, bool inline = false);

    /// <summary>
    /// Adds a pet field to the embed builder.
    /// </summary>
    /// <param name="embed">The embed builder to add the field to.</param>
    /// <param name="petData">The pet data to display.</param>
    /// <param name="culture">The culture to use for the description.</param>
    /// <param name="inline">Whether to display the field inline.</param>
    /// <returns>The updated embed builder.</returns>
    DiscordEmbedBuilder AddPetField(DiscordEmbedBuilder embed, PetData petData, CultureInfo? culture, bool inline = false);
}

public sealed class EmbedBuilderService : IEmbedBuilderService
{
    private readonly DofusDatacenter _dofusDatacenter;
    private readonly string _username;
    private readonly string _baseIconUrl;
    private readonly string _footerIconUrl;
    private readonly DiscordColor _embedColor;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmbedBuilderService"/> class.
    /// </summary>
    /// <param name="config">The bot configuration.</param>
    /// <param name="client">The Discord client.</param>
    public EmbedBuilderService(BotConfig config, DiscordClient client, DofusApiConfig dofusApiConfig, DofusDatacenter dofusDatacenter)
    {
        _dofusDatacenter = dofusDatacenter;
        _username = client.CurrentUser.Username;
        _baseIconUrl = $"{dofusApiConfig.CdnUrl}/images/discord/embed_categories";
        _footerIconUrl = $"{dofusApiConfig.CdnUrl}/images/discord/mini-salamandra.png";
        _embedColor = new(config.EmbedColor);
    }

    public DiscordEmbedBuilder CreateEmbedBuilder(EmbedCategory category, string authorText, CultureInfo? culture)
    {
        var formattedDate = DateTime.Now.ToInGameDateTime().ToLongRolePlayString(culture);

        return new DiscordEmbedBuilder()
            .WithColor(_embedColor)
            .WithAuthor(authorText, iconUrl: GetIconUrl(category))
            .WithFooter($"{_username} • {formattedDate}", _footerIconUrl);
    }

    public DiscordEmbedBuilder AddEffectFields(DiscordEmbedBuilder embed, string name, IEnumerable<IEffect> effects, bool sort, CultureInfo? culture, bool inline = false)
    {
        if (effects.Any())
        {
            var effectsParse = GetEffectsDescription(effects, sort, culture, x => Formatter.Bold(Formatter.Sanitize(x)));
            return embed.AddFields(name, effectsParse, inline);
        }

        return embed.AddField(name, Translation.Get<BotTranslations>("Embed.Field.Effects.Content.None", culture), inline);
    }

    public DiscordEmbedBuilder AddCriteriaFields(DiscordEmbedBuilder embed, CriteriaReadOnlyCollection criteria, CultureInfo? culture, bool inline = false)
    {
        var criteriaParse = GetCriteriaDescription(criteria, culture, x => Formatter.Bold(Formatter.Sanitize(x)));
        return embed.AddFields(Translation.Get<BotTranslations>("Embed.Field.Criteria.Title", culture), criteriaParse, inline);
    }

    public DiscordEmbedBuilder AddQuestObjectivesFields(DiscordEmbedBuilder embed, IEnumerable<IQuestObjective> questObjectives, CultureInfo? culture, bool inline = false)
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

    public DiscordEmbedBuilder SetCraftDescription(DiscordEmbedBuilder embed, CraftData craftData, int quantity, bool recursive, CultureInfo? culture)
    {
        StringBuilder builder = new();

        var ingredients = recursive ? craftData.GetIngredientsWithSubCraft(quantity) : craftData.GetIngredients(quantity);
        foreach (var ingredient in ingredients)
        {
            var quantityFormatted = Formatter.Bold(ingredient.Value.ToFormattedString(culture));
            var itemName = Formatter.Sanitize(ingredient.Key.Name.ToString(culture));

            if (!recursive)
            {
                var subCraftData = _dofusDatacenter.CraftsRepository.GetCraftDataById(ingredient.Key.Id);
                if (subCraftData is not null)
                {
                    itemName = Formatter.Bold(itemName);
                }
            }

            builder.Append(quantityFormatted);
            builder.Append("x ");
            builder.Append(itemName);
            builder.Append('\n');
        }

        builder.Remove(builder.Length - 1, 1);

        return embed.WithDescription(builder.ToString());
    }

    public DiscordEmbedBuilder AddCraftField(DiscordEmbedBuilder embed, CraftData craftData, int quantity, CultureInfo? culture, bool inline = false)
    {
        StringBuilder builder = new();

        foreach (var ingredient in craftData.GetIngredients(quantity))
        {
            var quantityFormatted = Formatter.Bold(ingredient.Value.ToFormattedString(culture));
            var itemName = Formatter.Sanitize(ingredient.Key.Name.ToString(culture));

            builder.Append(quantityFormatted);
            builder.Append("x ");
            builder.Append(itemName);
            builder.Append(" + ");
        }

        builder.Remove(builder.Length - 3, 3);

        return embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Craft.Title", culture), builder.ToString(), inline);
    }

    public DiscordEmbedBuilder AddWeaponInfosField(DiscordEmbedBuilder embed, ItemWeaponData itemWeaponData, bool twoHanded, ItemTypeData? itemTypeData, CultureInfo? culture, bool inline = false)
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
            builder.Append(Emojis.EffectArea(itemTypeData.EffectArea, culture));
            builder.Append(' ');
            builder.Append(itemTypeData.EffectArea.GetDescription(culture).ToString(Formatter.Bold));
        }

        return embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Weapon.Title", culture), builder.ToString(), inline);
    }

    public DiscordEmbedBuilder AddPetField(DiscordEmbedBuilder embed, PetData petData, CultureInfo? culture, bool inline = false)
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
            builder.Append(Emojis.Effect(petFoodsData.Effect, culture));
            builder.Append(' ');
            builder.Append(Formatter.Bold(petFoodsData.Effect.GetDescription(culture)));
            builder.Append(" :\n");

            if (petFoodsData.ItemsId.Count > 0)
            {
                var itemsName = petFoodsData.ItemsId
                    .Select(x => _dofusDatacenter.ItemsRepository.GetItemNameById(x, culture));

                builder.Append(string.Join(", ", itemsName));
                builder.Append('\n');
            }

            if (petFoodsData.ItemTypesId.Count > 0)
            {
                var itemTypesName = petFoodsData.ItemTypesId
                    .Select(x => _dofusDatacenter.ItemsRepository.GetItemTypeNameById(x, culture));

                builder.Append(string.Join(", ", itemTypesName));
                builder.Append('\n');
            }

            if (petFoodsData.MonstersIdQuantities.Count > 0)
            {
                foreach (var group in petFoodsData.MonstersIdQuantities.GroupBy(x => x.Value))
                {
                    var monstersName = group
                        .Select(x => _dofusDatacenter.MonstersRepository.GetMonsterNameById(x.Key, culture));

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
                        .Select(x => _dofusDatacenter.MonstersRepository.GetMonsterRaceNameById(x.Key, culture));

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
                        .Select(x => _dofusDatacenter.MonstersRepository.GetMonsterSuperRaceNameById(x.Key, culture));

                    builder.Append(Formatter.Bold(group.Key.ToString()));
                    builder.Append("x ");
                    builder.Append(string.Join(", ", monsterSuperRacesName));
                    builder.Append('\n');

                }
            }
        }

        return embed.AddField(Translation.Get<BotTranslations>("Embed.Field.Pet.Title", culture), builder.ToString(), inline);
    }

    private static ReadOnlyCollection<string> GetEffectsDescription(IEnumerable<IEffect> effects, bool sort, CultureInfo? culture, Func<string, string>? parametersDecorator = null)
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

            effectDescriptionBuilder.Append(Emojis.Effect(effect, culture));
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
                effectDescriptionBuilder.Append(Emojis.Dispellable(culture));
            }

            if (effect.EffectArea.Size != EffectAreaFactory.Default.Size)
            {
                effectDescriptionBuilder.Append(" - ");
                effectDescriptionBuilder.Append(Emojis.EffectArea(effect.EffectArea, culture));
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

        return effectsDescription.AsReadOnly();
    }

    private static ReadOnlyCollection<string> GetCriteriaDescription(CriteriaReadOnlyCollection criteria, CultureInfo? culture, Func<string, string>? parametersDecorator = null)
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
                        criteriaDescription.AddRange(subCriteriaParse.Skip(1));
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

        return criteriaDescription.AsReadOnly();
    }

    /// <summary>
    /// Gets the icon URL for the specified category.
    /// </summary>
    /// <param name="category">The embed category.</param>
    /// <returns>The icon URL.</returns>
    private string GetIconUrl(EmbedCategory category)
    {
        return $"{_baseIconUrl}/{(int)category}.png";
    }
}
