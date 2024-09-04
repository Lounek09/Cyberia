using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
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
        ItemSuperTypeSlots = [];
        ItemTypes = FrozenDictionary<int, ItemTypeData>.Empty;
        Items = FrozenDictionary<int, ItemData>.Empty;
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

    public string GetItemTypeNameById(int id)
    {
        var itemTypeData = GetItemTypeDataById(id);

        return itemTypeData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : itemTypeData.Name;
    }

    public ItemData? GetItemDataById(int id)
    {
        Items.TryGetValue(id, out var itemData);
        return itemData;
    }

    public IEnumerable<ItemData> GetItemsDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Items.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.NormalizedName.ToString().Contains(y, StringComparison.OrdinalIgnoreCase);
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

    public string GetItemNameById(int id)
    {
        var itemData = GetItemDataById(id);

        return itemData is null
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : itemData.Name;
    }

    protected override void LoadCustomData()
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
