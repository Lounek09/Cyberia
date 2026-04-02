using Cyberia.Api.Data.Quests;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.DailyQuest;

public sealed class DailyQuestIdData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public int QuestId { get; init; }

    [JsonConstructor]
    internal DailyQuestIdData() { }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestDataById(QuestId);
    }
}
