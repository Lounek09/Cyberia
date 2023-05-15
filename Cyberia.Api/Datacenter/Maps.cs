using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Map
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

        public Map()
        {
            Placement1 = string.Empty;
            Placement2 = string.Empty;
            Parameters = new();
            MaxPlayerPerFight = 16;
            MaxPlayerPerTeam = 8;
        }

        public string GetCoordinate()
        {
            return $"[{XCoord}, {YCoord}]";
        }

        public string GetImagePath()
        {
            return $"{DofusApi.Instance.CdnUrl}/images/maps/{Id}.jpg";
        }

        public MapSubArea? GetMapSubArea()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaById(MapSubAreaId);
        }

        public string GetMapAreaName()
        {
            MapSubArea? mapSubArea = GetMapSubArea();
            string mapSubAreaName = mapSubArea is null ? $"MapSubArea Inconnu ({MapSubAreaId})" : mapSubArea.Name.TrimStart("//");
            MapArea? mapArea = mapSubArea?.GetMapArea();
            string mapAreaName = mapArea is null ? $"MapArea Inconnu ({mapSubArea?.MapAreaId})" : mapArea.Name;

            return mapAreaName + (mapAreaName.Equals(mapSubAreaName) ? "" : $" ({mapSubAreaName})");
        }

        public House? GetHouse()
        {
            HouseMap? houseMap = DofusApi.Instance.Datacenter.HousesData.GetHouseMapByMapId(Id);

            return houseMap?.GetHouse();
        }

        public bool IsHouse()
        {
            return GetHouse() is not null;
        }
    }

    public sealed class MapSuperArea
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("v")]
        public string Name { get; init; }

        public MapSuperArea()
        {
            Name = string.Empty;
        }
    }

    public sealed class MapArea
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("sua")]
        public int MapSuperAreaId { get; init; }

        public MapArea()
        {
            Name = string.Empty;
        }

        public MapSuperArea? GetMapSuperArea()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapSuperAreaById(MapSuperAreaId);
        }

        public List<Map> GetMaps()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapsByMapAreaId(Id);
        }
    }

    public sealed class MapSubArea
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

        public MapSubArea()
        {
            Name = string.Empty;
            FightAudioMusicId = new();
            NearMapSubAreasId = new();
        }

        public MapArea? GetMapArea()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapAreaById(MapAreaId);
        }

        public List<MapSubArea?> GetNearMapSubAreas()
        {
            List<MapSubArea?> mapSubAreas = new();

            foreach (int mapSubAreaId in NearMapSubAreasId)
                mapSubAreas.Add(DofusApi.Instance.Datacenter.MapsData.GetMapSubAreaById(mapSubAreaId));

            return mapSubAreas;
        }

        public AudioMusic? GetFightAudioMusic()
        {
            return DofusApi.Instance.Datacenter.AudiosData.GetAudioMusicById(FightAudioMusicId[0] ?? -1);
        }

        public List<Map> GetMaps()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapsByMapSubAreaId(Id);
        }
    }

    public sealed class MapsData
    {
        private const string FILE_NAME = "maps.json";

        [JsonPropertyName("MAm")]
        public List<Map> Maps { get; init; }

        [JsonPropertyName("MAsua")]
        public List<MapSuperArea> MapSuperAreas { get; init; }

        [JsonPropertyName("MAa")]
        public List<MapArea> MapAreas { get; init; }

        [JsonPropertyName("MAsa")]
        public List<MapSubArea> MapSubAreas { get; init; }

        public MapsData()
        {
            Maps = new();
            MapSuperAreas = new();
            MapAreas = new();
            MapSubAreas = new();
        }

        internal static MapsData Build()
        {
            return Json.LoadFromFile<MapsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Map? GetMapById(int id)
        {
            return Maps.Find(x => x.Id == id);
        }

        public List<Map> GetMapsByCoordinate(int xCoord, int yCoord)
        {
            return Maps.FindAll(x => x.XCoord == xCoord && x.YCoord == yCoord);
        }

        public List<Map> GetMapsByMapAreaId(int id)
        {
            return Maps.FindAll(x => x.GetMapSubArea()?.GetMapArea()?.Id == id);
        }

        public List<Map> GetMapsByMapSubAreaId(int id)
        {
            return Maps.FindAll(x => x.GetMapSubArea()?.Id == id);
        }

        public MapSuperArea? GetMapSuperAreaById(int id)
        {
            return MapSuperAreas.Find(x => x.Id == id);
        }

        public string GetMapSuperAreaNameById(int id)
        {
            MapSuperArea? mapSuperArea = GetMapSuperAreaById(id);

            return mapSuperArea is null ? $"Inconnu ({id})" : mapSuperArea.Name;
        }

        public MapArea? GetMapAreaById(int id)
        {
            return MapAreas.Find(x => x.Id == id);
        }

        public List<MapArea> GetMapAreasByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return MapAreas.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetMapAreaNameById(int id)
        {
            MapArea? mapArea = GetMapAreaById(id);

            return mapArea is null ? $"Inconnu ({id})" : mapArea.Name;
        }

        public MapSubArea? GetMapSubAreaById(int id)
        {
            return MapSubAreas.Find(x => x.Id == id);
        }

        public List<MapSubArea> GetMapSubAreasByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return MapSubAreas.FindAll(x => names.All(x.Name.RemoveDiacritics().Contains));
        }

        public string GetMapSubAreaNameById(int id)
        {
            MapSubArea? mapSubArea = GetMapSubAreaById(id);

            return mapSubArea is null ? $"Inconnu ({id})" : mapSubArea.Name;
        }
    }
}
