using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record SetCraftermageEffect : Effect
{
    public string Name { get; init; }

    private SetCraftermageEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, string name)
        : base(id, duration, probability, criteria, effectArea)
    {
        Name = name;
    }

    internal static SetCraftermageEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param4);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, string.Empty, Name);
    }
}
