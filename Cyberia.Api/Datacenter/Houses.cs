using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class House
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        public int OutdoorMapId { get; internal set; }

        public int RoomNumber { get; internal set; }

        public int ChestNumber { get; internal set; }

        public int Price { get; internal set; }

        public House()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public List<Map> GetMaps()
        {
            List<Map> maps = new();

            foreach (HouseMap houseMap in DofusApi.Instance.Datacenter.HousesData.GetHouseMapsByHouseId(Id))
            {
                Map? map = houseMap.GetMap();
                if (map is not null)
                    maps.Add(map);
            }

            return maps;
        }

        public Map? GetOutdoorMap()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapById(OutdoorMapId);
        }

        public string GetCoordinate()
        {
            Map? map = GetOutdoorMap();

            return map is null ? "" : map.GetCoordinate();
        }
    }

    public sealed class HouseMap
    {
        [JsonPropertyName("id")]
        public int MapId { get; init; }

        [JsonPropertyName("v")]
        public int HouseId { get; init; }

        public HouseMap()
        {

        }

        public Map? GetMap()
        {
            return DofusApi.Instance.Datacenter.MapsData.GetMapById(MapId);
        }

        public House? GetHouse()
        {
            return DofusApi.Instance.Datacenter.HousesData.GetHouseById(HouseId);
        }
    }

    public sealed class HousesData
    {
        private const string FILE_NAME = "houses.json";

        [JsonPropertyName("H.h")]
        public List<House> Houses { get; init; }

        [JsonPropertyName("H.m")]
        public List<HouseMap> HouseMaps { get; init; }

        [JsonPropertyName("H.ids")]
        public List<int> HousesIndoorSkillsId { get; init; }

        public HousesData()
        {
            Houses = new();
            HouseMaps = new();
            HousesIndoorSkillsId = new();
        }

        internal static HousesData Build()
        {
            HousesData data = Json.LoadFromFile<HousesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
            HousesCustomData customData = Json.LoadFromFile<HousesCustomData>($"{DofusApi.CUSTOM_PATH}/{FILE_NAME}");

            foreach (HouseCustom houseCustom in customData.HousesCustom)
            {
                House? house = data.GetHouseById(houseCustom.Id);
                if (house is not null)
                {
                    house.OutdoorMapId = houseCustom.OutdoorMapId;
                    house.RoomNumber = houseCustom.RoomNumber;
                    house.ChestNumber = houseCustom.ChestNumber;
                    house.Price = houseCustom.Price;
                }
            }

            return data;
        }

        public House? GetHouseById(int id)
        {
            return Houses.Find(x => x.Id == id);
        }

        public List<House> GetHousesByName(string name)
        {
            string[] names = name.RemoveDiacritics().Split(' ');
            return Houses.FindAll(h => names.All(h.Name.RemoveDiacritics().Contains)).OrderBy(h => h.Id).ToList();
        }

        public List<House> GetHousesByCoordinate(int x, int y)
        {
            List<House> houses = new();

            foreach (House house in Houses)
            {
                Map? map = house.GetOutdoorMap();
                if (map is not null && map.XCoord == x && map.YCoord == y)
                    houses.Add(house);
            }

            return houses;
        }

        public List<House> GetHousesByMapSubAreaId(int id)
        {
            List<House> houses = new();

            foreach (House house in Houses)
            {
                Map? map = house.GetOutdoorMap();
                if (map is not null && map.GetMapSubArea()?.Id == id)
                    houses.Add(house);
            }

            return houses;
        }

        public List<House> GetHousesByMapAreaId(int id)
        {
            List<House> houses = new();

            foreach (House house in Houses)
            {
                Map? map = house.GetOutdoorMap();
                if (map is not null && map.GetMapSubArea()?.GetMapArea()?.Id == id)
                    houses.Add(house);
            }

            return houses;
        }

        public List<HouseMap> GetHouseMapsByHouseId(int id)
        {
            return HouseMaps.FindAll(x => x.HouseId == id);
        }

        public HouseMap? GetHouseMapByMapId(int id)
        {
            return HouseMaps.Find(x => x.MapId == id);
        }
    }
}
