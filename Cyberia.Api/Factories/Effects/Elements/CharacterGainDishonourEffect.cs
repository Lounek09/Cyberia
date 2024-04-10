using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterGainDishonourEffect : Effect, IEffect
{
    public int Dishonour { get; init; }

    private CharacterGainDishonourEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int dishonour)
        : base(id, duration, probability, criteria, effectArea)
    {
        Dishonour = dishonour;
    }

    internal static CharacterGainDishonourEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Dishonour);
    }
}
