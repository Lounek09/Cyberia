using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class HintsCategory
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public string Color { get; init; }

        public HintsCategory()
        {
            Name = string.Empty;
            Color = string.Empty;
        }
    }

    public sealed class Hint
    {
        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("c")]
        public int HintsCategoryId { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonPropertyName("m")]
        public int MapId { get; init; }

        public Hint()
        {
            Name = string.Empty;
        }
    }

    public sealed class HintsData
    {
        private const string FILE_NAME = "hints.json";

        [JsonPropertyName("HIC")]
        public List<HintsCategory> HintsCategories { get; init; }

        [JsonPropertyName("HI")]
        public List<Hint> Hints { get; init; }

        public HintsData()
        {
            HintsCategories = new();
            Hints = new();
        }

        internal static HintsData Build()
        {
            return Json.LoadFromFile<HintsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }
    }
}
