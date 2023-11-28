using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks;

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
