using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Custom;

internal sealed class MonstersCustomData
    : IDofusData
{
    [JsonPropertyName("CM")]
    public IReadOnlyList<MonsterCustomData> MonstersCustom { get; init; }

    [JsonConstructor]
    internal MonstersCustomData()
    {
        MonstersCustom = [];
    }
}
