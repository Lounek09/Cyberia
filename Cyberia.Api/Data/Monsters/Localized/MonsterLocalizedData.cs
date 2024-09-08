using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Localized;

internal sealed class MonsterLocalizedData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("n")]
    public string Name { get; init; }

    [JsonConstructor]
    internal MonsterLocalizedData()
    {
        Name = string.Empty;
    }
}
