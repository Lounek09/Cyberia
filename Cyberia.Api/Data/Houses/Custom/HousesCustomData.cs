using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses.Custom;

internal sealed class HousesCustomData
    : IDofusData
{
    [JsonPropertyName("CH.h")]
    public IReadOnlyList<HouseCustomData> HousesCustom { get; init; }

    [JsonConstructor]
    internal HousesCustomData()
    {
        HousesCustom = [];
    }
}
