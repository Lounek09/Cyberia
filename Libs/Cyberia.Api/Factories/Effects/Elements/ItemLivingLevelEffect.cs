using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemLivingLevelEffect : Effect
{
    public int Experience { get; init; }

    private ItemLivingLevelEffect(int id, int experience)
        : base(id)
    {
        Experience = experience;
    }

    internal static ItemLivingLevelEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Experience);
    }
}
