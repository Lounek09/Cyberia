﻿using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class ServerData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

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
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        Language = string.Empty;
        RealLanguages = [];
    }

    public ServerPopulationData? GetServerPopulationData()
    {
        return DofusApi.Datacenter.ServersRepository.GetServerPopulationDataById(ServerPopulationId);
    }

    public ServerCommunityData? GetServerCommunityData()
    {
        return DofusApi.Datacenter.ServersRepository.GetServerCommunityDataById(ServerCommunityId);
    }
}
