using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses.Localized;

internal sealed class HouseLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal HouseLocalizedData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
