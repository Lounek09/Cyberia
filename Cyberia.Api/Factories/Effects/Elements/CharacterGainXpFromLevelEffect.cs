using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainXpFromLevelEffect : Effect
{
    public int Level { get; init; }
    public int RemainingPercent { get; init; }

    private CharacterGainXpFromLevelEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int level, int remainingPercent)
        : base(id, duration, probability, criteria, effectArea)
    {
        Level = level;
        RemainingPercent = remainingPercent;
    }

    internal static CharacterGainXpFromLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Level, RemainingPercent);
    }
}
