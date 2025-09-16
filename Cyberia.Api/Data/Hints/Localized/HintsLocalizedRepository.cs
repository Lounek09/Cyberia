using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints.Localized;

internal sealed class HintsLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => HintsRepository.FileName;

    [JsonPropertyName("HIC")]
    public IReadOnlyList<HintCategoryLocalizedData> HintCategories { get; init; }

    [JsonConstructor]
    internal HintsLocalizedRepository()
    {
        HintCategories = ReadOnlyCollection<HintCategoryLocalizedData>.Empty;
    }
}
