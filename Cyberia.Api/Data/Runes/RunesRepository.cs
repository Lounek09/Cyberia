using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
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

    public RuneData? GetRuneDataByName(string name)
    {
        name = name.NormalizeToAscii();

        return Runes.Values.FirstOrDefault(x =>
        {
            return ExtendString.NormalizeToAscii(x.Name).Equals(name, StringComparison.OrdinalIgnoreCase);
        });
    }

    public IEnumerable<RuneData> GetRunesDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Runes.Values.Where(x =>
        {
            return names.All(y =>
            {
                return ExtendString.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetAllRuneName()
    {
        return string.Join(", ", Runes.Values.Select(x => x.Name));
    }
}
