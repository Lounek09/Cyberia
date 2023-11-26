using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class MapData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("x")]
        public int XCoord { get; init; }

        [JsonPropertyName("y")]
        public int YCoord { get; init; }

        [JsonPropertyName("sa")]
        public int MapSubAreaId { get; init; }

        [JsonPropertyName("p1")]
        public string Placement1 { get; init; }

        [JsonPropertyName("p2")]
        public string Placement2 { get; init; }

        [JsonPropertyName("p")]
        public List<List<object>> Parameters { get; init; }

        [JsonPropertyName("d")]
        public int DungeonId { get; init; }

        [JsonPropertyName("c")]
        public int MaxPlayerPerFight { get; init; }

        [JsonPropertyName("t")]
        public int MaxPlayerPerTeam { get; init; }

        [JsonPropertyName("ep")]
        public int Episode { get; init; }

        [JsonConstructor]
        internal MapData()
        {
            Placement1 = string.Empty;
            Placement2 = string.Empty;
            Parameters = [];
            MaxPlayerPerFight = 16;
            MaxPlayerPerTeam = 8;
        }

        public string GetCoordinate()
        {
            return $"[{XCoord}, {YCoord}]";
        }

        public string GetImagePath()
        {
            return $"{DofusApi.Config.CdnUrl}/images/maps/{Id}.jpg";
        }

        public MapSubAreaData? GetMapSubAreaData()
        {
            return DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(MapSubAreaId);
        }

        public string GetMapAreaName()
        {
            MapSubAreaData? mapSubAreaData = GetMapSubAreaData();
            string mapSubAreaName = mapSubAreaData is null ? $"{nameof(MapSubAreaData)} {PatternDecoder.Description(Resources.Unknown_Data, MapSubAreaId)}" : mapSubAreaData.Name.TrimStart("//");

            MapAreaData? mapAreaData = mapSubAreaData?.GetMapAreaData();
            string mapAreaName = mapAreaData is null ? $"{nameof(MapAreaData)} {PatternDecoder.Description(Resources.Unknown_Data, mapSubAreaData?.MapAreaId ?? 0)}" : mapAreaData.Name;

            return mapAreaName + (mapAreaName.Equals(mapSubAreaName) ? "" : $" ({mapSubAreaName})");
        }

        public HouseData? GetHouseData()
        {
            HouseMapData? houseMapData = DofusApi.Datacenter.HousesData.GetHouseMapDataById(Id);

            return houseMapData?.GetHouseData();
        }

        public bool IsHouse()
        {
            return GetHouseData() is not null;
        }
    }

    public sealed class MapSuperAreaData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        [JsonConstructor]
        internal MapSuperAreaData()
        {
            Name = string.Empty;
        }
    }

    public sealed class MapAreaData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("sua")]
        public int MapSuperAreaId { get; init; }

        [JsonConstructor]
        internal MapAreaData()
        {
            Name = string.Empty;
        }

        public MapSuperAreaData? GetMapSuperAreaData()
        {
            return DofusApi.Datacenter.MapsData.GetMapSuperAreaDataById(MapSuperAreaId);
        }

        public IEnumerable<MapData> GetMapsData()
        {
            return DofusApi.Datacenter.MapsData.GetMapsDataByMapAreaId(Id);
        }
    }

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
            foreach (int mapSubAreaId in NearMapSubAreasId)
            {
                MapSubAreaData? mapSubAreaData = DofusApi.Datacenter.MapsData.GetMapSubAreaDataById(mapSubAreaId);
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

    public sealed class MapsData : IDofusData
    {
        private const string FILE_NAME = "maps.json";

        [JsonPropertyName("MA.m")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapData>))]
        public FrozenDictionary<int, MapData> Maps { get; init; }

        [JsonPropertyName("MA.sua")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapSuperAreaData>))]
        public FrozenDictionary<int, MapSuperAreaData> MapSuperAreas { get; init; }

        [JsonPropertyName("MA.a")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapAreaData>))]
        public FrozenDictionary<int, MapAreaData> MapAreas { get; init; }

        [JsonPropertyName("MA.sa")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, MapSubAreaData>))]
        public FrozenDictionary<int, MapSubAreaData> MapSubAreas { get; init; }

        [JsonConstructor]
        internal MapsData()
        {
            Maps = FrozenDictionary<int, MapData>.Empty;
            MapSuperAreas = FrozenDictionary<int, MapSuperAreaData>.Empty;
            MapAreas = FrozenDictionary<int, MapAreaData>.Empty;
            MapSubAreas = FrozenDictionary<int, MapSubAreaData>.Empty;
        }

        internal static MapsData Load()
        {
            return Datacenter.LoadDataFromFile<MapsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public MapData? GetMapDataById(int id)
        {
            Maps.TryGetValue(id, out MapData? mapData);
            return mapData;
        }

        public IEnumerable<MapData> GetMapsDataByCoordinate(int xCoord, int yCoord)
        {
            return Maps.Values.Where(x => x.XCoord == xCoord && x.YCoord == yCoord);
        }

        public IEnumerable<MapData> GetMapsDataByMapAreaId(int id)
        {
            return Maps.Values.Where(x => x.GetMapSubAreaData()?.GetMapAreaData()?.Id == id);
        }

        public IEnumerable<MapData> GetMapsDataByMapSubAreaId(int id)
        {
            return Maps.Values.Where(x => x.GetMapSubAreaData()?.Id == id);
        }

        public MapSuperAreaData? GetMapSuperAreaDataById(int id)
        {
            MapSuperAreas.TryGetValue(id, out MapSuperAreaData? mapSuperAreaData);
            return mapSuperAreaData;
        }

        public string GetMapSuperAreaNameById(int id)
        {
            MapSuperAreaData? mapSuperAreaData = GetMapSuperAreaDataById(id);

            return mapSuperAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapSuperAreaData.Name;
        }

        public MapAreaData? GetMapAreaDataById(int id)
        {
            MapAreas.TryGetValue(id, out MapAreaData? mapAreaData);
            return mapAreaData;
        }

        public IEnumerable<MapAreaData> GetMapAreasDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return MapAreas.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetMapAreaNameById(int id)
        {
            MapAreaData? mapAreaData = GetMapAreaDataById(id);

            return mapAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapAreaData.Name;
        }

        public MapSubAreaData? GetMapSubAreaDataById(int id)
        {
            MapSubAreas.TryGetValue(id, out MapSubAreaData? mapSubAreaData);
            return mapSubAreaData;
        }

        public IEnumerable<MapSubAreaData> GetMapSubAreasDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return MapSubAreas.Values.Where(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetMapSubAreaNameById(int id)
        {
            MapSubAreaData? mapSubAreaData = GetMapSubAreaDataById(id);

            return mapSubAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapSubAreaData.Name.TrimStart("//");
        }
    }
}
