using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Incarnations;

public sealed class IncarnationsData : IDofusData
{
    private const string c_fileName = "incarnation.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

    [JsonPropertyName("INCA")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, IncarnationData>))]
    public FrozenDictionary<int, IncarnationData> Incarnations { get; init; }

    [JsonConstructor]
    internal IncarnationsData()
    {
        Incarnations = FrozenDictionary<int, IncarnationData>.Empty;
    }

    internal static async Task<IncarnationsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<IncarnationsData>(s_filePath);
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
