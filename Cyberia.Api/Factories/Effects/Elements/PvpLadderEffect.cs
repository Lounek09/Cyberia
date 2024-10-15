using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PvpLadderEffect : Effect
{
    public int Count { get; init; }

    private PvpLadderEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int count)
        : base(id, duration, probability, criteria, effectArea)
    {
        Count = count;
    }

    internal static PvpLadderEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, Count);
    }
}
