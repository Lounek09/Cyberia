using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class RideData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("g")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int GfxId { get; init; }

        [JsonPropertyName("c1")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int Color1 { get; init; }

        [JsonPropertyName("c2")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int Color2 { get; init; }

        [JsonPropertyName("c3")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int Color3 { get; init; }

        public RideData()
        {
            Name = string.Empty;
        }
    }

    public sealed class RideAbilityData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("e")]
        public string E { get; init; }

        public RideAbilityData()
        {
            Name = string.Empty;
            Description = string.Empty;
            E = string.Empty;
        }
    }

    public sealed class RidesData
    {
        private const string FILE_NAME = "rides.json";

        [JsonPropertyName("RI")]
        public List<RideData> Rides { get; init; }

        [JsonPropertyName("RIA")]
        public List<RideAbilityData> RideAbilities { get; init; }

        public RidesData()
        {
            Rides = new();
            RideAbilities = new();
        }

        internal static RidesData Build()
        {
            return Json.LoadFromFile<RidesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public RideData? GetRideDataById(int id)
        {
            return Rides.Find(x => x.Id == id);
        }

        public string GetRideNameById(int id)
        {
            RideData? rideData = GetRideDataById(id);

            return rideData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : rideData.Name;
        }

        public RideAbilityData? GetRideAbilityDataById(int id)
        {
            return RideAbilities.Find(x => x.Id == id);
        }

        public string GetRideAbilityNameById(int id)
        {
            RideAbilityData? rideAbilityData = GetRideAbilityDataById(id);

            return rideAbilityData is null ? PatternDecoder.Description(Resources.Unknown_Data, id) : rideAbilityData.Name;
        }
    }

}
