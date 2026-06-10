using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Data.Quests;

using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.WantedDocuments;

public sealed class WantedDocumentData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("p")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int DocumentId { get; init; }

    [JsonPropertyName("b")]
    public LocalizedString Body { get; init; }

    [JsonPropertyName("d")]
    public LocalizedString Description { get; init; }

    [JsonPropertyName("q")]
    public int QuestId { get; init; }

    [JsonPropertyName("r")]
    public int KamasReward { get; init; }

    [JsonPropertyName("m")]
    public int MonsterId { get; init; }

    [JsonPropertyName("s")]
    public int Score { get; init; }

    [JsonConstructor]
    internal WantedDocumentData()
    {
        Body = LocalizedString.Empty;
        Description = LocalizedString.Empty;
    }

    public QuestData? GetQuestData()
    {
        return DofusApi.Datacenter.QuestsRepository.GetQuestDataById(QuestId);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }
}
