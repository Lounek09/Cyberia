using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemCeremonialChangeDurabilityEffect : Effect, IEffect<ItemCeremonialChangeDurabilityEffect>
{
    public int CurrentDurability { get; init; }
    public int MaxDurability { get; init; }

    private ItemCeremonialChangeDurabilityEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int currentDurability, int maxDurability)
        : base(id, duration, probability, criteria, effectArea)
    {
        CurrentDurability = currentDurability;
        MaxDurability = maxDurability;
    }

    public static ItemCeremonialChangeDurabilityEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param2, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, CurrentDurability, MaxDurability);
    }
}
