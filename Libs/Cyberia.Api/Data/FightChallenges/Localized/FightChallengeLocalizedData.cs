using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges.Localized;

internal sealed class FightChallengeLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal FightChallengeLocalizedData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
