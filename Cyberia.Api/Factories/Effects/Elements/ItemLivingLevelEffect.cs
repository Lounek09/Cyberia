using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemLivingLevelEffect
    : Effect, IEffect
{
    public int Experience { get; init; }

    private ItemLivingLevelEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int experience)
        : base(id, duration, probability, criteria, effectArea)
    {
        Experience = experience;
    }

    internal static ItemLivingLevelEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Experience);
    }
}
