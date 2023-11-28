using Cyberia.Api.Data.Audios;
using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Maps;

public sealed class MapSubAreaData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("a")]
    public int MapAreaId { get; init; }

    [JsonPropertyName("m")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int?>))]
    public ReadOnlyCollection<int?> FightAudioMusicId { get; init; }

    [JsonPropertyName("v")]
    [JsonConverter(typeof(ReadOnlyCollectionConverter<int>))]
    public ReadOnlyCollection<int> NearMapSubAreasId { get; init; }

    [JsonConstructor]
    internal MapSubAreaData()
    {
        Name = string.Empty;
        FightAudioMusicId = ReadOnlyCollection<int?>.Empty;
        NearMapSubAreasId = ReadOnlyCollection<int>.Empty;
    }

    public MapAreaData? GetMapAreaData()
    {
        return DofusApi.Datacenter.MapsData.GetMapAreaDataById(MapAreaId);
    }

    public IEnumerable<MapSubAreaData> GetNearMapSubAreasData()
    {
        foreach (var mapSubAreaId in NearMapSubAreasId)
        {
            var mapSubAreaData = DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(mapSubAreaId);
            if (mapSubAreaData is not null)
            {
                yield return mapSubAreaData;
            }
        }
    }

    public AudioMusicData? GetFightAudioMusicData()
    {
        return FightAudioMusicId[0] is null ? null : DofusApi.Datacenter.AudiosData.GetAudioMusicDataById(FightAudioMusicId[0]!.Value);
    }

    public IEnumerable<MapData> GetMapsData()
    {
        return DofusApi.Datacenter.MapsData.GetMapsDataByMapSubAreaId(Id);
    }
}
