using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LadderIdEffect : Effect, IEffect<LadderIdEffect>
{
    public int MonsterId { get; init; }
    public int Count { get; init; }

    private LadderIdEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId, int count)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        Count = count;
    }

    public static LadderIdEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param3);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

        return GetDescription(monsterName, null, Count);
    }
}
