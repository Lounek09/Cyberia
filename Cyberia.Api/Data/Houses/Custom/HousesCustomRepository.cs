using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses.Custom;

internal sealed class HousesCustomRepository : IDofusRepository
{
    [JsonPropertyName("CH.h")]
    public IReadOnlyList<HouseCustomData> HousesCustom { get; init; }

    [JsonConstructor]
    internal HousesCustomRepository()
    {
        HousesCustom = [];
    }
}
