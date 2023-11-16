using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class GuildRankData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("o")]
        public int Order { get; init; }

        [JsonPropertyName("i")]
        public int Index { get; init; }

        [JsonConstructor]
        internal GuildRankData()
        {
            Name = string.Empty;
        }
    }

    public sealed class RanksData
    {
        private const string FILE_NAME = "ranks.json";

        [JsonPropertyName("R")]
        public List<GuildRankData> GuildRanks { get; init; }

        [JsonConstructor]
        public RanksData()
        {
            GuildRanks = [];
        }

        internal static RanksData Load()
        {
            return Json.LoadFromFile<RanksData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }
    }
}
