using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

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
    public IReadOnlyList<string> RealLanguages { get; init; }

    [JsonConstructor]
    internal ServerData()
    {
        Name = string.Empty;
        Description = string.Empty;
        Language = string.Empty;
        RealLanguages = [];
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
