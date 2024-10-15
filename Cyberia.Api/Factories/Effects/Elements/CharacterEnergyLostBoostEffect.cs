using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterEnergyLossBoostEffect : Effect
{
    public int EnergyLoss { get; init; }

    private CharacterEnergyLossBoostEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int energyLoss)
        : base(id, duration, probability, criteria, effectArea)
    {
        EnergyLoss = energyLoss;
    }

    internal static CharacterEnergyLossBoostEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, EnergyLoss);
    }
}
