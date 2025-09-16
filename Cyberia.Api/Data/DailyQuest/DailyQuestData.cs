using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.DailyQuest;

public sealed class DailyQuestData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("i")]
    [JsonInclude]
    internal int DailyQuestId { get; init; }

    [JsonPropertyName("q")]
    public int QuestId { get; init; }

    [JsonPropertyName("m")]
    public int MapId { get; init; }

    [JsonPropertyName("n")]
    public DailyQuestNpcData NpcData { get; init; }

    [JsonConstructor]
    internal DailyQuestData()
    {
        NpcData = new();
    }
}
