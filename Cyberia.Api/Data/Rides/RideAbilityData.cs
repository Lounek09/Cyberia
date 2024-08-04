using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides;

public sealed class RideAbilityData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonPropertyName("e")]
    [JsonInclude]
    internal string E { get; init; }

    [JsonConstructor]
    internal RideAbilityData()
    {
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
        E = string.Empty;
    }
}
