using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks;

public sealed class RanksRepository : IDofusRepository
{
    private const string c_fileName = "ranks.json";

    [JsonPropertyName("R")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, GuildRankData>))]
    public FrozenDictionary<int, GuildRankData> GuildRanks { get; init; }

    [JsonConstructor]
    internal RanksRepository()
    {
        GuildRanks = FrozenDictionary<int, GuildRankData>.Empty;
    }

    internal static RanksRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<RanksRepository>(filePath);
    }

    public GuildRankData? GetGuildRank(int id)
    {
        GuildRanks.TryGetValue(id, out var rank);
        return rank;
    }
}
