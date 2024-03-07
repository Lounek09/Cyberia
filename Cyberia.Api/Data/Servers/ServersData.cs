using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class ServersData
    : IDofusData
{
    private const string FILE_NAME = "servers.json";

    [JsonPropertyName("SR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerData>))]
    public FrozenDictionary<int, ServerData> Servers { get; init; }

    [JsonPropertyName("SRP")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerPopulationData>))]
    public FrozenDictionary<int, ServerPopulationData> ServerPopulations { get; init; }

    [JsonPropertyName("SRPW")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerPopulationWeightData>))]
    internal FrozenDictionary<int, ServerPopulationWeightData> ServerPopulationsWeight { get; init; }

    [JsonPropertyName("SRC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerCommunityData>))]
    public FrozenDictionary<int, ServerCommunityData> ServerCommunities { get; init; }

    [JsonPropertyName("SRVT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DefaultServerSpecificTextData>))]
    public FrozenDictionary<int, DefaultServerSpecificTextData> DefaultServerSpecificTexts { get; init; }

    [JsonPropertyName("SRVC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<string, ServerSpecificTextData>))]
    public FrozenDictionary<string, ServerSpecificTextData> ServerSpecificTexts { get; init; }

    [JsonConstructor]
    internal ServersData()
    {
        Servers = FrozenDictionary<int, ServerData>.Empty;
        ServerPopulations = FrozenDictionary<int, ServerPopulationData>.Empty;
        ServerPopulationsWeight = FrozenDictionary<int, ServerPopulationWeightData>.Empty;
        ServerCommunities = FrozenDictionary<int, ServerCommunityData>.Empty;
        DefaultServerSpecificTexts = FrozenDictionary<int, DefaultServerSpecificTextData>.Empty;
        ServerSpecificTexts = FrozenDictionary<string, ServerSpecificTextData>.Empty;
    }

    internal static ServersData Load()
    {
        var data = Datacenter.LoadDataFromFile<ServersData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));

        foreach (var serverPopulationData in data.ServerPopulations.Values)
        {
            var serverPopulationWeightData = data.GetServerPopulationWeightDataById(serverPopulationData.Id);
            if (serverPopulationWeightData is not null)
            {
                serverPopulationData.Weight = serverPopulationWeightData.Weight;
            }
        }

        return data;
    }

    public ServerData? GetServerDataById(int id)
    {
        Servers.TryGetValue(id, out var serverData);
        return serverData;
    }

    public string GetServerNameById(int id)
    {
        var serverData = GetServerDataById(id);

        return serverData is null
            ? PatternDecoder.Description(Resources.Unknown_Data, id)
            : serverData.Name;
    }

    public ServerPopulationData? GetServerPopulationDataById(int id)
    {
        ServerPopulations.TryGetValue(id, out var serverPopulationData);
        return serverPopulationData;
    }

    internal ServerPopulationWeightData? GetServerPopulationWeightDataById(int id)
    {
        ServerPopulationsWeight.TryGetValue(id, out var serverPopulationWeightData);
        return serverPopulationWeightData;
    }

    public ServerCommunityData? GetServerCommunityDataById(int id)
    {
        ServerCommunities.TryGetValue(id, out var serverCommunityData);
        return serverCommunityData;
    }
}
