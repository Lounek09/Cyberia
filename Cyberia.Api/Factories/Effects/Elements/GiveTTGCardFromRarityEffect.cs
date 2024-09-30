using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

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

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Rarity.GetDescription());
    }
}
