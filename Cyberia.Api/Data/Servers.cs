using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data;

public sealed class ServerData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("l")]
    public string Language { get; init; }

    [JsonPropertyName("p")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int ServerPopulationId { get; init; }

    [JsonPropertyName("t")]
    public int Type { get; init; }

    [JsonPropertyName("c")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int ServerCommunityId { get; init; }

    [JsonPropertyName("date")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public long Date { get; init; }

    [JsonPropertyName("rlng")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<string>))]
    public ReadOnlyCollection<string> RealLanguages { get; init; }

    [JsonConstructor]
    internal ServerData()
    {
        Name = string.Empty;
        Description = string.Empty;
        Language = string.Empty;
        RealLanguages = ReadOnlyCollection<string>.Empty;
    }

    public ServerPopulationData? GetServerPopulationData()
    {
        return DofusApi.Datacenter.ServersData.GetServerPopulationDataById(ServerPopulationId);
    }

    public ServerCommunityData? GetServerCommunityData()
    {
        return DofusApi.Datacenter.ServersData.GetServerCommunityDataById(ServerCommunityId);
    }
}

public sealed class ServerPopulationData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonIgnore]
    public int Weight { get; internal set; }

    [JsonConstructor]
    internal ServerPopulationData()
    {
        Name = string.Empty;
    }
}

internal sealed class ServerPopulationWeightData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public int Weight { get; init; }

    [JsonConstructor]
    internal ServerPopulationWeightData()
    {

    }
}

public sealed class ServerCommunityData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public bool Visible { get; init; }

    [JsonPropertyName("i")]
    [JsonInclude]
    internal int Id2 { get; init; }

    [JsonPropertyName("c")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<string>))]
    public ReadOnlyCollection<string> Countries { get; init; }

    [JsonConstructor]
    internal ServerCommunityData()
    {
        Name = string.Empty;
        Countries = ReadOnlyCollection<string>.Empty;
    }
}

public sealed class DefaultServerSpecificTextData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("l")]
    public string Label { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal DefaultServerSpecificTextData()
    {
        Label = string.Empty;
        Description = string.Empty;
    }
}

public sealed class ServerSpecificTextData : IDofusData<string>
{
    [JsonPropertyName("id")]
    public string Id { get; init; }

    [JsonPropertyName("v")]
    public string Description { get; init; }

    [JsonConstructor]
    internal ServerSpecificTextData()
    {
        Id = string.Empty;
        Description = string.Empty;
    }
}

public sealed class ServersData : IDofusData
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

        return serverData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : serverData.Name;
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
