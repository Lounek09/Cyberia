using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterMapResurectionAlliesEffect : Effect
{
    public int Energy { get; init; }

    private CharacterMapResurectionAlliesEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int energy)
        : base(id, duration, probability, criteria, effectArea)
    {
        Energy = energy;
    }

    internal static CharacterMapResurectionAlliesEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Energy);
    }
}
