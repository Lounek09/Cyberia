using Cyberia.Api.Data.FightChallenges.Localized;
using Cyberia.Api.JsonConverters;
using Cyberia.Langzilla.Primitives;

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

    public FightChallengeData? GetFightChallengeDataById(int id)
    {
        FightChallenges.TryGetValue(id, out var fightChallengeData);
        return fightChallengeData;
    }

    protected override void LoadLocalizedData(LangsIdentifier identifier)
    {
        var twoLetterISOLanguageName = identifier.Language.ToStringFast();
        var localizedRepository = DofusLocalizedRepository.Load<FightChallengesLocalizedRepository>(identifier);

        foreach (var fightChallengeLocalizedData in localizedRepository.FightChallenges)
        {
            var fightChallengeData = GetFightChallengeDataById(fightChallengeLocalizedData.Id);
            if (fightChallengeData is not null)
            {
                fightChallengeData.Name.TryAdd(twoLetterISOLanguageName, fightChallengeLocalizedData.Name);
                fightChallengeData.Description.TryAdd(twoLetterISOLanguageName, fightChallengeLocalizedData.Description);
            }
        }
    }
}
