﻿using Cyberia.Api.Data.Maps;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses;

public sealed class HouseData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonIgnore]
    public int OutdoorMapId { get; internal set; }

    [JsonIgnore]
    public int RoomNumber { get; internal set; }

    [JsonIgnore]
    public int ChestNumber { get; internal set; }

    [JsonIgnore]
    public int Price { get; internal set; }

    [JsonConstructor]
    internal HouseData()
    {
        Name = LocalizedString.Empty;
        Description = LocalizedString.Empty;
    }

    public IEnumerable<MapData> GetMapsData()
    {
        foreach (var houseMapData in DofusApi.Datacenter.HousesRepository.GetHouseMapsDataByHouseId(Id))
        {
            var mapData = houseMapData.GetMapData();
            if (mapData is not null)
            {
                yield return mapData;
            }
        }
    }

    public MapData? GetOutdoorMapData()
    {
        return DofusApi.Datacenter.MapsRepository.GetMapDataById(OutdoorMapId);
    }

    public string GetCoordinate()
    {
        var mapData = GetOutdoorMapData();

        return mapData is null ? "[x, x]" : mapData.GetCoordinate();
    }
}
