using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Localized;

internal sealed class QuestObjectiveTypeLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Description { get; init; }

    [JsonConstructor]
    internal QuestObjectiveTypeLocalizedData()
    {
        Description = string.Empty;
    }
}
