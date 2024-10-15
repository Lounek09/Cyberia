using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterPushEffect : Effect
{
    public int Distance { get; init; }

    private CharacterPushEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int distance)
        : base(id, duration, probability, criteria, effectArea)
    {
        Distance = distance;
    }

    internal static CharacterPushEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Distance);
    }
}
