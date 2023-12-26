using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record TeleportCreatureOnSameMapEffect : Effect, IEffect<TeleportCreatureOnSameMapEffect>
{
    public int MonsterId { get; init; }
    public int MaximumDistance { get; init; }

    private TeleportCreatureOnSameMapEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId, int maximumDistance)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        MaximumDistance = maximumDistance;
    }

    public static TeleportCreatureOnSameMapEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3, parameters.Param1);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

        return GetDescription(MaximumDistance, null, monsterName);
    }
}
