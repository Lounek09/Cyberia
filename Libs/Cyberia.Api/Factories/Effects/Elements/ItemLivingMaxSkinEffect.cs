using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemLivingMaxSkinEffect : Effect
{
    public int Number { get; init; }

    private ItemLivingMaxSkinEffect(int id, int number)
        : base(id)
    {
        Number = number;
    }

    internal static ItemLivingMaxSkinEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Number);
    }
}
