using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Custom;

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

    [JsonConstructor]
    internal QuestCustomData()
    {

    }
}
