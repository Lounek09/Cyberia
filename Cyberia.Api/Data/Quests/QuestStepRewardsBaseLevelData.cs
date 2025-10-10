using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestStepRewardsBaseLevelData : IDofusData
{
    [JsonPropertyName("k")]
    public int Kamas { get; init; }

    [JsonPropertyName("xp")]
    public int Experience { get; init; }

    [JsonPropertyName("max")]
    public int MaxLevel { get; init; }

    [JsonPropertyName("min")]
    public int MinLevel { get; init; }

    [JsonConstructor]
    internal QuestStepRewardsBaseLevelData() { }
}
