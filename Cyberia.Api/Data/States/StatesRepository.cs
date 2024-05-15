using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States;

public sealed class StatesRepository : IDofusRepository
{
    private const string c_fileName = "states.json";

    [JsonPropertyName("ST")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, StateData>))]
    public FrozenDictionary<int, StateData> States { get; init; }

    [JsonConstructor]
    internal StatesRepository()
    {
        States = FrozenDictionary<int, StateData>.Empty;
    }

    internal static StatesRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<StatesRepository>(filePath);
    }

    public StateData? GetStateDataById(int id)
    {
        States.TryGetValue(id, out var stateData);
        return stateData;
    }

    public string GetStateNameById(int id)
    {
        var stateData = GetStateDataById(id);

        return stateData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : stateData.Name;
    }
}
