using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Houses.Localized;

internal sealed class HousesLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => HousesRepository.FileName;

    [JsonPropertyName("H.h")]
    public IReadOnlyList<HouseLocalizedData> Houses { get; init; }

    [JsonConstructor]
    internal HousesLocalizedRepository()
    {
        Houses = ReadOnlyCollection<HouseLocalizedData>.Empty;
    }
}
