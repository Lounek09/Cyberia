using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record SizeEffect : Effect
{
    public int MinSize { get; init; }
    public int MaxSize { get; init; }
    public int ActualSize { get; init; }

    private SizeEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int min, int max, int actualSize)
        : base(id, duration, probability, criteria, effectArea)
    {
        MinSize = min;
        MaxSize = max;
        ActualSize = actualSize;
    }

    internal static SizeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, MinSize, MaxSize, ActualSize);
    }
}
