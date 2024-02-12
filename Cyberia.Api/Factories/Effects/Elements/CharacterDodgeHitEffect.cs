using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterDodgeHitEffect
    : Effect, IEffect
{
    public int DodgePercent { get; init; }
    public int CasePushed { get; init; }

    private CharacterDodgeHitEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int dodgePercent, int casePushed)
        : base(id, duration, probability, criteria, effectArea)
    {
        DodgePercent = dodgePercent;
        CasePushed = casePushed;
    }

    internal static CharacterDodgeHitEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public Description GetDescription()
    {
        return GetDescription(DodgePercent, CasePushed);
    }
}
