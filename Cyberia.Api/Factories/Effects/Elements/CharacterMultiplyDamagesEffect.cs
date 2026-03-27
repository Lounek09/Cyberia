using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterMultiplyDamagesEffect : Effect
{
    public int Multiplier { get; init; }

    private CharacterMultiplyDamagesEffect(int id, int multiplier)
        : base(id)
    {
        Multiplier = multiplier;
    }

    internal static CharacterMultiplyDamagesEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Multiplier);
    }
}
