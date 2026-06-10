using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemLivingMoodEffect : Effect
{
    public Corpulence Corpulence { get; init; }

    private ItemLivingMoodEffect(int id, Corpulence corpulence)
        : base(id)
    {
        Corpulence = corpulence;
    }

    internal static ItemLivingMoodEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (Corpulence)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Corpulence.GetDescription());
    }
}
