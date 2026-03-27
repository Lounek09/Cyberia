using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemPetsShapeEffect : Effect
{
    public Corpulence Corpulence { get; init; }

    private ItemPetsShapeEffect(int id, Corpulence corpulence)
        : base(id)
    {
        Corpulence = corpulence;
    }

    internal static ItemPetsShapeEffect Create(int effectId, EffectParameters parameters)
    {
        var corpulence = parameters.Param2 <= 6 ? parameters.Param3 <= 6 ? Corpulence.Satiated : Corpulence.Skinny : Corpulence.Obese;

        return new(effectId, corpulence);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Corpulence.GetDescription(culture));
    }
}
