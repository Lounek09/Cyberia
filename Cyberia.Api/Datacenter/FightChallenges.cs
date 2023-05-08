using System.Text.Json.Serialization;

namespace Cyberia.Api.DatacenterNS
{
    public sealed class FightChallenge
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        public FightChallenge()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class FightChallengesData
    {
        private const string FILE_NAME = "fightChallenge.json";

        [JsonPropertyName("FC")]
        public List<FightChallenge> FightChallenges { get; init; }

        public FightChallengesData()
        {
            FightChallenges = new();
        }

        internal static FightChallengesData Build()
        {
            return Json.LoadFromFile<FightChallengesData>($"{DofusApi.OUTPUT_PATH}/{FILE_NAME}");
        }
    }
}
