using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class ServersRepository : IDofusRepository
{
    private const string c_fileName = "servers.json";

    [JsonPropertyName("SR")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerData>))]
    public FrozenDictionary<int, ServerData> Servers { get; init; }

    [JsonPropertyName("SRP")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerPopulationData>))]
    public FrozenDictionary<int, ServerPopulationData> ServerPopulations { get; init; }

    [JsonPropertyName("SRPW")]
    [JsonInclude]
    internal IReadOnlyList<ServerPopulationWeightData> ServerPopulationsWeight { get; init; }

    [JsonPropertyName("SRC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, ServerCommunityData>))]
    public FrozenDictionary<int, ServerCommunityData> ServerCommunities { get; init; }

    [JsonPropertyName("SRVT")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DefaultServerSpecificTextData>))]
    public FrozenDictionary<int, DefaultServerSpecificTextData> DefaultServerSpecificTexts { get; init; }

    [JsonPropertyName("SRVC")]
    public IReadOnlyDictionary<string, string> ServerSpecificTexts { get; init; }

    [JsonConstructor]
    internal ServersRepository()
    {
        Servers = FrozenDictionary<int, ServerData>.Empty;
        ServerPopulations = FrozenDictionary<int, ServerPopulationData>.Empty;
        ServerPopulationsWeight = [];
        ServerCommunities = FrozenDictionary<int, ServerCommunityData>.Empty;
        DefaultServerSpecificTexts = FrozenDictionary<int, DefaultServerSpecificTextData>.Empty;
        ServerSpecificTexts = ReadOnlyDictionary<string, string>.Empty;
    }

    internal static ServersRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        var data = Datacenter.LoadRepository<ServersRepository>(filePath);

        foreach (var serverPopulationWeightData in data.ServerPopulationsWeight)
        {
            var serverPopulationData = data.GetServerPopulationDataById(serverPopulationWeightData.Id);
            if (serverPopulationData is not null)
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
            ? Translation.Format(ApiTranslations.Unknown_Data, id)
            : serverData.Name;
    }

    public ServerPopulationData? GetServerPopulationDataById(int id)
    {
        ServerPopulations.TryGetValue(id, out var serverPopulationData);
        return serverPopulationData;
    }

    public ServerCommunityData? GetServerCommunityDataById(int id)
    {
        ServerCommunities.TryGetValue(id, out var serverCommunityData);
        return serverCommunityData;
    }
}
