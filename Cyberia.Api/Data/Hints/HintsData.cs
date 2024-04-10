using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintsData : IDofusData
{
    private const string c_fileName = "hints.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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

    internal static async Task<HintsData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<HintsData>(s_filePath);
    }

    public HintCategoryData? GetHintCategory(int id)
    {
        HintCategories.TryGetValue(id, out var hintsCategory);
        return hintsCategory;
    }
}
