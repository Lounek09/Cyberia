using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides;

public sealed class RideAbilityData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("e")]
    [JsonInclude]
    internal string E { get; init; }

    [JsonConstructor]
    internal RideAbilityData()
    {
        Name = string.Empty;
        Description = string.Empty;
        E = string.Empty;
    }
}
