using Cyberia.Api.Data.Items.Localized;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "items.json";

    [JsonPropertyName("I.us")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemUnicStringData>))]
    public FrozenDictionary<int, ItemUnicStringData> ItemUnicStrings { get; init; }

    [JsonPropertyName("I.st")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSuperTypeData>))]
    public FrozenDictionary<int, ItemSuperTypeData> ItemSuperTypes { get; init; }

    [JsonPropertyName("I.ss")]
    [JsonInclude]
    internal IReadOnlyList<ItemSuperTypeSlotData> ItemSuperTypeSlots { get; init; }

    [JsonPropertyName("I.t")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemTypeData>))]
    public FrozenDictionary<int, ItemTypeData> ItemTypes { get; init; }

    [JsonPropertyName("I.u")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemData>))]
    public FrozenDictionary<int, ItemData> Items { get; init; }

    [JsonConstructor]
    internal ItemsRepository()
    {
        ItemUnicStrings = FrozenDictionary<int, ItemUnicStringData>.Empty;
        ItemSuperTypes = FrozenDictionary<int, ItemSuperTypeData>.Empty;
        ItemSuperTypeSlots = ReadOnlyCollection<ItemSuperTypeSlotData>.Empty;
        ItemTypes = FrozenDictionary<int, ItemTypeData>.Empty;
        Items = FrozenDictionary<int, ItemData>.Empty;
    }

    public ItemUnicStringData? GetItemUnicStringDataById(int id)
    {
        ItemUnicStrings.TryGetValue(id, out var itemUnicStringData);
        return itemUnicStringData;
    }

    public ItemSuperTypeData? GetItemSuperTypeDataById(int id)
    {
        ItemSuperTypes.TryGetValue(id, out var itemSuperTypeData);
        return itemSuperTypeData;
    }

    public ItemTypeData? GetItemTypeDataById(int id)
    {
        ItemTypes.TryGetValue(id, out var itemTypeData);
        return itemTypeData;
    }

    public string GetItemTypeNameById(int id, Language language)
    {
        return GetItemTypeNameById(id, language.ToCulture());
    }

    public string GetItemTypeNameById(int id, CultureInfo? culture = null)
    {
        var itemTypeData = GetItemTypeDataById(id);

        return itemTypeData is null
            ? Translation.UnknownData(id, culture)
            : itemTypeData.Name.ToString(culture);
    }

    public ItemData? GetItemDataById(int id)
    {
        Items.TryGetValue(id, out var itemData);
        return itemData;
    }

    public IEnumerable<ItemData> GetItemsDataByName(string typeName, Language language)
    {
        return GetItemsDataByName(typeName, language.ToCulture());
    }

    public IEnumerable<ItemData> GetItemsDataByName(string name, CultureInfo? culture = null)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Items.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.NormalizedName.ToString(culture).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public IEnumerable<ItemData> GetItemsDataWithEffectId(int effectId)
    {
        return Items.Values.Where(x =>
        {
            var itemStatsData = x.GetItemStatsData();

            return itemStatsData is not null &&
                itemStatsData.Effects.Any(y => y.Id == effectId);
        });
    }

    public IEnumerable<ItemData> GetItemsDataWithCriterionId(string criterionId)
    {
        return Items.Values.Where(x =>
        {
            return x.Criteria.OfType<ICriterion>()
                .Any(y => y.Id.Equals(criterionId));
        });
    }

    public string GetItemNameById(int id, Language language)
    {
        return GetItemNameById(id, language.ToCulture());
    }

    public string GetItemNameById(int id, CultureInfo? culture = null)
    {
        var itemData = GetItemDataById(id);

        return itemData is null
            ? Translation.UnknownData(id, culture)
            : itemData.Name.ToString(culture);
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<ItemsLocalizedRepository>(identifier);

        foreach (var itemUnicStringLocalizedData in localizedRepository.ItemUnicStrings)
        {
            var itemUnicStringData = GetItemUnicStringDataById(itemUnicStringLocalizedData.Id);
            itemUnicStringData?.Value.TryAdd(twoLetterISOLanguageName, itemUnicStringLocalizedData.Value);
        }

        foreach (var itemTypeLocalizedData in localizedRepository.ItemTypes)
        {
            var itemTypeData = GetItemTypeDataById(itemTypeLocalizedData.Id);
            itemTypeData?.Name.TryAdd(twoLetterISOLanguageName, itemTypeLocalizedData.Name);
        }

        foreach (var itemLocalizedData in localizedRepository.Items)
        {
            var itemData = GetItemDataById(itemLocalizedData.Id);
            if (itemData is not null)
            {
                itemData.Name.TryAdd(twoLetterISOLanguageName, itemLocalizedData.Name);
                itemData.NormalizedName.TryAdd(twoLetterISOLanguageName, itemLocalizedData.NormalizedName);
                itemData.Description.TryAdd(twoLetterISOLanguageName, itemLocalizedData.Description);
            }
        }
    }

    protected override void FinalizeLoading()
    {
        foreach (var itemSuperTypeSlotData in ItemSuperTypeSlots)
        {
            var itemSuperTypeData = GetItemSuperTypeDataById(itemSuperTypeSlotData.Id);
            if (itemSuperTypeData is not null)
            {
                itemSuperTypeData.SlotsId = itemSuperTypeSlotData.SlotsId;
            }
        }
    }
}
