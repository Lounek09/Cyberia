using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Ranks.Localized;

internal sealed class GuildRankLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal GuildRankLocalizedData()
    {
        Name = string.Empty;
    }
}
