using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class GuildRank
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("o")]
        public int Order { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        public GuildRank()
        {
            Name = string.Empty;
        }
    }

    public sealed class RanksData
    {
        private const string FILE_NAME = "ranks.json";

        [JsonPropertyName("R")]
        public List<GuildRank> GuildRanks { get; init; }

        internal static RanksData Build()
        {
            return Json.LoadFromFile<RanksData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }

        public RanksData()
        {
            GuildRanks = new();
        }
    }
}
