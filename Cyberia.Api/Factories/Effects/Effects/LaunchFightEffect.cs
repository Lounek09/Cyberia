using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record LaunchFightEffect : Effect, IEffect<LaunchFightEffect>
{
    public int MonsterId { get; init; }

    private LaunchFightEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
    }

    public static LaunchFightEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param2);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

        return GetDescription(null, monsterName);
    }
}
