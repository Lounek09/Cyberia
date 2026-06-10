using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterResurrectionEffect : Effect
{
    public int Energy { get; init; }

    private CharacterResurrectionEffect(int id, int energy)
        : base(id)
    {
        Energy = energy;
    }

    internal static CharacterResurrectionEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Energy);
    }
}
