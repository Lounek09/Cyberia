using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    internal sealed class MonsterCustom
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("bs")]
        public bool BreedSummon { get; init; }

        [JsonPropertyName("t")]
        public string TrelloUrl { get; init; }

        public MonsterCustom()
        {
            TrelloUrl = string.Empty;
        }
    }

    internal sealed class MonstersCustomData
    {
        [JsonPropertyName("CM")]
        public List<MonsterCustom> MonstersCustom { get; init; }

        public MonstersCustomData()
        {
            MonstersCustom = new();
        }
    }
}
