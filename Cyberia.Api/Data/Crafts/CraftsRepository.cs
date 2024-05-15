using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Crafts;

public sealed class CraftsRepository : IDofusRepository
{
    private const string c_fileName = "crafts.json";

    [JsonPropertyName("CR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, CraftData>))]
    public FrozenDictionary<int, CraftData> Crafts { get; init; }

    [JsonConstructor]
    internal CraftsRepository()
    {
        Crafts = FrozenDictionary<int, CraftData>.Empty;
    }

    internal static CraftsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<CraftsRepository>(filePath);
    }

    public CraftData? GetCraftDataById(int id)
    {
        Crafts.TryGetValue(id, out var craftData);
        return craftData;
    }

    public IEnumerable<CraftData> GetCraftsDataByItemName(string itemName)
    {
        var itemNames = itemName.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

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
