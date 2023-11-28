using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges;

public sealed class FightChallengesData : IDofusData
{
    private const string FILE_NAME = "fightChallenge.json";

    [JsonPropertyName("FC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, FightChallengeData>))]
    public FrozenDictionary<int, FightChallengeData> FightChallenges { get; init; }

    [JsonConstructor]
    internal FightChallengesData()
    {
        FightChallenges = FrozenDictionary<int, FightChallengeData>.Empty;
    }

    internal static FightChallengesData Load()
    {
        return Datacenter.LoadDataFromFile<FightChallengesData>(Path.Combine(DofusApi.OUTPUT_PATH, FILE_NAME));
    }

    public FightChallengeData? GetFightChallenge(int id)
    {
        FightChallenges.TryGetValue(id, out var fightChallenge);
        return fightChallenge;
    }
}
