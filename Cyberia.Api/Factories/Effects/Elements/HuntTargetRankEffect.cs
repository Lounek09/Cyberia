using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record HuntTargetRankEffect
    : Effect, IEffect
{
    public int Rank { get; init; }

    private HuntTargetRankEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int rank)
        : base(id, duration, probability, criteria, effectArea)
    {
        Rank = rank;
    }

    internal static HuntTargetRankEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Rank);
    }
}
