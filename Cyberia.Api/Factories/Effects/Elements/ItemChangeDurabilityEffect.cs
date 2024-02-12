using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemChangeDurabilityEffect
    : Effect, IEffect
{
    public int CurrentDurability { get; init; }
    public int MaxDurability { get; init; }

    private ItemChangeDurabilityEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int currentDurability, int maxDurability)
        : base(id, duration, probability, criteria, effectArea)
    {
        CurrentDurability = currentDurability;
        MaxDurability = maxDurability;
    }

    internal static ItemChangeDurabilityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, CurrentDurability, MaxDurability);
    }
}
