using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges;

public sealed class FightChallengesData
    : IDofusData
{
    private const string FILE_NAME = "fightChallenge.json";
    private static readonly string FILE_PATH = Path.Join(DofusApi.OUTPUT_PATH, FILE_NAME);

    [JsonPropertyName("FC")]
    [JsonConverter(typeof(DofusDataFrozenDictionaryConverter<int, FightChallengeData>))]
    public FrozenDictionary<int, FightChallengeData> FightChallenges { get; init; }

    [JsonConstructor]
    internal FightChallengesData()
    {
        FightChallenges = FrozenDictionary<int, FightChallengeData>.Empty;
    }

    internal static async Task<FightChallengesData> LoadAsync()
    {
        return await Datacenter.LoadDataAsync<FightChallengesData>(FILE_PATH);
    }

    public FightChallengeData? GetFightChallenge(int id)
    {
        FightChallenges.TryGetValue(id, out var fightChallenge);
        return fightChallenge;
    }
}
