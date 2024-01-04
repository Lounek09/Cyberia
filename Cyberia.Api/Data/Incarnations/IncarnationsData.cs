using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Incarnations;

public sealed class IncarnationsData
    : IDofusData
{
    private const string FILE_NAME = "incarnation.json";

    [JsonPropertyName("INCA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, IncarnationData>))]
    public FrozenDictionary<int, IncarnationData> Incarnations { get; init; }

    [JsonConstructor]
    internal IncarnationsData()
    {
        Incarnations = FrozenDictionary<int, IncarnationData>.Empty;
    }

    internal static IncarnationsData Load()
    {
        return Datacenter.LoadDataFromFile<IncarnationsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public IncarnationData? GetIncarnationDataByItemId(int id)
    {
        Incarnations.TryGetValue(id, out var incarnationData);
        return incarnationData;
    }

    public IEnumerable<IncarnationData> GetIncarnationsDataByName(string name)
    {
        var names = name.NormalizeCustom().Split(' ');
        return Incarnations.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
    }
}
