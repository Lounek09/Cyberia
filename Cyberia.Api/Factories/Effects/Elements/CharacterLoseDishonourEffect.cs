using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLoseDishonourEffect : Effect
{
    public int Dishonnour { get; init; }

    private CharacterLoseDishonourEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int dishonnour)
        : base(id, duration, probability, criteria, effectArea)
    {
        Dishonnour = dishonnour;
    }

    internal static CharacterLoseDishonourEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Dishonnour);
    }
}
