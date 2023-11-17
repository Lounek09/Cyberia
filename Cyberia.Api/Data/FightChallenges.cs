using System.Text.Json.Serialization;

namespace Cyberia.Api.Data
{
    public sealed class FightChallengeData
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("n")]
        public string Name { get; init; }

        [JsonPropertyName("d")]
        public string Description { get; init; }

        [JsonPropertyName("g")]
        public int GfxId { get; init; }

        [JsonConstructor]
        internal FightChallengeData()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }

    public sealed class FightChallengesData
    {
        private const string FILE_NAME = "fightChallenge.json";

        [JsonPropertyName("FC")]
        public List<FightChallengeData> FightChallenges { get; init; }

        [JsonConstructor]
        internal FightChallengesData()
        {
            FightChallenges = [];
        }

        internal static FightChallengesData Load()
        {
            return Datacenter.LoadDataFromFile<FightChallengesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
        }
    }
}
