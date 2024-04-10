using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Custom;

internal sealed class MonsterCustomData : IDofusData<int>
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("bs")]
    public bool BreedSummon { get; init; }

    [JsonPropertyName("t")]
    public string TrelloUrl { get; init; }

    [JsonConstructor]
    internal MonsterCustomData()
    {
        TrelloUrl = string.Empty;
    }
}
