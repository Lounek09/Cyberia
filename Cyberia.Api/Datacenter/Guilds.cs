using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class Guild
    {
        [JsonPropertyName("w")]
        public List<List<int>> BoostCostWeight { get; set; }

        [JsonPropertyName("p")]
        public List<List<int>> BoostCostProspecting { get; set; }

        [JsonPropertyName("c")]
        public List<List<int>> BoostCostTaxCollector { get; set; }

        [JsonPropertyName("x")]
        public List<List<int>> BoostCostWisdom { get; set; }

        [JsonPropertyName("s")]
        public List<List<int>> BoostCostSpell { get; set; }

        [JsonPropertyName("wm")]
        public int WeightMax { get; set; }

        [JsonPropertyName("pm")]
        public int ProspectingMax { get; set; }

        [JsonPropertyName("cm")]
        public int TaxCollectorMax { get; set; }

        [JsonPropertyName("xm")]
        public int WisdomMax { get; set; }

        [JsonPropertyName("sm")]
        public int SpellMax { get; set; }

        public Guild()
        {
            BoostCostWeight = new();
            BoostCostProspecting = new();
            BoostCostTaxCollector = new();
            BoostCostWisdom = new();
            BoostCostSpell = new();
        }
    }

    public sealed class GuildsData
    {
        private const string FILE_NAME = "guilds.json";

        [JsonPropertyName("GUb")]
        public Guild Guild { get; init; }

        public GuildsData()
        {
            Guild = new();
        }

        internal static GuildsData Build()
        {
            return Json.LoadFromFile<GuildsData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }
    }
}
