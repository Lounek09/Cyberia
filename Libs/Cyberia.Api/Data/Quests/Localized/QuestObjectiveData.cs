using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests.Localized;

internal sealed class QuestObjectiveLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("p")]
    [JsonConverter(typeof(TypeCoercingReadOnlyListConverter<string>))]
    public IReadOnlyList<string> Parameters { get; init; }

    [JsonConstructor]
    internal QuestObjectiveLocalizedData()
    {
        Parameters = ReadOnlyCollection<string>.Empty;
    }
}
