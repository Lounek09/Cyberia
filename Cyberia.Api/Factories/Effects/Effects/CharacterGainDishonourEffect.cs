using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterGainDishonourEffect : Effect, IEffect<CharacterGainDishonourEffect>
{
    public int Dishonour { get; init; }

    private CharacterGainDishonourEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int dishonour)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Dishonour = dishonour;
    }

    public static CharacterGainDishonourEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Dishonour);
    }
}
