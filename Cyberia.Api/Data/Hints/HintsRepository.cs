using Cyberia.Api.Data.Hints.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
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
        Hints = [];
    }

    public HintCategoryData? GetHintCategoryDataById(int id)
    {
        HintCategories.TryGetValue(id, out var hintsCategory);
        return hintsCategory;
    }

    protected override void LoadLocalizedData(LangType type, Language language)
    {
        var twoLetterISOLanguageName = language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<HintsLocalizedRepository>(type, language);

        foreach (var hintCategoryLocalizedData in localizedRepository.HintCategories)
        {
            var hintCategoryData = GetHintCategoryDataById(hintCategoryLocalizedData.Id);
            hintCategoryData?.Name.TryAdd(twoLetterISOLanguageName, hintCategoryLocalizedData.Name);
        }
    }
}
