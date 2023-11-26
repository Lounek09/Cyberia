using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Custom
{
    internal sealed class QuestCustomData : IDofusData<int>
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

        [JsonConstructor]
        internal QuestCustomData()
        {
            QuestStepsId = [];
        }
    }

    internal sealed class QuestStepCustomData : IDofusData<int>
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("d")]
        public int DialogQuestionId { get; init; }

        [JsonPropertyName("l")]
        public int OptimalLevel { get; init; }

        [JsonPropertyName("o")]
        public List<int> QuestObjectivesId { get; init; }

        [JsonConstructor]
        internal QuestStepCustomData()
        {
            QuestObjectivesId = [];
        }
    }

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
}
