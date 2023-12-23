using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;
using Cyberia.Api.Values;

namespace Cyberia.Api.Factories.Effects;

public sealed record ItemPetsShapeEffect : Effect, IEffect<ItemPetsShapeEffect>
{
    public Corpulence Corpulence { get; init; }

    private ItemPetsShapeEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, Corpulence corpulence)
        : base(id, duration, probability, criteria, effectArea)
    {
        Corpulence = corpulence;
    }

    public static ItemPetsShapeEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        var corpulence = parameters.Param2 <= 6 ? parameters.Param3 <= 6 ? Corpulence.Satisfied : Corpulence.Skinny : Corpulence.Obese;

        return new(effectId, duration, probability, criteria, effectArea, corpulence);
    }

    public Description GetDescription()
    {
        return GetDescription(Corpulence.GetDescription());
    }
}
