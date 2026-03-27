using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainDishonourEffect : Effect
{
    public int Dishonour { get; init; }

    private CharacterGainDishonourEffect(int id, int dishonour)
        : base(id)
    {
        Dishonour = dishonour;
    }

    internal static CharacterGainDishonourEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Dishonour);
    }
}
