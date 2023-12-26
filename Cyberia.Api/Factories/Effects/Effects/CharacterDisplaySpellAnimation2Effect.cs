using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterDisplaySpellAnimation2Effect : Effect, IEffect<CharacterDisplaySpellAnimation2Effect>
{
    public int GfxId { get; init; }

    private CharacterDisplaySpellAnimation2Effect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int gfxId)
        : base(id, duration, probability, criteria, effectArea)
    {
        GfxId = gfxId;
    }

    public static CharacterDisplaySpellAnimation2Effect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(null, null, GfxId);
    }
}
