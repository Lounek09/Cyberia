using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterChangeColorEffect : Effect
{
    public int Position { get; init; }
    public int Color { get; init; }

    private CharacterChangeColorEffect(int id, int position, int color)
        : base(id)
    {
        Position = position;
        Color = color;
    }

    internal static CharacterChangeColorEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Position, Color);
    }
}
