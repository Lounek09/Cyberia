using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps.Localized;

internal sealed class MapSuperAreaLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal MapSuperAreaLocalizedData()
    {
        Name = string.Empty;
    }
}
