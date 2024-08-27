using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterCurseEffect : Effect
{
    public int CurseId { get; init; }

    private CharacterCurseEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int curseId)
        : base(id, duration, probability, criteria, effectArea)
    {
        CurseId = curseId;
    }

    internal static CharacterCurseEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription()
    {
        return GetDescription(string.Empty, string.Empty, CurseId);
    }
}
