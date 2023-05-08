using Cyberia.Api.DatacenterNS;
using Cyberia.Api.Managers;

namespace Cyberia.Api.Factories.Effects
{
    public sealed class LearnEmoteEffect : BasicEffect
    {
        public int EmoteId { get; init; }

        public LearnEmoteEffect(int effectId, EffectParameters parameters, int duration, int probability, Area area) :
            base(effectId, parameters, duration, probability, area)
        {
            EmoteId = parameters.Param3;
        }

        public static new LearnEmoteEffect Create(int effectId, EffectParameters parameters, int duration, int probability, Area area)
        {
            return new(effectId, parameters, duration, probability, area);
        }

        public Emote? GetEmote()
        {
            return DofusApi.Instance.Datacenter.EmotesData.GetEmoteById(EmoteId);
        }

        public override string GetDescription()
        {
            string emoteName = DofusApi.Instance.Datacenter.EmotesData.GetEmoteNameById(EmoteId);

            return GetDescriptionFromParameters(null, null, emoteName);
        }
    }
}
