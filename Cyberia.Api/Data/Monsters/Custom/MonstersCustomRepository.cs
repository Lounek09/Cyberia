using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Custom;

internal sealed class MonstersCustomRepository : DofusCustomRepository, IDofusRepository
{
    public static string FileName => MonstersRepository.FileName;

    [JsonPropertyName("CM")]
    public IReadOnlyList<MonsterCustomData> MonstersCustom { get; init; }

    [JsonConstructor]
    internal MonstersCustomRepository()
    {
        MonstersCustom = ReadOnlyCollection<MonsterCustomData>.Empty;
    }
}
