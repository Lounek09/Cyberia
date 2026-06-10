using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterMapResurectionAlliesEffect : Effect
{
    public int Energy { get; init; }

    private CharacterMapResurectionAlliesEffect(int id, int energy)
        : base(id)
    {
        Energy = energy;
    }

    internal static CharacterMapResurectionAlliesEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Energy);
    }
}
