using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class PvpGrade
    {
        [JsonPropertyName("nc")]
        public string ShortName { get; init; }

        [JsonPropertyName("nl")]
        public string Name { get; init; }

        public PvpGrade()
        {
            ShortName = string.Empty;
            Name = string.Empty;
        }
    }

    public sealed class PvpData
    {
        private const string FILE_NAME = "pvp.json";

        [JsonPropertyName("PPhp")]
        public List<int> HonnorPointThresholds { get; init; }

        [JsonPropertyName("PPmaxdp")]
        public int MaxDishonourPoint { get; init; }

        [JsonPropertyName("PPgrds")]
        public List<List<PvpGrade>> PvpGrades { get; init; }

        public PvpData()
        {
            HonnorPointThresholds = new();
            PvpGrades = new();
        }

        internal static PvpData Build()
        {
            return Json.LoadFromFile<PvpData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }
    }
}
