using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks;

public sealed class RanksData
    : IDofusData
{
    private const string FILE_NAME = "ranks.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("R")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, GuildRankData>))]
    public FrozenDictionary<int, GuildRankData> GuildRanks { get; init; }

    [JsonConstructor]
    internal RanksData()
    {
        GuildRanks = FrozenDictionary<int, GuildRankData>.Empty;
    }

    internal static async Task<RanksData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<RanksData>(FILE_PATH);
    }

    public GuildRankData? GetGuildRank(int id)
    {
        GuildRanks.TryGetValue(id, out var rank);
        return rank;
    }
}
