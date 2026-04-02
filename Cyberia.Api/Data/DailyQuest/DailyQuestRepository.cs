using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.DailyQuest;

public sealed class DailyQuestRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "dailyquests.json";

    [JsonPropertyName("DQ")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DailyQuestData>))]
    public FrozenDictionary<int, DailyQuestData> DailyQuests { get; init; }

    [JsonPropertyName("DQI")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, DailyQuestData>))]
    public FrozenDictionary<int, DailyQuestIdData> DailyQuestIds { get; init; }

    [JsonConstructor]
    internal DailyQuestRepository()
    {
        DailyQuests = FrozenDictionary<int, DailyQuestData>.Empty;
        DailyQuestIds = FrozenDictionary<int, DailyQuestIdData>.Empty;
    }

    public DailyQuestData? GetDailyQuestDataById(int id)
    {
        DailyQuests.TryGetValue(id, out var dailyQuestData);
        return dailyQuestData;
    }

    public DailyQuestIdData? GetDailyQuestIdDataById(int id)
    {
        DailyQuestIds.TryGetValue(id, out var dailyQuestIdData);
        return dailyQuestIdData;
    }
}
