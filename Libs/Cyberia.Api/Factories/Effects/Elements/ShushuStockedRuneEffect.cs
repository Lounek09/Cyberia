using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ShushuStockedRuneEffect : Effect
{
    public int Amont { get; init; }

    private ShushuStockedRuneEffect(int id, int amont)
        : base(id)
    {
        Amont = amont;
    }

    internal static ShushuStockedRuneEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, string.Empty, Amont);
    }
}
