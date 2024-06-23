using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterDisplaySpellAnimation2Effect : Effect
{
    public int GfxId { get; init; }

    private CharacterDisplaySpellAnimation2Effect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int gfxId)
        : base(id, duration, probability, criteria, effectArea)
    {
        GfxId = gfxId;
    }

    internal static CharacterDisplaySpellAnimation2Effect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, GfxId);
    }
}
