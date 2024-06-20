using Cyberia.Api.Data.Emotes;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Factories.EffectAreas;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLearnEmoteEffect : Effect, IEffect
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
        return DofusApi.Datacenter.EmotesRepository.GetEmoteById(EmoteId);
    }

    public Description GetDescription()
    {
        var emoteName = DofusApi.Datacenter.EmotesRepository.GetEmoteNameById(EmoteId);

        return GetDescription(string.Empty, string.Empty, emoteName);
    }
}
