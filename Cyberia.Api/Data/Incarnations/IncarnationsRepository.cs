using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Incarnations;

public sealed class IncarnationsRepository : IDofusRepository
{
    private const string c_fileName = "incarnation.json";

    [JsonPropertyName("INCA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, IncarnationData>))]
    public FrozenDictionary<int, IncarnationData> Incarnations { get; init; }

    [JsonConstructor]
    internal IncarnationsRepository()
    {
        Incarnations = FrozenDictionary<int, IncarnationData>.Empty;
    }

    internal static IncarnationsRepository Load(string _)
    {
        var customFilePath = Path.Join(DofusApi.CustomPath, c_fileName);

        return Datacenter.LoadRepository<IncarnationsRepository>(customFilePath);
    }

    public IncarnationData? GetIncarnationDataByItemId(int id)
    {
        Incarnations.TryGetValue(id, out var incarnationData);
        return incarnationData;
    }

    public IEnumerable<IncarnationData> GetIncarnationsDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Incarnations.Values.Where(x =>
        {
            return names.All(y =>
            {
                return x.Name.NormalizeToAscii().Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }
}
