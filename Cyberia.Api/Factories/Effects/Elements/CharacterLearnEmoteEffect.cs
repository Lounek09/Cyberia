using Cyberia.Api.Data.Emotes;
using Cyberia.Api.Factories.Criteria.Elements;
using Cyberia.Api.Factories.EffectAreas;

using System.Globalization;

namespace Cyberia.Api.Factories.Effects.Elements;

public sealed record CharacterLearnEmoteEffect : Effect
{
    public int EmoteId { get; init; }

    private CharacterLearnEmoteEffect(int id, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea, int emoteId)
        : base(id, duration, probability, criteria, effectArea)
    {
        EmoteId = emoteId;
    }

    internal static CharacterLearnEmoteEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaReadOnlyCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, (int)parameters.Param3);
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
