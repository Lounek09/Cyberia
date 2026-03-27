using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterEnergyLossBoostEffect : Effect
{
    public int EnergyLoss { get; init; }

    private CharacterEnergyLossBoostEffect(int id, int energyLoss)
        : base(id)
    {
        EnergyLoss = energyLoss;
    }

    internal static CharacterEnergyLossBoostEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, EnergyLoss);
    }
}
