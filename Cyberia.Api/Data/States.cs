using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class StateData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("p")]
    [JsonInclude]
    internal int P { get; init; }

    [JsonConstructor]
    internal StateData()
    {
        Name = string.Empty;
    }

    public string GetImagePath()
    {
        return $"{DofusApi.Config.CdnUrl}/images/states/{Id}.png";
    }
}

public sealed class StatesData : IDofusData
{
    private const string FILE_NAME = "states.json";

    [JsonPropertyName("ST")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, StateData>))]
    public FrozenDictionary<int, StateData> States { get; init; }

    [JsonConstructor]
    internal StatesData()
    {
        States = FrozenDictionary<int, StateData>.Empty;
    }

    internal static StatesData Load()
    {
        return Datacenter.LoadDataFromFile<StatesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public StateData? GetStateDataById(int id)
    {
        States.TryGetValue(id, out var stateData);
        return stateData;
    }

    public string GetStateNameById(int id)
    {
        var stateData = GetStateDataById(id);

        return stateData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : stateData.Name;
    }
}
