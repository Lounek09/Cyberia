using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Localized;

internal sealed class MonstersLocalizedRepository : DofusLocalizedRepository, IDofusRepository
{
    public static string FileName => MonstersRepository.FileName;

    [JsonPropertyName("MSR")]
    public IReadOnlyList<MonsterSuperRaceLocalizedData> MonsterSuperRaces { get; init; }

    [JsonPropertyName("MR")]
    public IReadOnlyList<MonsterRaceLocalizedData> MonsterRaces { get; init; }

    [JsonPropertyName("M")]
    public IReadOnlyList<MonsterLocalizedData> Monsters { get; init; }

    [JsonConstructor]
    internal MonstersLocalizedRepository()
    {
        MonsterSuperRaces = ReadOnlyCollection<MonsterSuperRaceLocalizedData>.Empty;
        MonsterRaces = ReadOnlyCollection<MonsterRaceLocalizedData>.Empty;
        Monsters = ReadOnlyCollection<MonsterLocalizedData>.Empty;
    }
}
