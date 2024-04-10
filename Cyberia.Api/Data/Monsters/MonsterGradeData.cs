using System.Text.Json.Serialization;

namespace Cyberia.Api.Data.Monsters;

public sealed class MonsterGradeData : IDofusData
{
    [JsonPropertyName("l")]
    public int Level { get; init; }

    [JsonPropertyName("r")]
    public IReadOnlyList<int> Resistances { get; init; }

    [JsonPropertyName("lp")]
    public int? LifePoint { get; init; }

    [JsonPropertyName("ap")]
    public int? ActionPoint { get; init; }

    [JsonPropertyName("mp")]
    public int? MovementPoint { get; init; }

    [JsonIgnore]
    public MonsterData MonsterData { get; internal set; }

    [JsonIgnore]
    public int Rank { get; internal set; }

    [JsonConstructor]
    internal MonsterGradeData()
    {
        Resistances = [];
        MonsterData = new();
    }

    public int GetNeutralResistance()
    {
        return Resistances.Count > 0 ? Resistances[0] : 0;
    }

    public int GetEarthResistance()
    {
        return Resistances.Count > 1 ? Resistances[1] : 0;
    }

    public int GetFireResistance()
    {
        return Resistances.Count > 2 ? Resistances[2] : 0;
    }

    public int GetWaterResistance()
    {
        return Resistances.Count > 3 ? Resistances[3] : 0;
    }

    public int GetAirResistance()
    {
        return Resistances.Count > 4 ? Resistances[4] : 0;
    }

    public int GetActionPointDodge()
    {
        return Resistances.Count > 5 ? Resistances[5] : 0;
    }

    public int GetMovementPointDodge()
    {
        return Resistances.Count > 6 ? Resistances[6] : 0;
    }
}
