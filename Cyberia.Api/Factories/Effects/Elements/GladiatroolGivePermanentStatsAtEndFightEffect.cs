using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolGivePermanentStatsAtEndFightEffect : Effect, ICharacteristicEffect
{
    public int CharacteristicId { get; init; }
    public int Amount { get; init; }

    private GladiatroolGivePermanentStatsAtEndFightEffect(int id, int characteristicId, int amount)
        : base(id)
    {
        CharacteristicId = characteristicId;
        Amount = amount;
    }

    internal static GladiatroolGivePermanentStatsAtEndFightEffect Create(int effectId, EffectParameters parameters)
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

        return GetDescription(culture, elementName, string.Empty, Amount);
    }
}
