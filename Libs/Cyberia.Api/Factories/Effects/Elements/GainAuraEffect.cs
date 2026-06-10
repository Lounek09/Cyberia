using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GainAuraEffect : Effect
{
    public int AuraId { get; init; }

    private GainAuraEffect(int id, int auraId)
        : base(id)
    {
        AuraId = auraId;
    }

    internal static GainAuraEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, AuraId);
    }
}
