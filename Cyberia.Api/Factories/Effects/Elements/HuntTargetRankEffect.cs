using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record HuntTargetRankEffect : Effect
{
    public int Rank { get; init; }

    private HuntTargetRankEffect(int id, int rank)
        : base(id)
    {
        Rank = rank;
    }

    internal static HuntTargetRankEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Rank);
    }
}
