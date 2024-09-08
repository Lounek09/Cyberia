using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Rides.Localized;

internal sealed class RideLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal RideLocalizedData()
    {
        Name = string.Empty;
    }
}
