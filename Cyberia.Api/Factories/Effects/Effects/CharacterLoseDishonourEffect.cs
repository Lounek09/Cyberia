using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLoseDishonourEffect : Effect, IEffect<CharacterLoseDishonourEffect>
{
    public int Dishonnour { get; init; }

    private CharacterLoseDishonourEffect(int effectId, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int dishonnour)
        : base(effectId, duration, probability, criteria, effectArea)
    {
        Dishonnour = dishonnour;
    }

    public static CharacterLoseDishonourEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Dishonnour);
    }
}
