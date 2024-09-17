using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record PetLifePointsEffect : Effect
{
    public int LifePoints { get; init; }

    private PetLifePointsEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int lifePoints)
        : base(id, duration, probability, criteria, effectArea)
    {
        LifePoints = lifePoints;
    }

    internal static PetLifePointsEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, LifePoints);
    }
}
