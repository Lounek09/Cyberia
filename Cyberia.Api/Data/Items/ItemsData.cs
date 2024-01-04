using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemsData
    : IDofusData
{
    private const string FILE_NAME = "items.json";

    [JsonPropertyName("I.us")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemUnicStringData>))]
    public FrozenDictionary<int, ItemUnicStringData> ItemUnicStrings { get; init; }

    [JsonPropertyName("I.st")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSuperTypeData>))]
    public FrozenDictionary<int, ItemSuperTypeData> ItemSuperTypes { get; init; }

    [JsonPropertyName("I.ss")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSuperTypeSlotData>))]
    internal FrozenDictionary<int, ItemSuperTypeSlotData> ItemSuperTypeSlots { get; init; }

    [JsonPropertyName("I.t")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemTypeData>))]
    public FrozenDictionary<int, ItemTypeData> ItemTypes { get; init; }

    [JsonPropertyName("I.u")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemData>))]
    public FrozenDictionary<int, ItemData> Items { get; init; }

    [JsonConstructor]
    internal ItemsData()
    {
        ItemUnicStrings = FrozenDictionary<int, ItemUnicStringData>.Empty;
        ItemSuperTypes = FrozenDictionary<int, ItemSuperTypeData>.Empty;
        ItemSuperTypeSlots = FrozenDictionary<int, ItemSuperTypeSlotData>.Empty;
        ItemTypes = FrozenDictionary<int, ItemTypeData>.Empty;
        Items = FrozenDictionary<int, ItemData>.Empty;
    }

    internal static ItemsData Load()
    {
        var data = Datacenter.LoadDataFromFile<ItemsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));

        foreach (var itemSuperTypeSlotData in data.ItemSuperTypeSlots.Values)
        {
            var itemSuperTypeData = data.GetItemSuperTypeDataById(itemSuperTypeSlotData.Id);
            if (itemSuperTypeData is not null)
            {
                itemSuperTypeData.SlotsId = itemSuperTypeSlotData.SlotsId;
            }
        }

        return data;
    }

    public ItemSuperTypeData? GetItemSuperTypeDataById(int id)
    {
        ItemSuperTypes.TryGetValue(id, out var itemSuperTypeData);
        return itemSuperTypeData;
    }

    internal ItemSuperTypeSlotData? GetItemSuperTypeSlotDataById(int id)
    {
        ItemSuperTypeSlots.TryGetValue(id, out var itemSuperTypeSlotData);
        return itemSuperTypeSlotData;
    }

    public ItemTypeData? GetItemTypeDataById(int id)
    {
        ItemTypes.TryGetValue(id, out var itemTypeData);
        return itemTypeData;
    }

    public string GetItemTypeNameById(int id)
    {
        var itemTypeData = GetItemTypeDataById(id);

        return itemTypeData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : itemTypeData.Name;
    }

    public ItemData? GetItemDataById(int id)
    {
        Items.TryGetValue(id, out var itemData);
        return itemData;
    }

    public IEnumerable<ItemData> GetItemsData(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return Items.Values.Where(x => names.All(x.NormalizedName.Contains));
    }

    public string GetItemNameById(int id)
    {
        var itemData = GetItemDataById(id);

        return itemData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : itemData.Name;
    }
}
