using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GiveTTGCardFromRarityEffect : Effect
{
    public TTGCardRarity Rarity { get; init; }

    private GiveTTGCardFromRarityEffect(int id, TTGCardRarity rarity)
        : base(id)
    {
        Rarity = rarity;
    }

    internal static GiveTTGCardFromRarityEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (TTGCardRarity)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Rarity.GetDescription());
    }
}
