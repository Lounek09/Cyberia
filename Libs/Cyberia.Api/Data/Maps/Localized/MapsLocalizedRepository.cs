using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps.Localized;

internal sealed class MapsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => MapsRepository.FileName;

    [JsonPropertyName("MA.m")]
    public IReadOnlyList<MapLocalizedData> Maps { get; init; }

    [JsonPropertyName("MA.sua")]
    public IReadOnlyList<MapSuperAreaLocalizedData> MapSuperAreas { get; init; }

    [JsonPropertyName("MA.a")]
    public IReadOnlyList<MapAreaLocalizedData> MapAreas { get; init; }

    [JsonPropertyName("MA.sa")]
    public IReadOnlyList<MapSubAreaLocalizedData> MapSubAreas { get; init; }

    [JsonConstructor]
    internal MapsLocalizedRepository()
    {
        Maps = ReadOnlyCollection<MapLocalizedData>.Empty;
        MapSuperAreas = ReadOnlyCollection<MapSuperAreaLocalizedData>.Empty;
        MapAreas = ReadOnlyCollection<MapAreaLocalizedData>.Empty;
        MapSubAreas = ReadOnlyCollection<MapSubAreaLocalizedData>.Empty;
    }
}
