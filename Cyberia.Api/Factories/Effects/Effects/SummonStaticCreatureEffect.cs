using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record SummonStaticCreatureEffect : Effect, IEffect<SummonStaticCreatureEffect>
{
    public int MonsterId { get; init; }
    public int Grade { get; init; }

    private SummonStaticCreatureEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId, int grade)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        Grade = grade;
    }

    public static SummonStaticCreatureEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
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
