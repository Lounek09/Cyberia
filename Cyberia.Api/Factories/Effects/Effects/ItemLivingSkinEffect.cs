using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemLivingSkinEffect : Effect, IEffect<ItemLivingSkinEffect>
{
    public int Number { get; init; }

    private ItemLivingSkinEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int number)
        : base(id, duration, probability, criteria, effectArea)
    {
        Number = number;
    }

    public static ItemLivingSkinEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, Number);
    }
}
