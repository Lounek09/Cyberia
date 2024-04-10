using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLoseHonourEffect : Effect, IEffect
{
    public int Honour { get; init; }

    private CharacterLoseHonourEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int honour)
        : base(id, duration, probability, criteria, effectArea)
    {
        Honour = honour;
    }

    internal static CharacterLoseHonourEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Honour);
    }
}
