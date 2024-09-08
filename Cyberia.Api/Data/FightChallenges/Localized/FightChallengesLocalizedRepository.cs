using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.FightChallenges.Localized;

internal sealed class FightChallengesLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => FightChallengesRepository.FileName;

    [JsonPropertyName("FC")]
    public IReadOnlyList<FightChallengeLocalizedData> FightChallenges { get; init; }

    [JsonConstructor]
    internal FightChallengesLocalizedRepository()
    {
        FightChallenges = [];
    }
}
