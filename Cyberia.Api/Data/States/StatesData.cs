using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.States;

public sealed class StatesData
    : IDofusData
{
    private const string c_fileName = "states.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<StatesData>(s_filePath);
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
