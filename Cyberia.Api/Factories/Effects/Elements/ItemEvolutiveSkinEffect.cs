using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemEvolutiveSkinEffect : Effect
{
    public int Current { get; init; }
    public int Max { get; init; }

    private ItemEvolutiveSkinEffect(int id, int current, int max)
        : base(id)
    {
        Current = current;
        Max = max;
    }

    internal static ItemEvolutiveSkinEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3, (int)parameters.Param1);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, Max, string.Empty, Current);
    }
}
