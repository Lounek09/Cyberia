using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterGainHonourEffect : Effect
{
    public int Honour { get; init; }

    private CharacterGainHonourEffect(int id, int honour)
        : base(id)
    {
        Honour = honour;
    }

    internal static CharacterGainHonourEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Honour);
    }
}
