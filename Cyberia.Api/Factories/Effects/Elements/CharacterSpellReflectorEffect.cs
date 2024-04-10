using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterSpellReflectorEffect
    : Effect, IEffect
{
    public int Level { get; init; }

    private CharacterSpellReflectorEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int level)
        : base(id, duration, probability, criteria, effectArea)
    {
        Level = level;
    }

    internal static CharacterSpellReflectorEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, Level);
    }
}
