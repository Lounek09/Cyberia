using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterSpellReflectorEffect : Effect
{
    public int Level { get; init; }

    private CharacterSpellReflectorEffect(int id, int level)
        : base(id)
    {
        Level = level;
    }

    internal static CharacterSpellReflectorEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, Level);
    }
}
