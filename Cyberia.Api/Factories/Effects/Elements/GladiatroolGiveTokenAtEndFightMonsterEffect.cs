using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolGiveTokenAtEndFightMonsterEffect : Effect
{
    public int Quantity { get; init; }

    private GladiatroolGiveTokenAtEndFightMonsterEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int quantity)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        Quantity = quantity;
    }

    internal static GladiatroolGiveTokenAtEndFightMonsterEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Quantity);
    }
}
