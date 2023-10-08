using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS.Custom
{
    internal sealed class MonsterCustomData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("bs")]
        public bool BreedSummon { get; init; }

        [JsonPropertyName("t")]
        public string TrelloUrl { get; init; }

        public MonsterCustomData()
        {
            TrelloUrl = string.Empty;
        }
    }

    internal sealed class MonstersCustomData
    {
        [JsonPropertyName("CM")]
        public List<MonsterCustomData> MonstersCustom { get; init; }

        public MonstersCustomData()
        {
            MonstersCustom = new();
        }
    }
}
