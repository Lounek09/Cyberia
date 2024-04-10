using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record AddMonsterToFightEffect : Effect, IEffect
{
    public int MonsterId { get; init; }
    public int Grade { get; init; }

    private AddMonsterToFightEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int monsterId, int grade)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        Grade = grade;
    }

    internal static AddMonsterToFightEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

        return GetDescription(monsterName, Grade);
    }
}
