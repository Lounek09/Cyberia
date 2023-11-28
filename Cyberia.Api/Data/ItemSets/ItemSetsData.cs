using Cyberia.Api.Data.ItemSets.Custom;
using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.ItemSets;

public sealed class ItemSetsData : IDofusData
{
    private const string FILE_NAME = "itemsets.json";

    [JsonPropertyName("IS")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ItemSetData>))]
    public FrozenDictionary<int, ItemSetData> ItemSets { get; init; }

    [JsonConstructor]
    internal ItemSetsData()
    {
        ItemSets = FrozenDictionary<int, ItemSetData>.Empty;
    }

    internal static ItemSetsData Load()
    {
        var data = Datacenter.LoadDataFromFile<ItemSetsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        var customData = Datacenter.LoadDataFromFile<ItemSetsCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

        foreach (var itemSetCustomData in customData.ItemSetsCustom)
        {
            var itemSetData = data.GetItemSetDataById(itemSetCustomData.Id);
            if (itemSetData is not null)
            {
                itemSetData.Effects = itemSetCustomData.Effects.AsReadOnly();
            }
        }

        return data;
    }

    public ItemSetData? GetItemSetDataById(int id)
    {
        ItemSets.TryGetValue(id, out var itemSetData);
        return itemSetData;
    }

    public string GetItemSetNameById(int id)
    {
        var itemSetData = GetItemSetDataById(id);

        return itemSetData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : itemSetData.Name;
    }

    public IEnumerable<ItemSetData> GetItemSetsDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return ItemSets.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }
}
