using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record AddTTGCardToBinderEffect : Effect
{
    public int TTGCardId { get; init; }

    private AddTTGCardToBinderEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea, int ttgCardId)
        : base(id, duration, probability, criteria, dispellable, effectArea)
    {
        TTGCardId = ttgCardId;
    }

    internal static AddTTGCardToBinderEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, bool dispellable, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, dispellable, effectArea, (int)parameters.Param3);
    }

    public TTGCardData? GetTTGCardData()
    {
        return DofusApi.Datacenter.TTGRepository.GetTTGCardDataById(TTGCardId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var ttgCard = GetTTGCardData();
        var ttgEntityName = ttgCard is null
            ? $"{nameof(TTGCardData)} {Translation.UnknownData(TTGCardId, culture)}"
            : DofusApi.Datacenter.TTGRepository.GetTTGEntityNameById(ttgCard.TTGEntityId, culture);

        return GetDescription(culture, string.Empty, string.Empty, ttgEntityName);
    }
}
