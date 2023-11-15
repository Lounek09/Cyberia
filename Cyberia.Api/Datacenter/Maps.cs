using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class MapData
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

        public MapData()
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
            HouseMapData? houseMapData = DofusApi.Datacenter.HousesData.GetHouseMapDataByMapId(Id);

            return houseMapData?.GetHouseData();
        }

        public bool IsHouse()
        {
            return GetHouseData() is not null;
        }
    }

    public sealed class MapSuperAreaData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public MapSuperAreaData()
        {
            Name = string.Empty;
        }
    }

    public sealed class MapAreaData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("sua")]
        public int MapSuperAreaId { get; init; }

        public MapAreaData()
        {
            Name = string.Empty;
        }

        public MapSuperAreaData? GetMapSuperAreaData()
        {
            return DofusApi.Datacenter.MapsData.GetMapSuperAreaDataById(MapSuperAreaId);
        }

        public List<MapData> GetMapsData()
        {
            return DofusApi.Datacenter.MapsData.GetMapsDataByMapAreaId(Id);
        }
    }

    public sealed class MapSubAreaData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("a")]
        public int MapAreaId { get; init; }

        [JsonPropertyName("m")]
        public List<int?> FightAudioMusicId { get; init; }

        [JsonPropertyName("v")]
        public List<int> NearMapSubAreasId { get; init; }

        public MapSubAreaData()
        {
            Name = string.Empty;
            FightAudioMusicId = [];
            NearMapSubAreasId = [];
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

        public List<MapData> GetMapsData()
        {
            return DofusApi.Datacenter.MapsData.GetMapsDataByMapSubAreaId(Id);
        }
    }

    public sealed class MapsData
    {
        private const string FILE_NAME = "maps.json";

        [JsonPropertyName("MA.m")]
        public List<MapData> Maps { get; init; }

        [JsonPropertyName("MA.sua")]
        public List<MapSuperAreaData> MapSuperAreas { get; init; }

        [JsonPropertyName("MA.a")]
        public List<MapAreaData> MapAreas { get; init; }

        [JsonPropertyName("MA.sa")]
        public List<MapSubAreaData> MapSubAreas { get; init; }

        public MapsData()
        {
            Maps = [];
            MapSuperAreas = [];
            MapAreas = [];
            MapSubAreas = [];
        }

        internal static MapsData Build()
        {
            return Json.LoadFromFile<MapsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public MapData? GetMapDataById(int id)
        {
            return Maps.Find(x => x.Id == id);
        }

        public List<MapData> GetMapsDataByCoordinate(int xCoord, int yCoord)
        {
            return Maps.FindAll(x => x.XCoord == xCoord && x.YCoord == yCoord);
        }

        public List<MapData> GetMapsDataByMapAreaId(int id)
        {
            return Maps.FindAll(x => x.GetMapSubAreaData()?.GetMapAreaData()?.Id == id);
        }

        public List<MapData> GetMapsDataByMapSubAreaId(int id)
        {
            return Maps.FindAll(x => x.GetMapSubAreaData()?.Id == id);
        }

        public MapSuperAreaData? GetMapSuperAreaDataById(int id)
        {
            return MapSuperAreas.Find(x => x.Id == id);
        }

        public string GetMapSuperAreaNameById(int id)
        {
            MapSuperAreaData? mapSuperAreaData = GetMapSuperAreaDataById(id);

            return mapSuperAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapSuperAreaData.Name;
        }

        public MapAreaData? GetMapAreaDataById(int id)
        {
            return MapAreas.Find(x => x.Id == id);
        }

        public List<MapAreaData> GetMapAreasDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return MapAreas.FindAll(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetMapAreaNameById(int id)
        {
            MapAreaData? mapAreaData = GetMapAreaDataById(id);

            return mapAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapAreaData.Name;
        }

        public MapSubAreaData? GetMapSubAreaDataById(int id)
        {
            return MapSubAreas.Find(x => x.Id == id);
        }

        public List<MapSubAreaData> GetMapSubAreasDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return MapSubAreas.FindAll(x => names.All(x.Name.NormalizeCustom().Contains));
        }

        public string GetMapSubAreaNameById(int id)
        {
            MapSubAreaData? mapSubAreaData = GetMapSubAreaDataById(id);

            return mapSubAreaData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : mapSubAreaData.Name.TrimStart("//");
        }
    }
}
