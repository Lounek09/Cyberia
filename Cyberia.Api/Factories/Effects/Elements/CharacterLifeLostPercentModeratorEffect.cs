using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLifeLostPercentReductorEffect : Effect
{
    public int PercentReductor { get; init; }

    private CharacterLifeLostPercentReductorEffect(int id, int percentModerator)
        : base(id)
    {
        PercentReductor = percentModerator;
    }

    internal static CharacterLifeLostPercentReductorEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, PercentReductor);
    }
}
