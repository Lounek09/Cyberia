using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges;

public sealed class FightChallengeData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonPropertyName("g")]
    public int GfxId { get; init; }

    [JsonConstructor]
    internal FightChallengeData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
