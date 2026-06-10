using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks;

public sealed class GuildRankData : IDofusData<int>, IComparable<GuildRankData>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("o")]
    public int Order { get; init; }

    [JsonPropertyName("i")]
    public int Index { get; init; }

    [JsonConstructor]
    internal GuildRankData()
    {
        Name = LocalizedString.Empty;
    }

    public int CompareTo(GuildRankData? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Order.CompareTo(other.Order);
    }
}
