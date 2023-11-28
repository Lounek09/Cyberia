using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Runes;

public sealed class RunesData : IDofusData
{
    private const string FILE_NAME = "runes.json";

    [JsonPropertyName("RU")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RuneData>))]
    public FrozenDictionary<int, RuneData> Runes { get; init; }

    [JsonConstructor]
    internal RunesData()
    {
        Runes = FrozenDictionary<int, RuneData>.Empty;
    }

    internal static RunesData Load()
    {
        return Datacenter.LoadDataFromFile<RunesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public RuneData? GetRuneDataById(int id)
    {
        Runes.TryGetValue(id, out var runeData);
        return runeData;
    }

    public RuneData? GetRuneDataByName(string name)
    {
        return Runes.Values.FirstOrDefault(x => x.Name.NormalizeCustom().Equals(name.NormalizeCustom()));
    }

    public IEnumerable<RuneData> GetRunesDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return Runes.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }

    public string GetAllRuneName()
    {
        return string.Join(", ", Runes.Values.Select(x => x.Name));
    }
}
