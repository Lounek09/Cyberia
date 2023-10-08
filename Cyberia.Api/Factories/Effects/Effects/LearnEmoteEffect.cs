using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Factories.Criteria;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed record LearnEmoteEffect : Effect, IEffect<LearnEmoteEffect>
    {
        public int EmoteId { get; init; }

        private LearnEmoteEffect(int effectId, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea, int emoteId) :
            base(effectId, duration, probability, criteria, effectArea)
        {
            EmoteId = emoteId;
        }

        public static LearnEmoteEffect Create(int effectId, EffectParameters parameters, int duration, int probability, List<ICriteriaElement> criteria, EffectArea effectArea)
        {
            return new(effectId, duration, probability, criteria, effectArea, parameters.Param3);
        }

        public EmoteData? GetEmoteData()
        {
            return DofusApi.Instance.Datacenter.EmotesData.GetEmoteById(EmoteId);
        }

        public Description GetDescription()
        {
            string emoteName = DofusApi.Instance.Datacenter.EmotesData.GetEmoteNameById(EmoteId);

            return GetDescription(null, null, emoteName);
        }
    }
}
