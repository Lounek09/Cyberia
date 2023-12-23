using Cyberia.Api.Data.Monsters;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterTransform2Effect : Effect, IEffect<CharacterTransform2Effect>
{
    public int MonsterId { get; init; }

    private CharacterTransform2Effect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int monsterId)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        MonsterId = monsterId;
    }

    public static CharacterTransform2Effect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
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
