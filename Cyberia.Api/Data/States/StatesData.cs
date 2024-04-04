using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States;

public sealed class StatesData
    : IDofusData
{
    private const string FILE_NAME = "states.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("ST")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, StateData>))]
    public FrozenDictionary<int, StateData> States { get; init; }

    [JsonConstructor]
    internal StatesData()
    {
        States = FrozenDictionary<int, StateData>.Empty;
    }

    internal static async Task<StatesData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<StatesData>(FILE_PATH);
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
