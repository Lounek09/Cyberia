using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class GuildRankData : IDofusData<int>
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

    public sealed class RanksData : IDofusData
    {
        private const string FILE_NAME = "ranks.json";

        [JsonPropertyName("R")]
        [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, GuildRankData>))]
        public FrozenDictionary<int, GuildRankData> GuildRanks { get; init; }

        [JsonConstructor]
        internal RanksData()
        {
            GuildRanks = FrozenDictionary<int, GuildRankData>.Empty;
        }

        internal static RanksData Load()
        {
            return Datacenter.LoadDataFromFile<RanksData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }

        public GuildRankData? GetGuildRank(int id)
        {
            GuildRanks.TryGetValue(id, out GuildRankData? rank);
            return rank;
        }
    }
}
