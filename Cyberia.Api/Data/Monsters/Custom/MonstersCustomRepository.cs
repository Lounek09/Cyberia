using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Custom;

internal sealed class MonstersCustomRepository : IDofusRepository
{
    [JsonPropertyName("CM")]
    public IReadOnlyList<MonsterCustomData> MonstersCustom { get; init; }

    [JsonConstructor]
    internal MonstersCustomRepository()
    {
        MonstersCustom = [];
    }
}
