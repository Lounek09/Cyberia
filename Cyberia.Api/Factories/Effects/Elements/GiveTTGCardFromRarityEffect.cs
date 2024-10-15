using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GiveTTGCardFromRarityEffect : Effect
{
    public TTGCardRarity Rarity { get; init; }

    private GiveTTGCardFromRarityEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, TTGCardRarity rarity)
        : base(id, duration, probability, criteria, effectArea)
    {
        Rarity = rarity;
    }

    internal static GiveTTGCardFromRarityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (TTGCardRarity)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Rarity.GetDescription());
    }
}
