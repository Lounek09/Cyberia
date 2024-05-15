using Cyberia.Api.Data.Monsters;

namespace Cyberia.Api.Factories.Criteria;

public sealed record MonsterSummonCriterion : Criterion, ICriterion
{
    public int MonsterId { get; init; }

    public MonsterSummonCriterion(string id, char @operator, int monsterId)
        : base(id, @operator)
    {
        MonsterId = monsterId;
    }

    internal static MonsterSummonCriterion? Create(string id, char @operator, params string[] parameters)
    {
        if (parameters.Length > 0 && int.TryParse(parameters[0], out var monsterId))
        {
            return new(id, @operator, monsterId);
        }

        return null;
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    protected override string GetDescriptionName()
    {
        return $"Criterion.MonsterSummon.{GetOperatorDescriptionName()}";
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId);

        return GetDescription(monsterName);
    }
}
