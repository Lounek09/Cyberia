using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters;

public sealed class MonsterRaceData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("s")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public int MonsterSuperRaceId { get; init; }

    [JsonConstructor]
    internal MonsterRaceData()
    {
        Name = string.Empty;
    }

    public MonsterSuperRaceData? GetMonsterSuperRaceData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterSuperRaceDataById(MonsterSuperRaceId);
    }
}
