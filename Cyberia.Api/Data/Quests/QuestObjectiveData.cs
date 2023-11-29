using Cyberia.Api.JsonConverters;

using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Quests;

public sealed class QuestObjectiveData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("t")]
    public int QuestObjectiveTypeId { get; init; }

    [JsonPropertyName("p")]
    [JsonConverter(typeof(ToStringListConverter))]
    public IReadOnlyList<string> Parameters { get; init; }

    [JsonPropertyName("x")]
    public int? XCoord { get; init; }

    [JsonPropertyName("y")]
    public int? YCoord { get; init; }

    [JsonConstructor]
    internal QuestObjectiveData()
    {
        Parameters = [];
    }

    public QuestObjectiveTypeData? GetQuestObjectiveTypeData()
    {
        return DofusApi.Datacenter.QuestsData.GetQuestObjectiveTypeDataById(QuestObjectiveTypeId);
    }

    public string GetCoordinate()
    {
        return XCoord.HasValue && YCoord.HasValue
            ? $"[{XCoord}, {YCoord}]"
            : string.Empty;
    }
}
