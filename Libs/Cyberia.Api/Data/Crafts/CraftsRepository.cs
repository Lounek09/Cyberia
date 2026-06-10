using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Crafts;

public sealed class CraftsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "crafts.json";

    [JsonPropertyName("CR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, CraftData>))]
    public FrozenDictionary<int, CraftData> Crafts { get; init; }

    [JsonConstructor]
    internal CraftsRepository()
    {
        Crafts = FrozenDictionary<int, CraftData>.Empty;
    }

    public CraftData? GetCraftDataById(int id)
    {
        Crafts.TryGetValue(id, out var craftData);
        return craftData;
    }

    public IEnumerable<CraftData> GetCraftsDataByItemName(string itemName, Language language)
    {
        return GetCraftsDataByItemName(itemName, language.ToCulture());
    }

    public IEnumerable<CraftData> GetCraftsDataByItemName(string itemName, CultureInfo? culture = null)
    {
        var itemNames = itemName.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var craftData in Crafts.Values)
        {
            var itemData = craftData.GetItemData();
            if (itemData is not null &&
                itemNames.All(x => itemData.NormalizedName.ToString(culture).Contains(x, StringComparison.OrdinalIgnoreCase)))
            {
                yield return craftData;
            }
        }
    }
}
