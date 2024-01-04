using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestObjectiveTypeData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Description { get; init; }

    [JsonConstructor]
    internal QuestObjectiveTypeData()
    {
        Description = string.Empty;
    }
}
