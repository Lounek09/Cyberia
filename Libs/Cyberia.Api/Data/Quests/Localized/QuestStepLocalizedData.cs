using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Localized;

internal sealed class QuestStepLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("d")]
    public string Description { get; init; }

    [JsonConstructor]
    internal QuestStepLocalizedData()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
