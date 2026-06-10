using Cyberia.Api.Data.Emotes;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;
using Cyberia.Api.Factories.Effects.Interfaces;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLearnEmoteEffect : Effect, IEmoteEffect
{
    public int EmoteId { get; init; }

    private CharacterLearnEmoteEffect(int id, int emoteId)
        : base(id)
    {
        EmoteId = emoteId;
    }

    internal static CharacterLearnEmoteEffect Create(int effectId, EffectParameters parameters)
    {
        return new(effectId, (int)parameters.Param3);
    }

    public EmoteData? GetEmoteData()
    {
        return DofusApi.Datacenter.EmotesRepository.GetEmoteDataById(EmoteId);
    }

    public override DescriptionString GetDescription(CultureInfo? culture = null)
    {
        var emoteName = DofusApi.Datacenter.EmotesRepository.GetEmoteNameById(EmoteId, culture);

        return GetDescription(culture, string.Empty, string.Empty, emoteName);
    }
}
