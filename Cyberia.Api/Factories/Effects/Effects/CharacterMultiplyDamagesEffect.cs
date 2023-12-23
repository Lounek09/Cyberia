using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterMultiplyDamagesEffect : Effect, IEffect<CharacterMultiplyDamagesEffect>
{
    public int Multiplier { get; init; }

    private CharacterMultiplyDamagesEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int multiplier, int casePushed)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Multiplier = multiplier;
    }

    public static CharacterMultiplyDamagesEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param1, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(Multiplier);
    }
}
