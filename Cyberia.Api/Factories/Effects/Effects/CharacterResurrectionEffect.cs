using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterResurrectionEffect : Effect, IEffect<CharacterResurrectionEffect>
{
    public int Energy { get; init; }

    private CharacterResurrectionEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int energy)
        : base(id, duration, probability, criteria, effectArea)
    {
        Energy = energy;
    }

    public static CharacterResurrectionEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Energy);
    }
}
