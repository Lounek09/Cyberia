using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Crafts;

public sealed class CraftsData
    : IDofusData
{
    private const string FILE_NAME = "crafts.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("CR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, CraftData>))]
    public FrozenDictionary<int, CraftData> Crafts { get; init; }

    [JsonConstructor]
    internal CraftsData()
    {
        Crafts = FrozenDictionary<int, CraftData>.Empty;
    }

    internal static async Task<CraftsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<CraftsData>(FILE_PATH);
    }

    public CraftData? GetCraftDataById(int id)
    {
        Crafts.TryGetValue(id, out var craftData);
        return craftData;
    }

    public IEnumerable<CraftData> GetCraftsDataByItemName(string itemName)
    {
        var itemNames = itemName.NormalizeCustom().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var craftData in Crafts.Values)
        {
            var itemData = craftData.GetItemData();
            if (itemData is not null &&
                itemNames.All(x => itemData.NormalizedName.Contains(x, StringComparison.OrdinalIgnoreCase)))
            {
                yield return craftData;
            }
        }
    }
}
