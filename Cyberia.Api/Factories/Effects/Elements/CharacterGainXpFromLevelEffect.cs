using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

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

    public override Description GetDescription()
    {
        return GetDescription(Level, RemainingPercent);
    }
}
