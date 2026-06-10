using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record HuntTargetLevelEffect : Effect
{
    public int Level { get; init; }

    private HuntTargetLevelEffect(int id, int level)
        : base(id)
    {
        Level = level;
    }

    internal static HuntTargetLevelEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Level);
    }
}
