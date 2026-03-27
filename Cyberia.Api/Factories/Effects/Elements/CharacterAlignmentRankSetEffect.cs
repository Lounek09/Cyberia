using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterAlignmentRankSetEffect : Effect
{
    public int Rank { get; init; }

    private CharacterAlignmentRankSetEffect(int id, int rank)
        : base(id)
    {
        Rank = rank;
    }

    internal static CharacterAlignmentRankSetEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Rank);
    }
}
