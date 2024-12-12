using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemFragmentCompletionEffect : Effect
{
    public int ValueInt { get; init; }
    public int ValueDecimal { get; init; }

    private ItemFragmentCompletionEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int valueInt, int valueDecimal)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        ValueInt = valueInt;
        ValueDecimal = valueDecimal;
    }

    internal static ItemFragmentCompletionEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, ValueInt, ValueDecimal);
    }
}
