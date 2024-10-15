using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterDodgeHitEffect : Effect
{
    public int DodgePercent { get; init; }
    public int CasePushed { get; init; }

    private CharacterDodgeHitEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int dodgePercent, int casePushed)
        : base(id, duration, probability, criteria, effectArea)
    {
        DodgePercent = dodgePercent;
        CasePushed = casePushed;
    }

    internal static CharacterDodgeHitEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, DodgePercent, CasePushed);
    }
}
