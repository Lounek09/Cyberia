using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Servers;

public sealed class ServerPopulationData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public LocalizedString Name { get; init; }

    [JsonIgnore]
    public int Weight { get; internal set; }

    [JsonConstructor]
    internal ServerPopulationData()
    {
        Name = LocalizedString.Empty;
    }
}
