using Cyberia.Api.Data.Custom;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class HouseData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

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
            Name = string.Empty;
            Description = string.Empty;
        }

        public IEnumerable<MapData> GetMapsData()
        {
            foreach (HouseMapData houseMapData in DofusApi.Datacenter.HousesData.GetHouseMapsDataByHouseId(Id))
            {
                MapData? mapData = houseMapData.GetMapData();
                if (mapData is not null)
                {
                    yield return mapData;
                }
            }
        }

        public MapData? GetOutdoorMapData()
        {
            return DofusApi.Datacenter.MapsData.GetMapDataById(OutdoorMapId);
        }

        public string GetCoordinate()
        {
            MapData? mapData = GetOutdoorMapData();

            return mapData is null ? "" : mapData.GetCoordinate();
        }
    }

    public sealed class HouseMapData
    {
        [JsonPropertyName("id")]
        public int MapId { get; init; }

        [JsonPropertyName("v")]
        public int HouseId { get; init; }

        [JsonConstructor]
        internal HouseMapData()
        {

        }

        public MapData? GetMapData()
        {
            return DofusApi.Datacenter.MapsData.GetMapDataById(MapId);
        }

        public HouseData? GetHouseData()
        {
            return DofusApi.Datacenter.HousesData.GetHouseDataById(HouseId);
        }
    }

    public sealed class HousesData
    {
        private const string FILE_NAME = "houses.json";

        [JsonPropertyName("H.h")]
        public List<HouseData> Houses { get; init; }

        [JsonPropertyName("H.m")]
        public List<HouseMapData> HouseMaps { get; init; }

        [JsonPropertyName("H.ids")]
        public List<int> HousesIndoorSkillsId { get; init; }

        [JsonConstructor]
        public HousesData()
        {
            Houses = [];
            HouseMaps = [];
            HousesIndoorSkillsId = [];
        }

        internal static HousesData Load()
        {
            HousesData data = Json.LoadFromFile<HousesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
            HousesCustomData customData = Json.LoadFromFile<HousesCustomData>(Path.Combine(DofusApi.CUSTOM_PATH, FILE_NAME));

            foreach (HouseCustomData houseCustomData in customData.HousesCustom)
            {
                HouseData? houseData = data.GetHouseDataById(houseCustomData.Id);
                if (houseData is not null)
                {
                    houseData.OutdoorMapId = houseCustomData.OutdoorMapId;
                    houseData.RoomNumber = houseCustomData.RoomNumber;
                    houseData.ChestNumber = houseCustomData.ChestNumber;
                    houseData.Price = houseCustomData.Price;
                }
            }

            return data;
        }

        public HouseData? GetHouseDataById(int id)
        {
            return Houses.Find(x => x.Id == id);
        }

        public List<HouseData> GetHousesDataByName(string name)
        {
            string[] names = name.NormalizeCustom().Split(' ');
            return Houses.FindAll(h => names.All(h.Name.NormalizeCustom().Contains)).OrderBy(h => h.Id).ToList();
        }

        public List<HouseData> GetHousesDataByCoordinate(int x, int y)
        {
            List<HouseData> housesData = [];

            foreach (HouseData houseData in Houses)
            {
                MapData? map = houseData.GetOutdoorMapData();
                if (map is not null && map.XCoord == x && map.YCoord == y)
                {
                    housesData.Add(houseData);
                }
            }

            return housesData;
        }

        public List<HouseData> GetHousesDataByMapSubAreaId(int id)
        {
            List<HouseData> housesData = [];

            foreach (HouseData houseData in Houses)
            {
                MapData? map = houseData.GetOutdoorMapData();
                if (map is not null && map.GetMapSubAreaData()?.Id == id)
                {
                    housesData.Add(houseData);
                }
            }

            return housesData;
        }

        public List<HouseData> GetHousesDataByMapAreaId(int id)
        {
            List<HouseData> housesData = [];

            foreach (HouseData houseData in Houses)
            {
                MapData? map = houseData.GetOutdoorMapData();
                if (map is not null && map.GetMapSubAreaData()?.GetMapAreaData()?.Id == id)
                {
                    housesData.Add(houseData);
                }
            }

            return housesData;
        }

        public List<HouseMapData> GetHouseMapsDataByHouseId(int id)
        {
            return HouseMaps.FindAll(x => x.HouseId == id);
        }

        public HouseMapData? GetHouseMapDataByMapId(int id)
        {
            return HouseMaps.Find(x => x.MapId == id);
        }
    }
}
