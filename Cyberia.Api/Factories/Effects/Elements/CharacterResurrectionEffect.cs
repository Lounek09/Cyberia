using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterResurrectionEffect : Effect, IEffect
{
    public int Energy { get; init; }

    private CharacterResurrectionEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int energy)
        : base(id, duration, probability, criteria, effectArea)
    {
        Energy = energy;
    }

    internal static CharacterResurrectionEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, Energy);
    }
}
