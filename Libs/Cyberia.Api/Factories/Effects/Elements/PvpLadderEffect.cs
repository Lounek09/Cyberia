using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record PvpLadderEffect : Effect
{
    public int Count { get; init; }

    private PvpLadderEffect(int id, int count)
        : base(id)
    {
        Count = count;
    }

    internal static PvpLadderEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param2);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, Count);
    }
}
