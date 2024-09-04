using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges;

public sealed class FightChallengesRepository : DofusRepository, IDofusRepository
{
    public static string FileName => "fightChallenge.json";

    [JsonPropertyName("FC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, FightChallengeData>))]
    public FrozenDictionary<int, FightChallengeData> FightChallenges { get; init; }

    [JsonConstructor]
    internal FightChallengesRepository()
    {
        FightChallenges = FrozenDictionary<int, FightChallengeData>.Empty;
    }

    public FightChallengeData? GetFightChallenge(int id)
    {
        FightChallenges.TryGetValue(id, out var fightChallenge);
        return fightChallenge;
    }
}
