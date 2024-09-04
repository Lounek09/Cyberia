using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
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

    public IEnumerable<IncarnationData> GetIncarnationsDataByName(string name)
    {
        var names = name.NormalizeToAscii().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return Incarnations.Values.Where(x =>
        {
            return names.All(y =>
            {
                return ExtendString.NormalizeToAscii(x.Name).Contains(y, StringComparison.OrdinalIgnoreCase);
            });
        });
    }
}
