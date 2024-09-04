using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks;

public sealed class RanksRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "ranks.json";

    [JsonPropertyName("R")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, GuildRankData>))]
    public FrozenDictionary<int, GuildRankData> GuildRanks { get; init; }

    [JsonConstructor]
    internal RanksRepository()
    {
        GuildRanks = FrozenDictionary<int, GuildRankData>.Empty;
    }

    public GuildRankData? GetGuildRank(int id)
    {
        GuildRanks.TryGetValue(id, out var rank);
        return rank;
    }
}
