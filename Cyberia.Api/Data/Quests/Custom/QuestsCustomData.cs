using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Custom;

internal sealed class QuestsCustomData : IDofusData
{
    [JsonPropertyName("CQ.q")]
    public List<QuestCustomData> QuestsCustom { get; init; }

    [JsonPropertyName("CQ.s")]
    public List<QuestStepCustomData> QuestStepsCustom { get; init; }

    [JsonConstructor]
    internal QuestsCustomData()
    {
        QuestsCustom = [];
        QuestStepsCustom = [];
    }
}
