using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record ItemChangeDurabilityEffect : Effect
{
    public int CurrentDurability { get; init; }
    public int MaxDurability { get; init; }

    private ItemChangeDurabilityEffect(int id, int currentDurability, int maxDurability)
        : base(id)
    {
        CurrentDurability = currentDurability;
        MaxDurability = maxDurability;
    }

    internal static ItemChangeDurabilityEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param2, (int)parameters.Param3);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        return GetDescription(culture, string.Empty, CurrentDurability, MaxDurability);
    }
}
