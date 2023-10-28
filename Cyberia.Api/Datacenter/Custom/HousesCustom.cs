using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Custom
{
    internal sealed class HouseCustomData
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

        public HouseCustomData()
        {

        }
    }

    internal sealed class HousesCustomData
    {
        [JsonPropertyName("CH.h")]
        public List<HouseCustomData> HousesCustom { get; init; }

        public HousesCustomData()
        {
            HousesCustom = new();
        }
    }
}
