using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLoseDishonourEffect : Effect, IEffect
{
    public int Dishonnour { get; init; }

    private CharacterLoseDishonourEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int dishonnour)
        : base(id, duration, probability, criteria, effectArea)
    {
        Dishonnour = dishonnour;
    }

    internal static CharacterLoseDishonourEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Dishonnour);
    }
}
