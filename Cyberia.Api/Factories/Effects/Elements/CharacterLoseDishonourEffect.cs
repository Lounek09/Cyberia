using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLoseDishonourEffect : Effect
{
    public int Dishonnour { get; init; }

    private CharacterLoseDishonourEffect(int id, int dishonnour)
        : base(id)
    {
        Dishonnour = dishonnour;
    }

    internal static CharacterLoseDishonourEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Dishonnour);
    }
}
