using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Incarnations;

public sealed class IncarnationsRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => "incarnation.json";

    [JsonPropertyName("INCA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, IncarnationData>))]
    public FrozenDictionary<int, IncarnationData> Incarnations { get; init; }

    [JsonConstructor]
    internal IncarnationsRepository()
    {
        Incarnations = FrozenDictionary<int, IncarnationData>.Empty;
    }

    public IncarnationData? GetIncarnationDataByItemId(int id)
    {
        Incarnations.TryGetValue(id, out var incarnationData);
        return incarnationData;
    }

    public IEnumerable<IncarnationData> GetIncarnationsDataByItemName(string itemName, Language language)
    {
        return GetIncarnationsDataByItemName(itemName, language.ToCulture());
    }

    public IEnumerable<IncarnationData> GetIncarnationsDataByItemName(string itemName, CultureInfo? culture = null)
    {
        var itemNames = itemName.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var incarnationData in Incarnations.Values)
        {
            var itemData = incarnationData.GetItemData();
            if (itemData is not null &&
                itemNames.All(x => itemData.NormalizedName.ToString(culture).Contains(x, StringComparison.OrdinalIgnoreCase)))
            {
                yield return incarnationData;
            }
        }
    }
}
