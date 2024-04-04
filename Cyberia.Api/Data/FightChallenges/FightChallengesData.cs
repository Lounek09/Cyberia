using Cyberia.Api.JsonConverters;

using System.Collections.Frozen;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges;

public sealed class FightChallengesData
    : IDofusData
{
    private const string c_fileName = "fightChallenge.json";

    private static readonly string s_filePath = Path.Join(DofusApi.OutputPath, c_fileName);

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
        return await Datacenter.LoadDataAsync<FightChallengesData>(s_filePath);
    }

    public FightChallengeData? GetFightChallenge(int id)
    {
        FightChallenges.TryGetValue(id, out var fightChallenge);
        return fightChallenge;
    }
}
