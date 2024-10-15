using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterSpellReflectorEffect : Effect
{
    public int Level { get; init; }

    private CharacterSpellReflectorEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int level)
        : base(id, duration, probability, criteria, effectArea)
    {
        Level = level;
    }

    internal static CharacterSpellReflectorEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, Level);
    }
}
