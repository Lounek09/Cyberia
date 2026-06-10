using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides.Localized;

internal sealed class RideAbilityLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal RideAbilityLocalizedData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
