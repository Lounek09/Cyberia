using Cyberia.Api.Data.Titles;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record AccountGainTitleEffect : Effect, ITitileEffect
{
    public int TitleId { get; init; }

    private AccountGainTitleEffect(int id, int titleId)
        : base(id)
    {
        TitleId = titleId;
    }

    internal static AccountGainTitleEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public TitleData? GetTitleData()
    {
        return DofusApi.Datacenter.TitlesRepository.GetTitleDataById(TitleId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var titleName = DofusApi.Datacenter.TitlesRepository.GetTitleDescriptionById(TitleId, culture);

        return GetDescription(culture, string.Empty, string.Empty, titleName);
    }
}
