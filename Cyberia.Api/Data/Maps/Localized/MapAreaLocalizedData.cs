using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps.Localized;

internal sealed class MapAreaLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal MapAreaLocalizedData()
    {
        Name = string.Empty;
    }
}
