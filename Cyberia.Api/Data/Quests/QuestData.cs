using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public LocalizedString Name { get; init; }

    [JsonPropertyName("s")]
    public IReadOnlyList<int> QuestStepsId { get; init; }

    [JsonIgnore]
    public bool Repeatable { get; internal set; }

    [JsonIgnore]
    public bool Account { get; internal set; }

    [JsonIgnore]
    public bool HasDungeon { get; internal set; }

    [JsonConstructor]
    internal QuestData()
    {
        Name = LocalizedString.Empty;
        QuestStepsId = [];
    }

    public IEnumerable<QuestStepData> GetQuestStepsData()
    {
        foreach (var questStepId in QuestStepsId)
        {
            var questStepData = DofusApi.Datacenter.QuestsRepository.GetQuestStepDataById(questStepId);
            if (questStepData is not null)
            {
                yield return questStepData;
            }
        }
    }
}
