using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Ride
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

        public Ride()
        {
            Name = string.Empty;
        }
    }

    public sealed class RideAbility
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("e")]
        public string E { get; init; }

        public RideAbility()
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
        public List<Ride> Rides { get; init; }

        [JsonPropertyName("RIA")]
        public List<RideAbility> RideAbilities { get; init; }

        public RidesData()
        {
            Rides = new();
            RideAbilities = new();
        }

        internal static RidesData Build()
        {
            return Json.LoadFromFile<RidesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public Ride? GetRideById(int id)
        {
            return Rides.Find(x => x.Id == id);
        }

        public string GetRideNameById(int id)
        {
            Ride? ride = GetRideById(id);

            return ride is null ? $"Inconnu ({id})" : ride.Name;
        }

        public RideAbility? GetRideAbilityById(int id)
        {
            return RideAbilities.Find(x => x.Id == id);
        }

        public string GetRideAbilityNameById(int id)
        {
            RideAbility? rideAbility = GetRideAbilityById(id);

            return rideAbility is null ? $"Inconnu ({id})" : rideAbility.Name;
        }
    }

}
