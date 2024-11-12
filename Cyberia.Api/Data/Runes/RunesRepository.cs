using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Runes;

public sealed class RunesRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => "runes.json";

    [JsonPropertyName("RU")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RuneData>))]
    public FrozenDictionary<int, RuneData> Runes { get; init; }

    [JsonConstructor]
    internal RunesRepository()
    {
        Runes = FrozenDictionary<int, RuneData>.Empty;
    }

    public RuneData? GetRuneDataById(int id)
    {
        Runes.TryGetValue(id, out var runeData);
        return runeData;
    }

    public IEnumerable<RuneData> GetRunesDataByItemName(string itemName, Language language)
    {
        return GetRunesDataByItemName(itemName, language.ToCulture());
    }

    public IEnumerable<RuneData> GetRunesDataByItemName(string itemName, CultureInfo? culture = null)
    {
        var itemNames = itemName.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (var runeData in Runes.Values)
        {
            var itemData = runeData.GetBaRuneItemData();
            if (itemData is not null &&
                itemNames.All(x => itemData.NormalizedName.ToString(culture).Contains(x, StringComparison.OrdinalIgnoreCase)))
            {
                yield return runeData;
            }
        }
    }
}
