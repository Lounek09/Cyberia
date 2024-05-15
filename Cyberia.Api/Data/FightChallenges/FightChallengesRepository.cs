using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges;

public sealed class FightChallengesRepository : IDofusRepository
{
    private const string c_fileName = "fightChallenge.json";

    [JsonPropertyName("FC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, FightChallengeData>))]
    public FrozenDictionary<int, FightChallengeData> FightChallenges { get; init; }

    [JsonConstructor]
    internal FightChallengesRepository()
    {
        FightChallenges = FrozenDictionary<int, FightChallengeData>.Empty;
    }

    internal static FightChallengesRepository Load(string directoryPath)
    {
        var filePath = Path.Join(directoryPath, c_fileName);

        return Datacenter.LoadRepository<FightChallengesRepository>(filePath);
    }

    public FightChallengeData? GetFightChallenge(int id)
    {
        FightChallenges.TryGetValue(id, out var fightChallenge);
        return fightChallenge;
    }
}
