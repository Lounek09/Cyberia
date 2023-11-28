using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonIgnore]
    public bool Repeatable { get; internal set; }

    [JsonIgnore]
    public bool Account { get; internal set; }

    [JsonIgnore]
    public bool HasDungeon { get; internal set; }

    [JsonIgnore]
    public ReadOnlyCollection<int> QuestStepsId { get; internal set; }

    [JsonConstructor]
    internal QuestData()
    {
        Name = string.Empty;
        QuestStepsId = ReadOnlyCollection<int>.Empty;
    }

    public IEnumerable<QuestStepData> GetQuestStepsData()
    {
        foreach (var questStepId in QuestStepsId)
        {
            var questStepData = DofusApi.Datacenter.QuestsData.GetQuestStepDataById(questStepId);
            if (questStepData is not null)
            {
                yield return questStepData;
            }
        }
    }
}
