using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Localized;

internal sealed class QuestLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("v")]
    public string Name { get; init; }

    [JsonConstructor]
    internal QuestLocalizedData()
    {
        Name = string.Empty;
    }
}
