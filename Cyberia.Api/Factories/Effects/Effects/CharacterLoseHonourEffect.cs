using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLoseHonourEffect : Effect, IEffect<CharacterLoseHonourEffect>
{
    public int Honour { get; init; }

    private CharacterLoseHonourEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int honour)
        : base(id, duration, probability, criteria, effectArea)
    {
        Honour = honour;
    }

    public static CharacterLoseHonourEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Honour);
    }
}
