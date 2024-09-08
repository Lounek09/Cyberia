using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps.Localized;

internal sealed class MapsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => MapsRepository.FileName;

    [JsonPropertyName("MA.sua")]
    public IReadOnlyList<MapSuperAreaLocalizedData> MapSuperAreas { get; init; }

    [JsonPropertyName("MA.a")]
    public IReadOnlyList<MapAreaLocalizedData> MapAreas { get; init; }

    [JsonPropertyName("MA.sa")]
    public IReadOnlyList<MapSubAreaLocalizedData> MapSubAreas { get; init; }

    [JsonConstructor]
    internal MapsLocalizedRepository()
    {
        MapSuperAreas = [];
        MapAreas = [];
        MapSubAreas = [];
    }
}
