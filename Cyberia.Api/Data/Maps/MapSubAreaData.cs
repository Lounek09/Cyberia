using Cyberia.Api.Data.Audios;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapSubAreaData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("a")]
    public int MapAreaId { get; init; }

    [JsonPropertyName("m")]
    public IReadOnlyList<int?> FightAudioMusicId { get; init; }

    [JsonPropertyName("v")]
    public IReadOnlyList<int> NearMapSubAreasId { get; init; }

    [JsonConstructor]
    internal MapSubAreaData()
    {
        Name = LocalizedString.Empty;
        FightAudioMusicId = [];
        NearMapSubAreasId = [];
    }

    public MapAreaData? GetMapAreaData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapAreaDataById(MapAreaId);
    }

    public IEnumerable<MapSubAreaData> GetNearMapSubAreasData()
    {
        foreach (var mapSubAreaId in NearMapSubAreasId)
        {
            var mapSubAreaData = DofusApi.Datacenter.MapsRepository.GetMapSubAreaDataById(mapSubAreaId);
            if (mapSubAreaData is not null)
            {
                yield return mapSubAreaData;
            }
        }
    }

    public AudioMusicData? GetFightAudioMusicData()
    {
        return FightAudioMusicId[0] is null ? null : DofusApi.Datacenter.AudiosRepository.GetAudioMusicDataById(FightAudioMusicId[0]!.Value);
    }

    public IEnumerable<MapData> GetMapsData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapsDataByMapSubAreaId(Id);
    }
}
