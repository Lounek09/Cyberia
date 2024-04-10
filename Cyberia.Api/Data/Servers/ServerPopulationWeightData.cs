using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

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
