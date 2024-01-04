using Cyberia.Api.Data.Emotes;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects;

public sealed record CharacterLearnEmoteEffect
    : Effect, IEffect
{
    public int EmoteId { get; init; }

    private CharacterLearnEmoteEffect(int id, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea, int emoteId)
        : base(id, duration, probability, criteria, effectArea)
    {
        EmoteId = emoteId;
    }

    internal static CharacterLearnEmoteEffect Create(int effectId, EffectParameters parameters, int duration, int probability, CriteriaCollection criteria, EffectArea effectArea)
    {
        return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
    }

    public EmoteData? GetEmoteData()
    {
        return DofusApi.Datacenter.EmotesData.GetEmoteById(EmoteId);
    }

    public Description GetDescription()
    {
        var emoteName = DofusApi.Datacenter.EmotesData.GetEmoteNameById(EmoteId);

        return GetDescription(string.Empty, string.Empty, emoteName);
    }
}
