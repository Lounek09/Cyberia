using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Localized;

internal sealed class MonsterRaceLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal MonsterRaceLocalizedData()
    {
        Name = string.Empty;
    }
}
