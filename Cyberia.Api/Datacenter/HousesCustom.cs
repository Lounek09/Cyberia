using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    internal sealed class HouseCustom
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("om")]
        public int OutdoorMapId { get; init; }

        [JsonPropertyName("r")]
        public int RoomNumber { get; init; }

        [JsonPropertyName("c")]
        public int ChestNumber { get; init; }

        [JsonPropertyName("p")]
        public int Price { get; init; }

        public HouseCustom()
        {

        }
    }

    internal sealed class HousesCustomData
    {
        [JsonPropertyName("CH.h")]
        public List<HouseCustom> HousesCustom { get; init; }

        public HousesCustomData()
        {
            HousesCustom = new();
        }
    }
}
