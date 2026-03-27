using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PetPowerBoostEffect : Effect
{
    public int Power { get; init; }

    private PetPowerBoostEffect(int id, int power)
        : base(id)
    {
        Power = power;
    }

    internal static PetPowerBoostEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Power);
    }
}
