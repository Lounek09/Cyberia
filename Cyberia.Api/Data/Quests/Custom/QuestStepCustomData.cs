using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Custom;

internal sealed class QuestStepCustomData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("d")]
    public int DialogQuestionId { get; init; }

    [JsonPropertyName("l")]
    public int OptimalLevel { get; init; }

    [JsonPropertyName("o")]
    public IReadOnlyList<int> QuestObjectivesId { get; init; }

    [JsonConstructor]
    internal QuestStepCustomData()
    {
        QuestObjectivesId = [];
    }
}
