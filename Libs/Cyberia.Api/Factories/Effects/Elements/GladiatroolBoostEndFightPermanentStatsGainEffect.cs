using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolBoostEndFightPermanentStatsGainEffect : Effect, ICharacteristicEffect
{
    public int CharacteristicId { get; init; }
    public int BoostPercent { get; init; }

    private GladiatroolBoostEndFightPermanentStatsGainEffect(int id, int characteristicId, int boostPercent)
        : base(id)
    {
        CharacteristicId = characteristicId;
        BoostPercent = boostPercent;
    }

    internal static GladiatroolBoostEndFightPermanentStatsGainEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param1, (int)parameters.Param3);
    }

    public Element? GetElement()
    {
        return Element.FromCharacteristicId(CharacteristicId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var element = GetElement();
        var elementName = element.HasValue
            ? element.Value.GetDescription(culture)
            : Translation.UnknownData(CharacteristicId, culture);

        return GetDescription(culture, elementName, string.Empty, BoostPercent);
    }
}
