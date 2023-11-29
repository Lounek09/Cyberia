using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintsData : IDofusData
{
    private const string FILE_NAME = "hints.json";

    [JsonPropertyName("HIC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HintCategoryData>))]
    public FrozenDictionary<int, HintCategoryData> HintCategories { get; init; }

    [JsonPropertyName("HI")]
    public IReadOnlyList<HintData> Hints { get; init; }

    [JsonConstructor]
    internal HintsData()
    {
        HintCategories = FrozenDictionary<int, HintCategoryData>.Empty;
        Hints = [];
    }

    internal static HintsData Load()
    {
        return Datacenter.LoadDataFromFile<HintsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public HintCategoryData? GetHintCategory(int id)
    {
        HintCategories.TryGetValue(id, out var hintsCategory);
        return hintsCategory;
    }
}
