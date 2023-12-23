using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterSpellReflectorEffect : Effect, IEffect<CharacterSpellReflectorEffect>
{
    public int Level { get; init; }

    private CharacterSpellReflectorEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int level)
        : base(id, duration, probability, criteria, effectArea)
    {
        Level = level;
    }

    public static CharacterSpellReflectorEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(null, Level);
    }
}
