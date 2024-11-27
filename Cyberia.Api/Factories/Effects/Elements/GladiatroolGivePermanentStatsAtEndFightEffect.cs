using Cyberia.Api.Enums;
using Cyberia.Api.Extensions;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record GladiatroolGivePermanentStatsAtEndFightEffect : Effect
{
    public int CharacteristicId { get; init; }
    public int Amount { get; init; }

    private GladiatroolGivePermanentStatsAtEndFightEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int characteristicId, int amount)
        : base(id, duration, probability, criteria, effectArea)
    {
        CharacteristicId = characteristicId;
        Amount = amount;
    }

    internal static GladiatroolGivePermanentStatsAtEndFightEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param1, (int)parameters.Param3);
    }

    public Element? GetElement()
    {
        return ElementExtensions.GetFromCharacteristicId(CharacteristicId);
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
