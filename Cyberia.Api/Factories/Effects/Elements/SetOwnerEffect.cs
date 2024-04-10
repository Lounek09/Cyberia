using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record SetOwnerEffect
    : Effect, IEffect
{
    public string Name { get; init; }

    private SetOwnerEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, string name)
        : base(id, duration, probability, criteria, effectArea)
    {
        Name = name;
    }

    internal static SetOwnerEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param4);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, string.Empty, Name);
    }
}
