using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters.Custom;

internal sealed class MonstersCustomData : IDofusData
{
    [JsonPropertyName("CM")]
    public List<MonsterCustomData> MonstersCustom { get; init; }

    [JsonConstructor]
    internal MonstersCustomData()
    {
        MonstersCustom = [];
    }
}
