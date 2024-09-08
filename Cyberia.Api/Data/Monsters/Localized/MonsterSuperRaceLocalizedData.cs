using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Localized;

internal sealed class MonsterSuperRaceLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal MonsterSuperRaceLocalizedData()
    {
        Name = string.Empty;
    }
}
