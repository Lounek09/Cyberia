using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class PvpGradeData
    {
        [JsonPropertyName("nc")]
        public string ShortName { get; init; }

        [JsonPropertyName("nl")]
        public string Name { get; init; }

        public PvpGradeData()
        {
            ShortName = string.Empty;
            Name = string.Empty;
        }
    }

    public sealed class PvpData
    {
        private const string FILE_NAME = "pvp.json";

        [JsonPropertyName("PP.hp")]
        public List<int> HonnorPointThresholds { get; init; }

        [JsonPropertyName("PP.maxdp")]
        public int MaxDishonourPoint { get; init; }

        [JsonPropertyName("PP.grds")]
        public List<List<PvpGradeData>> PvpGrades { get; init; }

        public PvpData()
        {
            HonnorPointThresholds = [];
            PvpGrades = [];
        }

        internal static PvpData Build()
        {
            return Json.LoadFromFile<PvpData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }
    }
}
