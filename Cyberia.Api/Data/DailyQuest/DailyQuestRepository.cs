using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Enums;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.DailyQuest;

public sealed class DailyQuestRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "dailyquests.json";

    [JsonPropertyName("DQ")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DailyQuestData>))]
    public FrozenDictionary<int, DailyQuestData> DailyQuests { get; init; }

    [JsonConstructor]
    internal DailyQuestRepository()
    {
        DailyQuests = FrozenDictionary<int, DailyQuestData>.Empty;
    }

    public DailyQuestData? GetDailyQuestDataById(int id)
    {
        DailyQuests.TryGetValue(id, out var dailyQuestData);
        return dailyQuestData;
    }
}
