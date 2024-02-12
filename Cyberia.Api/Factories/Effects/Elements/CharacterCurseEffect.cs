using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterCurseEffect
    : Effect, IEffect
{
    public int CurseId { get; init; }

    private CharacterCurseEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int curseId)
        : base(id, duration, probability, criteria, effectArea)
    {
        CurseId = curseId;
    }

    internal static CharacterCurseEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public Description GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, CurseId);
    }
}
