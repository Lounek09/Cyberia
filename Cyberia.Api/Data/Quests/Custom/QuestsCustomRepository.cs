using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Custom;

internal sealed class QuestsCustomRepository : IDofusRepository
{
    [JsonPropertyName("CQ.q")]
    public IReadOnlyList<QuestCustomData> QuestsCustom { get; init; }

    [JsonPropertyName("CQ.s")]
    public IReadOnlyList<QuestStepCustomData> QuestStepsCustom { get; init; }

    [JsonConstructor]
    internal QuestsCustomRepository()
    {
        QuestsCustom = [];
        QuestStepsCustom = [];
    }
}
