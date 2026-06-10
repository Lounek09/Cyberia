using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses.Custom;

internal sealed class HousesCustomRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => HousesRepository.FileName;

    [JsonPropertyName("CH.h")]
    public IReadOnlyList<HouseCustomData> HousesCustom { get; init; }

    [JsonConstructor]
    internal HousesCustomRepository()
    {
        HousesCustom = ReadOnlyCollection<HouseCustomData>.Empty;
    }
}
