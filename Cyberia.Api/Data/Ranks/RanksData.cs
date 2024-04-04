using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks;

public sealed class RanksData
    : IDofusData
{
    private const string c_fileName = "ranks.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<RanksData>(s_filePath);
    }

    public GuildRankData? GetGuildRank(int id)
    {
        GuildRanks.TryGetValue(id, out var rank);
        return rank;
    }
}
