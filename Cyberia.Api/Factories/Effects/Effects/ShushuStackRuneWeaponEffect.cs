using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ShushuStackRuneWeaponEffect : Effect, IEffect<ShushuStackRuneWeaponEffect>
{
    public int Value { get; init; }

    private ShushuStackRuneWeaponEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int value)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Value = value;
    }

    public static ShushuStackRuneWeaponEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(Value);
    }
}
