using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterReduceWeaponCostEffect : Effect, IEffect
{
    public int ActionPointsReduced { get; init; }

    private CharacterReduceWeaponCostEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int actionPointsReduced)
        : base(id, duration, probability, criteria, effectArea)
    {
        ActionPointsReduced = actionPointsReduced;
    }

    internal static CharacterReduceWeaponCostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, ActionPointsReduced);
    }
}
