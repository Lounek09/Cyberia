using Cyberia.Api.Data.Hints.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintsRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "hints.json";

    [JsonPropertyName("HIC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HintCategoryData>))]
    public FrozenDictionary<int, HintCategoryData> HintCategories { get; init; }

    [JsonPropertyName("HI")]
    public IReadOnlyList<HintData> Hints { get; init; }

    [JsonConstructor]
    internal HintsRepository()
    {
        HintCategories = FrozenDictionary<int, HintCategoryData>.Empty;
        Hints = ReadOnlyCollection<HintData>.Empty;
    }

    public HintCategoryData? GetHintCategoryDataById(int id)
    {
        HintCategories.TryGetValue(id, out var hintsCategory);
        return hintsCategory;
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<HintsLocalizedRepository>(identifier);

        foreach (var hintCategoryLocalizedData in localizedRepository.HintCategories)
        {
            var hintCategoryData = GetHintCategoryDataById(hintCategoryLocalizedData.Id);
            hintCategoryData?.Name.TryAdd(twoLetterISOLanguageName, hintCategoryLocalizedData.Name);
        }
    }
}
