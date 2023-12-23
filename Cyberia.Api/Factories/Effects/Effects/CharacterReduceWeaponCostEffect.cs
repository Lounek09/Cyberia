using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterReduceWeaponCostEffect : Effect, IEffect<CharacterReduceWeaponCostEffect>
{
    public int ActionPointsReduced { get; init; }

    private CharacterReduceWeaponCostEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int actionPointsReduced)
        : base(id, duration, probability, criteria, effectArea)
    {
        ActionPointsReduced = actionPointsReduced;
    }

    public static CharacterReduceWeaponCostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, ActionPointsReduced);
    }
}
