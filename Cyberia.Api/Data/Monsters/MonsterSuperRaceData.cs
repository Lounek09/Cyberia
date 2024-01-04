using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters;

public sealed class MonsterSuperRaceData
    : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonPropertyName("s")]
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonInclude]
    internal int MonsterSuperRaceId { get; init; }

    [JsonConstructor]
    internal MonsterSuperRaceData()
    {
        Name = string.Empty;
    }
}
