using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterChangeColorEffect : Effect
{
    public int Position { get; init; }
    public int Color { get; init; }

    private CharacterChangeColorEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int position, int color)
        : base(id, duration, probability, criteria, effectArea)
    {
        Position = position;
        Color = color;
    }

    internal static CharacterChangeColorEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Position, Color);
    }
}
