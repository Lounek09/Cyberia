using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemEvolutiveSkinEffect : Effect
{
    public int Current { get; init; }
    public int Max { get; init; }

    private ItemEvolutiveSkinEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int current, int max)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        Current = current;
        Max = max;
    }

    internal static ItemEvolutiveSkinEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Max, string.Empty, Current);
    }
}
