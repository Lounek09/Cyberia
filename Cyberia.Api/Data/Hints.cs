using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class HintCategoryData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public string Color { get; init; }

        [JsonConstructor]
        internal HintCategoryData()
        {
            Name = string.Empty;
            Color = string.Empty;
        }
    }

    public sealed class HintData : IDofusData
    {
        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public int HintCategoryId { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("m")]
        public int MapId { get; init; }

        [JsonConstructor]
        internal HintData()
        {
            Name = string.Empty;
        }

        public HintCategoryData? GetHintCategory()
        {
            return DofusApi.Datacenter.HintsData.GetHintCategory(HintCategoryId);
        }

        public MapData? GetMap()
        {
            return DofusApi.Datacenter.MapsData.GetMapDataById(MapId);
        }
    }

    public sealed class HintsData : IDofusData
    {
        private const string FILE_NAME = "hints.json";

        [JsonPropertyName("HIC")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, HintCategoryData>))]
        public FrozenDictionary<int, HintCategoryData> HintCategories { get; init; }

        [JsonPropertyName("HI")]
        [JsonConverter(typeof(ReadOnlyCollectionConverter<HintData>))]
        public ReadOnlyCollection<HintData> Hints { get; init; }

        [JsonConstructor]
        internal HintsData()
        {
            HintCategories = FrozenDictionary<int, HintCategoryData>.Empty;
            Hints = ReadOnlyCollection<HintData>.Empty;
        }

        internal static HintsData Load()
        {
            return Datacenter.LoadDataFromFile<HintsData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public HintCategoryData? GetHintCategory(int id)
        {
            HintCategories.TryGetValue(id, out HintCategoryData? hintsCategory);
            return hintsCategory;
        }
    }
}
