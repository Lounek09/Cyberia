﻿using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Items;

public sealed class ItemsRepository : IDofusRepository
{
    private const string c_fileName = "items.json";

    [JsonPropertyName("I.us")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemUnicStringData>))]
    public FrozenDictionary<int, ItemUnicStringData> ItemUnicStrings { get; init; }

    [JsonPropertyName("I.st")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSuperTypeData>))]
    public FrozenDictionary<int, ItemSuperTypeData> ItemSuperTypes { get; init; }

    [JsonPropertyName("I.ss")]
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

    internal static ItemsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        var data = Datacenter.LoadRepository<ItemsRepository>(filePath);

        foreach (var itemSuperTypeSlotData in data.ItemSuperTypeSlots)
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

    public ItemTypeData? GetItemTypeDataById(int id)
    {
        ItemTypes.TryGetValue(id, out var itemTypeData);
        return itemTypeData;
    }

    public string GetItemTypeNameById(int id)
    {
        var itemTypeData = GetItemTypeDataById(id);

        return itemTypeData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
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
                return x.NormalizedName.Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetItemNameById(int id)
    {
        var itemData = GetItemDataById(id);

        return itemData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : itemData.Name;
    }
}