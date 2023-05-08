using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    internal sealed class QuestCustom
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("r")]
        public bool Repeatable { get; init; }

        [JsonPropertyName("a")]
        public bool Account { get; init; }

        [JsonPropertyName("d")]
        public bool HasDungeon { get; init; }

        [JsonPropertyName("s")]
        public List<int> QuestStepsId { get; init; }

        public QuestCustom()
        {
            QuestStepsId = new();
        }
    }

    internal sealed class QuestStepCustom
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("d")]
        public int DialogQuestionId { get; init; }

        [JsonPropertyName("l")]
        public int OptimalLevel { get; init; }

        [JsonPropertyName("o")]
        public List<int> QuestObjectivesId { get; init; }

        public QuestStepCustom()
        {
            QuestObjectivesId = new();
        }
    }

    internal sealed class QuestsCustomData
    {
        [JsonPropertyName("CQq")]
        public List<QuestCustom> QuestsCustom { get; init; }

        [JsonPropertyName("CQs")]
        public List<QuestStepCustom> QuestStepsCustom { get; init; }

        public QuestsCustomData()
        {
            QuestsCustom = new();
            QuestStepsCustom = new();
        }
    }
}
