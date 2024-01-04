using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Crafts;

public sealed class CraftsData
    : IDofusData
{
    private const string FILE_NAME = "crafts.json";

    [JsonPropertyName("CR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, CraftData>))]
    public FrozenDictionary<int, CraftData> Crafts { get; init; }

    [JsonConstructor]
    internal CraftsData()
    {
        Crafts = FrozenDictionary<int, CraftData>.Empty;
    }

    internal static CraftsData Load()
    {
        return Datacenter.LoadDataFromFile<CraftsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public CraftData? GetCraftDataById(int id)
    {
        Crafts.TryGetValue(id, out var craftData);
        return craftData;
    }

    public IEnumerable<CraftData> GetCraftsDataByItemName(string itemName)
    {
        var itemNames = itemName.NormalizeCustom().Split(' ');
        foreach (var craftData in Crafts.Values)
        {
            var itemData = craftData.GetItemData();
            if (itemData is not null && itemNames.All(itemData.NormalizedName.Contains))
            {
                yield return craftData;
            }
        }
    }
}
