using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterTransformEffect
    : Effect, IEffect
{
    public int MonsterId { get; init; }

    private CharacterTransformEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId)
        : base(id, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
    }

    internal static CharacterTransformEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1);
    }

    public MonsterData? GetMonsterData()
    {
        return DofusApi.Datacenter.MonstersData.GetMonsterDataById(MonsterId);
    }

    public Description GetDescription()
    {
        var monsterName = DofusApi.Datacenter.MonstersData.GetMonsterNameById(MonsterId);

        return GetDescription(monsterName);
    }
}
