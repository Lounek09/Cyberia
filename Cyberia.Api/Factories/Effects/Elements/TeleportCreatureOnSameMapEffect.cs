using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record TeleportCreatureOnSameMapEffect : Effect
{
    public int MonsterId { get; init; }
    public int MaximumDistance { get; init; }

    private TeleportCreatureOnSameMapEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int monsterId, int maximumDistance)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
        MaximumDistance = maximumDistance;
    }

    internal static TeleportCreatureOnSameMapEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3, (int)parameters.Param1);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersRepository.GetMonsterDataById(MonsterId);
    }

    public override Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersRepository.GetMonsterNameById(MonsterId);

        return GetDescription(MaximumDistance, string.Empty, monsterName);
    }
}
