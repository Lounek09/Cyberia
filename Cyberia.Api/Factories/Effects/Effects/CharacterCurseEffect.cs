using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.Effects.Templates;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterCurseEffect : Effect, IEffect<CharacterCurseEffect>
{
    public int CurseId { get; init; }

    private CharacterCurseEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int curseId)
        : base(id, duration, probability, criteria, effectArea)
    {
        CurseId = curseId;
    }

    public static CharacterCurseEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(CurseId);
    }
}
