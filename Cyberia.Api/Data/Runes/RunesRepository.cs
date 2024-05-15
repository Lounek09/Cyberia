using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Runes;

public sealed class RunesRepository : IDofusRepository
{
    private const string c_fileName = "runes.json";

    [JsonPropertyName("RU")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, RuneData>))]
    public FrozenDictionary<int, RuneData> Runes { get; init; }

    [JsonConstructor]
    internal RunesRepository()
    {
        Runes = FrozenDictionary<int, RuneData>.Empty;
    }

    internal static RunesRepository Load(string _)
    {
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        return Datacenter.LoadRepository<RunesRepository>(customFilePath);
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
            return x.Name.NormalizeToAscii().Equals(name, StringComparison.OrdinalIgnoreCase);
        });
    }

    public IEnumerable<RuneData> GetRunesDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Runes.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }

    public string GetAllRuneName()
    {
        return string.Join(", ", Runes.Values.Select(x => x.Name));
    }
}
