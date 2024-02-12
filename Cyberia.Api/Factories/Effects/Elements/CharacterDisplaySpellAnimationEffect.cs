using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterDisplaySpellAnimationEffect
    : Effect, IEffect
{
    public int GfxId { get; init; }

    private CharacterDisplaySpellAnimationEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int gfxId)
        : base(id, duration, probability, criteria, effectArea)
    {
        GfxId = gfxId;
    }

    internal static CharacterDisplaySpellAnimationEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, GfxId);
    }
}
