using Cyberia.Api.Data;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record SummonStaticMonsterInFightEffect : Effect, IEffect<SummonStaticMonsterInFightEffect>
{
    public int MonsterId { get; init; }
    public int Grade { get; init; }

    private SummonStaticMonsterInFightEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId, int grade)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        Grade = grade;
    }

    public static SummonStaticMonsterInFightEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
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
