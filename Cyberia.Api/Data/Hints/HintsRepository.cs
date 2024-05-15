using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Hints;

public sealed class HintsRepository : IDofusRepository
{
    private const string c_fileName = "hints.json";

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

    internal static HintsRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<HintsRepository>(filePath);
    }

    public HintCategoryData? GetHintCategory(int id)
    {
        HintCategories.TryGetValue(id, out var hintsCategory);
        return hintsCategory;
    }
}
