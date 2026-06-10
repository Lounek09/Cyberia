using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainXpFromLevelEffect : Effect
{
    public int Level { get; init; }
    public int RemainingPercent { get; init; }

    private CharacterGainXpFromLevelEffect(int id, int level, int remainingPercent)
        : base(id)
    {
        Level = level;
        RemainingPercent = remainingPercent;
    }

    internal static CharacterGainXpFromLevelEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Level, RemainingPercent);
    }
}
