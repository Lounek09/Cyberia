using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Custom;

internal sealed class QuestsCustomData
    : IDofusData
{
    [JsonPropertyName("CQ.q")]
    public IReadOnlyList<QuestCustomData> QuestsCustom { get; init; }

    [JsonPropertyName("CQ.s")]
    public IReadOnlyList<QuestStepCustomData> QuestStepsCustom { get; init; }

    [JsonConstructor]
    internal QuestsCustomData()
    {
        QuestsCustom = [];
        QuestStepsCustom = [];
    }
}
