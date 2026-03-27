using Cyberia.Api.Data.TTG;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record AddTTGCardToBinderEffect : Effect, ITTGCardEffect
{
    public int TTGCardId { get; init; }

    private AddTTGCardToBinderEffect(int id, int ttgCardId)
        : base(id)
    {
        TTGCardId = ttgCardId;
    }

    internal static AddTTGCardToBinderEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
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
